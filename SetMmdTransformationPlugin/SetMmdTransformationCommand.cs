using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DxMath;
using Linearstar.Keystone.IO.MikuMikuDance;
using Linearstar.MikuMikuMoving.Framework;
using Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Properties;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin
{
	public class SetMmdTransformationCommand : CommandBase
	{
		public override void Run(CommandArgs e)
		{
			var mmds = Process.GetProcessesByName("MikuMikuDance");

			e.Cancel = true;

			try
			{
				if (!mmds.Any())
				{
					MessageBox.Show(this.ApplicationForm, Util.Bilingual
					(
						this.Scene.Language,
						"MikuMikuDance が起動されていません。\r\nMikuMikuDance が起動している状態で実行してください。",
						"Cannot find MikuMikuDance.\r\nPlease start MikuMikuDance to proceed."
					), Util.Bilingual(this.Scene.Language, this.Text, this.EnglishText), MessageBoxButtons.OK, MessageBoxIcon.Information);

					return;
				}

				switch (this.Scene.Mode)
				{
					case EditMode.CameraMode:
					case EditMode.ModelMode:
						break;
					case EditMode.None:
						MessageBox.Show(this.ApplicationForm, Util.Bilingual
						(
							this.Scene.Language,
							"モデルの現在の変形状態およびモデル、カメラ、または照明のキーフレームのみ送信できます。\r\nモデル、カメラ、または照明を選択してください。",
							"Only the current model transformation or model, camera, light keyframes can be sent.\r\nPlease select them first."
						), Util.Bilingual(this.Scene.Language, this.Text, this.EnglishText), MessageBoxButtons.OK, MessageBoxIcon.Information);

						return;
				}

				using (var f = new SetMmdTransformationForm(this.Scene.Language, mmds))
				{
					if (f.ShowDialog(this.ApplicationForm) != DialogResult.OK)
						return;

					if (this.Scene.Mode == EditMode.ModelMode)
					{
						var model = this.Scene.ActiveModel;
						var changedBonesOnly = f.ChangedBonesOnly;
						var vpd = new VpdDocument
						{
							ParentFileName = "miku.osm",
						};
						var vmd = new VmdDocument
						{
							ModelName = model.Name,
						};
						var useVmd = false;
						var frameNumbers = model.Bones.SelectMany(_ => _.Layers)
													  .SelectMany(_ => _.Frames)
													  .Where(_ => _.Selected)
													  .Select(_ => _.FrameNumber)
													  .Concat(model.Morphs.SelectMany(_ => _.Frames)
																		  .Where(_ => _.Selected)
																		  .Select(_ => _.FrameNumber))
													  .DefaultIfEmpty(0)
													  .ToArray();
						var firstFrame = frameNumbers.Min();

						foreach (var i in model.Bones)
						{
							var local = i.CurrentLocalMotion;

							if (!changedBonesOnly ||
								local.Move != Vector3.Zero ||
								local.Rotation != Quaternion.Identity)
								vpd.Bones.Add(new VpdBone
								{
									BoneName = i.Name,
									Position = ToArray(local.Move),
									Quaternion = ToArray(local.Rotation),
								});

							foreach (var j in i.Layers.First().SelectedFrames)
							{
								useVmd = true;

								vmd.BoneFrames.Add(new VmdBoneFrame
								{
									FrameTime = (uint)(j.FrameNumber - firstFrame),
									Name = i.Name,
									Position = ToArray(j.Position),
									Quaternion = ToArray(j.Quaternion),
									RotationInterpolation = ToArray(j.InterpolRA, j.InterpolRB),
									XInterpolation = ToArray(j.InterpolXA, j.InterpolXB),
									YInterpolation = ToArray(j.InterpolYA, j.InterpolYB),
									ZInterpolation = ToArray(j.InterpolZA, j.InterpolZB),
								});
							}
						}

						foreach (var i in model.Morphs)
							foreach (var j in i.SelectedFrames)
							{
								useVmd = true;

								vmd.MorphFrames.Add(new VmdMorphFrame
								{
									FrameTime = (uint)(j.FrameNumber - firstFrame),
									Name = i.Name,
									Weight = j.Weight,
								});
							}

						if (useVmd)
							using (var vmdStream = new MemoryStream())
							{
								vmd.Write(vmdStream);
								vmdStream.Seek(0, SeekOrigin.Begin);

								MmdDrop.DropFile(f.SelectedMmd.MainWindowHandle, new MmdDropFile("TempMotion" + f.SelectedMmd.Id + ".vmd", vmdStream));
							}

						using (var vpdStream = new MemoryStream(Encoding.GetEncoding(932).GetBytes(vpd.GetFormattedText())))
							MmdDrop.DropFile(f.SelectedMmd.MainWindowHandle, new MmdDropFile("TempPose" + f.SelectedMmd.Id + ".vpd", vpdStream)
							{
								Timeout = 500,
							});
					}
					else if (this.Scene.Mode == EditMode.CameraMode)
					{
						var vmd = new VmdDocument
						{
							ModelName = "カメラ・照明\0on Data",
						};

						foreach (var i in this.Scene.Cameras.First().Layers.First().SelectedFrames)
							vmd.CameraFrames.Add(new VmdCameraFrame
							{
								FrameTime = (uint)i.FrameNumber,
								Position = ToArray(i.Position),
								Angle = ToArray(new Vector3(-i.Angle.X, (float)(i.Angle.Y - Math.PI), i.Angle.Z)),
								FovInDegree = (int)MathHelper.ToDegrees(i.Fov),
								Ortho = /* TODO: get perspective */ false,
								Radius = -i.Radius,
								AngleInterpolation = ToArray(i.InterpolRoteA, i.InterpolRoteB),
								XInterpolation = ToArray(i.InterpolMoveA, i.InterpolMoveB),
								YInterpolation = ToArray(i.InterpolMoveA, i.InterpolMoveB),
								ZInterpolation = ToArray(i.InterpolMoveA, i.InterpolMoveB),
								RadiusInterpolation = ToArray(i.InterpolDistA, i.InterpolDistB),
								FovInterpolation = ToArray(i.InterpolFovA, i.InterpolFovB),
							});

						foreach (var i in this.Scene.Lights.First().SelectedFrames)
							vmd.LightFrames.Add(new VmdLightFrame
							{
								FrameTime = (uint)i.FrameNumber,
								Position = ToArray(-i.Position / 100),
								Color = ToArray(i.Color),
							});

						using (var vmdStream = new MemoryStream())
						{
							vmd.Write(vmdStream);
							vmdStream.Seek(0, SeekOrigin.Begin);

							MmdDrop.DropFile(f.SelectedMmd.MainWindowHandle, new MmdDropFile("TempMotion" + f.SelectedMmd.Id + ".vmd", vmdStream));
						}
					}
				}
			}
			finally
			{
				foreach (var i in mmds)
					i.Dispose();
			}
		}

		static VmdInterpolationPoint[] ToArray(InterpolatePoint a, InterpolatePoint b)
		{
			return new[] { new VmdInterpolationPoint((byte)a.X, (byte)a.Y), new VmdInterpolationPoint((byte)b.X, (byte)b.Y) };
		}

		static float[] ToArray(Vector3 value)
		{
			return new[] { value.X, value.Y, value.Z };
		}

		static float[] ToArray(Quaternion value)
		{
			return new[] { value.X, value.Y, value.Z, value.W };
		}

		public override Image Image
		{
			get
			{
				return Resources.SetMmdTransformation32;
			}
		}

		public override Image SmallImage
		{
			get
			{
				return Resources.SetMmdTransformation20;
			}
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Set MMD\r\nTransformation");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("MMD\r\nポーズ設定");
			}
		}

		public override string Description
		{
			get
			{
				return Util.Bilingual(this.Scene.Language, "現在の変形状態を MMD で現在選択されているモデルに設定します。", " Set current transformation to the current model on MMD.");
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("a3467169-214b-42a5-8249-45813043d12c");
			}
		}
	}
}
