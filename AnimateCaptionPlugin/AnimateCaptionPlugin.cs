using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Properties;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public class AnimateCaptionPlugin : ResidentBase, IHaveUserControl, ICanSavePlugin
	{
		readonly Dictionary<object, Action<ICaption>> list = new Dictionary<object, Action<ICaption>>();
		readonly Dictionary<object, AnimationData> datas = new Dictionary<object, AnimationData>();
		readonly AnimateCaptionControl control = new AnimateCaptionControl
		{
			IsPluginEnabled = false,
			Caption = null,
		};
		object activeCaption;

		public override void Update(float frame, float elapsedTime)
		{
			foreach (var i in datas.Keys.Except(this.Scene.Captions.Select(_ => _.GetRealCaption())).ToArray())
				datas.Remove(i);

			if (datas.ContainsValue(control.AnimationData))
				control.SetNewValues();

			ResetAnimation();

			var newActiveCaption = this.Scene.SelectedCaptions.FirstOrDefault();

			if (newActiveCaption == null)
			{
				activeCaption = null;
				control.IsAnimationEnabled = false;
				control.AnimationData = null;
			}
			else
			{
				var newActiveRealCaption = newActiveCaption.GetRealCaption();
				var data = GetAnimationData(newActiveCaption, newActiveRealCaption);

				if (newActiveRealCaption == activeCaption)
				{
					if (data == null && control.IsAnimationEnabled)
						data = CreateAnimationData(newActiveCaption, newActiveRealCaption);
					else if (data != null && !control.IsAnimationEnabled)
					{
						data.ApplyAnimation(newActiveCaption, (float)newActiveCaption.StartFrame);
						datas.Remove(newActiveRealCaption);
						data = null;
					}
				}

				control.IsAnimationEnabled = data != null;
				control.AnimationData = data;
				activeCaption = newActiveRealCaption;
			}

			control.Caption = newActiveCaption;
			control.CurrentFrame = frame;
			ApplyAnimation(frame);
		}

		void ApplyAnimation(float frame)
		{
			foreach (var i in this.Scene.Captions.Select((_, idx) => new
			{
				Caption = _,
				_.StartFrame,
				EndFrame = _.StartFrame + _.DurationFrame,
			}).Where(_ => frame >= _.StartFrame && frame <= _.EndFrame))
			{
				var realCaption = i.Caption.GetRealCaption();

				if (datas.ContainsKey(realCaption))
					list.Add(realCaption, datas[realCaption].ApplyAnimation(i.Caption, frame));
			}
		}

		void ResetAnimation()
		{
			foreach (var i in this.Scene.Captions.Select(_ => new
			{
				Caption = _,
				RealCaption = _.GetRealCaption(),
			}))
				if (list.ContainsKey(i.RealCaption) && datas.ContainsKey(i.RealCaption))
					list[i.RealCaption](i.Caption);

			list.Clear();
		}

		AnimationData GetAnimationData(ICaption caption, object realCaption)
		{
			return datas.ContainsKey(realCaption) ? datas[realCaption] : null;
		}

		AnimationData CreateAnimationData(ICaption caption, object realCaption)
		{
			return datas[realCaption] = new AnimationData(caption);
		}

		public void OnLoadProject(Stream stream)
		{
			using (var br = new BinaryReader(stream))
			{
				var version = br.ReadByte();

				datas.Clear();

				foreach (var i in Enumerable.Range(0, br.ReadInt32())
											.Select(_ => AnimationData.Parse(version, br)))
					datas.Add(this.Scene.Captions[i.Id].GetRealCaption(), i);
			}
		}

		public Stream OnSaveProject()
		{
			using (var bw = new BinaryWriter(new MemoryStream()))
			{
				bw.Write((byte)1);
				bw.Write(datas.Count);

				foreach (var i in this.Scene.Captions.Select((_, idx) => new
				{
					Index = idx,
					Item = _,
				}))
				{
					var real = i.Item.GetRealCaption();

					if (datas.ContainsKey(real))
					{
						var data = datas[real];

						data.Id = i.Index;
						data.Write(bw);
					}
				}

				return new MemoryStream(((MemoryStream)bw.BaseStream).ToArray());
			}
		}

		public override void Initialize()
		{
			control.Scene = this.Scene;
		}

		public override void Enabled()
		{
			control.IsPluginEnabled = true;
			control.Caption = null;
		}

		public override void Disabled()
		{
			ResetAnimation();
			control.IsPluginEnabled = false;
		}

		public override Image Image
		{
			get
			{
				return Resources.AnimateCaptionPlugin32;
			}
		}

		public override Image SmallImage
		{
			get
			{
				return Resources.AnimateCaptionPlugin20;
			}
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Animate\r\nCaptions");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("字幕\r\nアニメーション");
			}
		}

		public override string Description
		{
			get
			{
				return "字幕の座標や角度などをアニメーションできます。";
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("03f63956-256a-434f-8d45-3f9745faf6fa");
			}
		}

		public UserControl CreateControl()
		{
			return control;
		}
	}
}
