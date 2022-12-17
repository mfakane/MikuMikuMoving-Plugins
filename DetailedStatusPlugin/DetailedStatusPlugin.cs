using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.DetailedStatusPlugin
{
	public class DetailedStatusPlugin : ResidentBase
	{
		ScreenImage_2D image;
		string previousText;

		public override void Initialize()
		{
			image = new UnclickableScreenImage2D
			{
				Image = CreateStatusImage(""),
			};
			this.Scene.ScreenObjects.Add(image);
		}

		public override void Update(float frame, float elapsedTime)
		{
			if (!(image.Visible = this.Scene.State == SceneState.Editing))
				return;

			var text = GetStatusText((long)frame);

			if (text != previousText)
			{
				this.Scene.ScreenObjects.Remove(image);
				image.Image.Dispose();

				image.Image = CreateStatusImage(text);
				this.Scene.ScreenObjects.Add(image);
			}

			previousText = text;
			image.Position = new Point(this.Scene.SystemInformation.BoneOperationControlDock == ScreenDock.BottomLeft ? 96 : 1, this.Scene.ScreenSize.Height - image.Image.Height + 2);
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
								p.AddString(s, font.FontFamily, (int)font.Style, font.Size, new Point(0, y), StringFormat.GenericDefault);
								g.DrawPath(lgp2, p);
								g.DrawPath(lgp, p);
								g.FillPath(lgb, p);
								y += rowHeight;
							}
					}

				return b;
			}
		}

		string GetStatusText(long frame)
		{
			switch (this.Scene.Mode)
			{
				case EditMode.AccessoryMode:
					var acc = this.Scene.ActiveAccessory;

					if (acc != null)
					{
						var selectedLayer = acc.SelectedLayers.FirstOrDefault() ?? acc.Layers.First();
						var accData = selectedLayer.CurrentLocalMotion;

						return FormatVector3(Util.Bilingual(this.Scene.Language, "アクセサリ ", "Accessory ") + selectedLayer.Name, MathHelper.ToDegrees(MathHelper.ToEulerAngle(accData.Rotation)), accData.Move);
					}

					break;
				case EditMode.EffectMode:
					var effect = this.Scene.ActiveEffect;

					if (effect != null)
						return FormatVector3(Util.Bilingual(this.Scene.Language, "エフェクト ", "Effect "), MathHelper.ToDegrees(MathHelper.ToEulerAngle(effect.CurrentLocalMotion.Rotation)), effect.CurrentLocalMotion.Move);

					break;
				case EditMode.ModelMode:
					var model = this.Scene.ActiveModel;

					if (model != null)
					{
						var selectedBone = model.Bones.FirstOrDefault(_ => _.SelectedLayers.Any());

						if (selectedBone != null)
						{
							var selectedLayer = selectedBone.SelectedLayers.First();
							var boneData = selectedLayer.CurrentLocalMotion;
							var layerName = selectedLayer.LayerID.ToString();

							// NOTE: Name で例外が出ることへの暫定対策
							try
							{
								layerName = selectedLayer.Name;
							}
							catch
							{
							}

							return FormatVector3(Util.Bilingual(this.Scene.Language, selectedBone.Name, selectedBone.EnglishName) + " " + layerName, MathHelper.ToDegrees(MathHelper.ToEulerAngle(boneData.Rotation)), (selectedBone.BoneFlags & BoneType.XYZ) != 0 ? (Vector3?)boneData.Move : null);
						}
					}

					break;
			}

			var selectedCamera = this.Scene.Cameras.SelectMany(_ => _.Layers).FirstOrDefault(_ => _.SelectedFrames.Any()) ?? this.Scene.Cameras.First().Layers.First();
			var cameraData = selectedCamera.CurrentLocalMotion;

			return FormatVector3(Util.Bilingual(this.Scene.Language, "カメラ ", "Camera ") + selectedCamera.Name, MathHelper.ToDegrees(cameraData.Angle), cameraData.Position);
		}

		string FormatVector3(string name, Vector3 rotation, Vector3? position = null)
		{
			const string format = "X:{0:0.000000}, Y:{1:0.000000}, Z:{2:0.000000}";

			if (position.HasValue)
				return name + "\r\n" + Util.Bilingual(this.Scene.Language, "位置 ", "Pos ") + string.Format(format, position.Value.X, position.Value.Y, position.Value.Z) + "\r\n" + Util.Bilingual(this.Scene.Language, "角度 ", "Ang ") + string.Format(format, rotation.X, rotation.Y, rotation.Z);
			else
				return name + "\r\n" + Util.Bilingual(this.Scene.Language, "角度 ", "Ang ") + string.Format(format, rotation.X, rotation.Y, rotation.Z);
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Detailed\r\nStatus");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("詳細\r\n情報");
			}
		}

		public override string Description
		{
			get
			{
				return Util.Bilingual(this.Scene.Language, "選択されたボーンなどのオブジェクトの位置および回転を表示します。", "Shows detailed position/rotation information for selected bone or camera.");
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("f5baf169-b0f1-4809-bf89-35a972046f3a");
			}
		}
	}
}
