using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Properties;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin
{
	public class BeatBasedCopyCommand : CommandBase
	{
		int beginFrame;
		float beatsPerMinute = 120;
		float startupBeats;
		float intervalBeats = 1;
		int times = 4;

		public override void Run(CommandArgs e)
		{
			var found = false;

			switch (this.Scene.Mode)
			{
				case EditMode.AccessoryMode:
					found = this.Scene.ActiveAccessory != null && this.Scene.ActiveAccessory.Layers.SelectMany(_ => _.SelectedFrames).Any();

					break;
				case EditMode.CameraMode:
					found = true;

					break;
				case EditMode.EffectMode:
					found = this.Scene.ActiveEffect != null && this.Scene.ActiveEffect.SelectedFrames.Any();

					break;
				case EditMode.ModelMode:
					found = this.Scene.ActiveModel != null && this.Scene.ActiveModel.Bones.SelectMany(_ => _.Layers).SelectMany(_ => _.SelectedFrames).Any();

					break;
			}

			if (!found)
			{
				MessageBox.Show(this.ApplicationForm, "対象のキーフレームがありません。\r\n対象のボーン、アクセサリ、カメラ、またはエフェクトのキーフレームを選択してから実行してください。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

				e.Cancel = true;

				return;
			}

			using (var f = new BeatBasedCopyForm
			{
				BeginFrame = beginFrame,
				BeatsPerMinute = beatsPerMinute,
				StartupBeats = startupBeats,
				IntervalBeats = intervalBeats,
				Times = times,
			})
			{
				if (f.ShowDialog(this.ApplicationForm) != DialogResult.OK)
				{
					e.Cancel = true;

					return;
				}

				beginFrame = f.BeginFrame;
				beatsPerMinute = f.BeatsPerMinute;
				startupBeats = f.StartupBeats;
				intervalBeats = f.IntervalBeats;
				times = f.Times;
			}

			using (new UndoBlock(this.Scene))
				switch (this.Scene.Mode)
				{
					case EditMode.AccessoryMode:
						var acc = this.Scene.ActiveAccessory;

						if (acc != null)
						{
							var frames = acc.Layers.SelectMany(_ => _.SelectedFrames.Select(f => new
							{
								Layer = _,
								Frame = f,
							})).ToArray();
							var frameStart = frames.Min(_ => _.Frame.FrameNumber);

							foreach (var i in CopyByBeat().SelectMany(f => frames.Select(_ => new
							{
								_.Layer,
								Frame = new MotionFrameData(_.Frame.FrameNumber - frameStart + f, _.Frame.Position, _.Frame.Quaternion)
								{
									InterpolRA = _.Frame.InterpolRA,
									InterpolRB = _.Frame.InterpolRB,
									InterpolXA = _.Frame.InterpolXA,
									InterpolXB = _.Frame.InterpolXB,
									InterpolYA = _.Frame.InterpolYA,
									InterpolYB = _.Frame.InterpolYB,
									InterpolZA = _.Frame.InterpolZA,
									InterpolZB = _.Frame.InterpolZB,
									Selected = true,
								},
							})).GroupBy(_ => _.Layer))
								i.Key.Frames.AddKeyFrame(i.Select(_ => _.Frame).ToList());
						}

						break;
					case EditMode.CameraMode:
						var camera = this.Scene.ActiveCamera;

						if (camera != null)
						{
							var frames = camera.Layers.SelectMany(_ => _.Frames.Select(f => new
							{
								Layer = _,
								Frame = f,
							})).ToArray();
							var frameStart = frames.Min(_ => _.Frame.FrameNumber);

							foreach (var i in CopyByBeat().SelectMany(f => frames.Select(_ => new
							{
								_.Layer,
								Frame = new CameraFrameData(_.Frame.FrameNumber - frameStart + f, _.Frame.Position, _.Frame.Angle, _.Frame.Radius, _.Frame.Fov)
								{
									InterpolDistA = _.Frame.InterpolDistA,
									InterpolDistB = _.Frame.InterpolDistB,
									InterpolFovA = _.Frame.InterpolFovA,
									InterpolFovB = _.Frame.InterpolFovB,
									InterpolMoveA = _.Frame.InterpolMoveA,
									InterpolMoveB = _.Frame.InterpolMoveB,
									InterpolRoteA = _.Frame.InterpolRoteA,
									InterpolRoteB = _.Frame.InterpolRoteB,
									Selected = true,
								},
							})).GroupBy(_ => _.Layer))
								i.Key.Frames.AddKeyFrame(i.Select(_ => _.Frame).ToList());
						}

						foreach (var light in this.Scene.Lights)
							if (light.SelectedFrames.Any())
							{
								var frames = light.Frames.ToArray();
								var frameStart = frames.Min(_ => _.FrameNumber);

								light.Frames.AddKeyFrame(CopyByBeat().SelectMany(f => frames.Select(_ => new LightFrameData(_.FrameNumber - frameStart + f, _.Position, _.Color)
								{
									InterpolColorA = _.InterpolColorA,
									InterpolColorB = _.InterpolColorB,
									InterpolPosA = _.InterpolPosA,
									InterpolPosB = _.InterpolPosB,
									Selected = true,
								})).ToList());
							}

						break;
					case EditMode.EffectMode:
						var eff = this.Scene.ActiveEffect;

						if (eff != null)
						{
							var frames = eff.SelectedFrames.ToArray();
							var frameStart = frames.Min(_ => _.FrameNumber);

							eff.Frames.AddKeyFrame(CopyByBeat().SelectMany(f => frames.Select(_ => new MotionFrameData(_.FrameNumber - frameStart + f, _.Position, _.Quaternion)
							{
								InterpolRA = _.InterpolRA,
								InterpolRB = _.InterpolRB,
								InterpolXA = _.InterpolXA,
								InterpolXB = _.InterpolXB,
								InterpolYA = _.InterpolYA,
								InterpolYB = _.InterpolYB,
								InterpolZA = _.InterpolZA,
								InterpolZB = _.InterpolZB,
								Selected = true,
							})).ToList());
						}
						break;
					case EditMode.ModelMode:
						var model = this.Scene.ActiveModel;

						if (model != null)
						{
							var frames = model.Bones.SelectMany(b => b.Layers).SelectMany(_ => _.SelectedFrames.Select(f => new
							{
								Layer = _,
								Frame = f,
							})).ToArray();
							var frameStart = frames.Min(_ => _.Frame.FrameNumber);

							foreach (var i in CopyByBeat().SelectMany(f => frames.Select(_ => new
							{
								_.Layer,
								Frame = new MotionFrameData(_.Frame.FrameNumber - frameStart + f, _.Frame.Position, _.Frame.Quaternion)
								{
									InterpolRA = _.Frame.InterpolRA,
									InterpolRB = _.Frame.InterpolRB,
									InterpolXA = _.Frame.InterpolXA,
									InterpolXB = _.Frame.InterpolXB,
									InterpolYA = _.Frame.InterpolYA,
									InterpolYB = _.Frame.InterpolYB,
									InterpolZA = _.Frame.InterpolZA,
									InterpolZB = _.Frame.InterpolZB,
									Selected = true,
								},
							})).GroupBy(_ => _.Layer))
								i.Key.Frames.AddKeyFrame(i.Select(_ => _.Frame).ToList());
						}

						break;
				}
		}

		IEnumerable<long> CopyByBeat()
		{
			var fpb = GetFramesPerBeat(beatsPerMinute, this.Scene.KeyFramePerSec);
			var current = this.Scene.MarkerPosition;
			var startAt = startupBeats * fpb;
			var j = 0;

			foreach (var i in GetBeats(fpb).Select(_ => _ + startAt)
										   .Where(_ => current <= _)
										   .Take((int)(times * Math.Max(intervalBeats, 1))))
			{
				if (j == 0)
					yield return (long)i;

				if (++j >= Math.Max(intervalBeats, 1))
					j = 0;
			}
		}

		static double GetFramesPerBeat(float beatsPerMinute, float framesPerSecond)
		{
			return framesPerSecond / (beatsPerMinute / 60.0);
		}

		static IEnumerable<long> GetBeats(double framesPerBeat)
		{
			for (int i = 0; ; i++)
				yield return (long)(framesPerBeat * i);
		}

		public override Image Image
		{
			get
			{
				return Resources.BeatBasedCopyPlugin32;
			}
		}

		public override Image SmallImage
		{
			get
			{
				return Resources.BeatBasedCopyPlugin20;
			}
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Beat based\r\nCopy");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("ビート\r\nコピー");
			}
		}

		public override string Description
		{
			get
			{
				return "選択されたキーフレームを拍単位で位置を指定してコピーします。";
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("e7e50c23-7025-4d89-9ea6-171d023c11ab");
			}
		}
	}
}
