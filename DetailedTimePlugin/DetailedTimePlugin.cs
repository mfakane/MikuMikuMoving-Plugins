using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.DetailedTimePlugin.Properties;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.DetailedTimePlugin
{
	public class DetailedTimePlugin : ResidentBase, ICanSavePlugin, IHaveUserControl
	{
		ScreenImage_2D image;
		string previousText;
		readonly DetailedTimeControl control = new DetailedTimeControl();

		public override void Initialize()
		{
			image = new UnclickableScreenImage2D
			{
				Image = CreateStatusImage(""),
				VisibleWhenPlaying = true,
			};
			this.Scene.ScreenObjects.Add(image);
		}

		public override void Update(float frame, float elapsedTime)
		{
			if (!(image.Visible = this.Scene.State != SceneState.AVI_Rendering))
				return;

			var text = GetStatusText(frame == 0 ? this.Scene.MarkerPosition : (long)frame);

			if (text != previousText)
			{
				this.Scene.ScreenObjects.Remove(image);
				image.Image.Dispose();

				image.Image = CreateStatusImage(text);
				this.Scene.ScreenObjects.Add(image);
			}

			previousText = text;
			image.Position = new Point(this.Scene.ScreenSize.Width - image.Image.Width - 68, 2);
		}

		public override void Dispose()
		{
			this.Scene.ScreenObjects.Remove(image);
			image.Image.Dispose();
			image.Dispose();
			base.Dispose();
		}

		Bitmap CreateStatusImage(string text)
		{
			var fonts = FontFamily.Families.Select(_ => _.Name).ToArray();

			using (var font = new Font(new[] { "ＭＳ ゴシック", "Segoe UI Mono", "Lucida Console" }.First(fonts.Contains), 16, FontStyle.Bold))
			{
				var size = TextRenderer.MeasureText(text, font);
				var b = new Bitmap(Math.Max(size.Width, 1), Math.Max(size.Height, 1));

				if (size.Width > 0 && size.Height > 0)
					using (var g = Graphics.FromImage(b))
					{
						var y = 0;
						var rows = text.Split('\n');
						var rowHeight = size.Height / rows.Length;
						var beginColor = Color.FromArgb(84, 115, 221);
						var endColor = Color.FromArgb(26, 43, 165);

						foreach (var i in rows)
							using (var p = new GraphicsPath())
							using (var lgb = new LinearGradientBrush(new Point(0, y), new Point(0, y + rowHeight), beginColor, endColor))
							using (var lgb2 = new LinearGradientBrush(new Point(0, y), new Point(0, y + rowHeight), Color.FromArgb(25, beginColor), Color.FromArgb(25, endColor)))
							using (var lgp = new Pen(lgb2, 3))
							using (var lgp2 = new Pen(Color.White, 3))
							{
								var s = i.Trim('\r');

								g.TextRenderingHint = TextRenderingHint.AntiAlias;
								g.SmoothingMode = SmoothingMode.AntiAlias;
								p.AddString(s, font.FontFamily, (int)font.Style, font.Size, new Rectangle(0, y, size.Width, rowHeight), new StringFormat
								{
									Alignment = StringAlignment.Far,
								});
								g.DrawPath(lgp2, p);
								g.DrawPath(lgp, p);
								g.FillPath(lgb, p);
								y += rowHeight;
							}
					}

				return b;
			}
		}

		public void OnLoadProject(Stream stream)
		{
			using (var br = new BinaryReader(stream))
			{
				control.BeginFrame = br.ReadInt64();
				control.BeatsPerMinute = br.ReadSingle();
				control.BeatsPerMeasure = br.ReadInt32();
				control.Resolution = br.ReadInt32();
			}
		}

		public Stream OnSaveProject()
		{
			using (var ms = new MemoryStream())
			using (var bw = new BinaryWriter(ms))
			{
				bw.Write(control.BeginFrame);
				bw.Write(control.BeatsPerMinute);
				bw.Write(control.BeatsPerMeasure);
				bw.Write(control.Resolution);

				return new MemoryStream(ms.ToArray());
			}
		}

		public UserControl CreateControl()
		{
			return control;
		}

		string GetStatusText(float frame)
		{
			frame -= control.BeginFrame;

			if (frame < 0)
				return string.Format("{0:hh':'mm':'ss'.'fff} Meas {1,3:0}:{2:0}:{3,4:0}", TimeSpan.Zero, 0, 0, 0);

			var beatsPerMeas = control.BeatsPerMeasure;
			var time = TimeSpan.FromSeconds(frame / this.Scene.KeyFramePerSec);
			var beatFrames = GetFramesPerBeat(control.BeatsPerMinute, this.Scene.KeyFramePerSec);
			var totalBeats = frame / beatFrames;
			var meas = totalBeats / beatsPerMeas + 1;
			var beats = totalBeats % beatsPerMeas + 1;
			var ticks = (totalBeats - (int)totalBeats) * control.Resolution;

			return string.Format("{0:hh':'mm':'ss'.'fff} Meas {1,3:0}:{2:0}:{3,4:0}", time, (int)meas, (int)beats, (int)ticks);
		}

		static double GetFramesPerBeat(float beatsPerMinute, float framesPerSecond)
		{
			return framesPerSecond / (beatsPerMinute / 60.0);
		}

		public override Image Image
		{
			get
			{
				return Resources.DetailedTimePlugin32;
			}
		}

		public override Image SmallImage
		{
			get
			{
				return Resources.DetailedTimePlugin20;
			}
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Detailed\r\nTime");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("詳細\r\n時間");
			}
		}

		public override string Description
		{
			get
			{
				return Util.Bilingual(this.Scene.Language, "詳細時間。", "");
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("cf05d377-8b6e-4254-b67c-240fe2a4fea8");
			}
		}
	}
}
