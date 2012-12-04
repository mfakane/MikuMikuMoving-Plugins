using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.ApplyRotationPlugin.Properties;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin
{
	public class ApplyRotationPlugin : CommandBase
	{
		Vector3 position;
		Vector3 rotation;
		bool isMoveOnly;

		public override void Run(CommandArgs e)
		{
			var mode = this.Scene.Mode;
			Action reset = null;

			if (mode != EditMode.ModelMode && mode != EditMode.AccessoryMode && mode != EditMode.EffectMode ||
				mode == EditMode.ModelMode && this.Scene.ActiveModel == null ||
				mode == EditMode.AccessoryMode && this.Scene.ActiveAccessory == null ||
				mode == EditMode.EffectMode && this.Scene.ActiveEffect == null)
			{
				MessageBox.Show(this.ApplicationForm, Util.Bilingual
				(
					this.Scene.Language,
					"対象のモデル、アクセサリ、またはエフェクトがありません。\r\n対象のキーフレームを選択してから実行してください。",
					"No target selected.\r\nPlease select one or more keyframes to rotate."
				), Util.Bilingual(this.Scene.Language, this.Text, this.EnglishText), MessageBoxButtons.OK, MessageBoxIcon.Information);

				e.Cancel = true;

				return;
			}

			using (new EnableScreenObjectBlock(this.Scene))
			using (var image = new ScreenImage_2D(Point.Empty, Resources.Origin))
			using (var f = new ApplyRotationForm(this.Scene.Language)
			{
				Position = position,
				Rotation = rotation,
				IsMoveOnly = isMoveOnly,
			})
				try
				{
					this.Scene.ScreenObjects.Add(image);
					f.ValueChanged += (sender, e2) =>
					{
						var camera = this.Scene.ActiveCamera ?? this.Scene.Cameras.First();
						var pt = Vector3.Project
						(
							f.Position,
							0,
							0,
							this.Scene.ScreenSize.Width,
							this.Scene.ScreenSize.Height,
							this.Scene.SystemInformation.NearPlane,
							this.Scene.SystemInformation.FarPlane,
							camera.ViewMatrix * camera.ProjectionMatrix
						);

						image.Position = new Point((int)pt.X - image.Size.Width / 2, (int)pt.Y - image.Size.Height / 2);
						Apply(mode, f, true, ref reset);
					};

					var rt = f.ShowDialog(this.ApplicationForm);

					if (reset != null)
					{
						reset();
						reset = null;
					}

					if (rt != DialogResult.OK)
					{
						e.Cancel = true;

						return;
					}

					position = f.Position;
					rotation = f.Rotation;
					isMoveOnly = f.IsMoveOnly;

					using (new UndoBlock(this.Scene))
						Apply(mode, f, false, ref reset);
				}
				finally
				{
					this.Scene.ScreenObjects.Remove(image);
				}
		}

		void Apply(EditMode mode, ApplyRotationForm f, bool previewOnly, ref Action reset)
		{
			var rotate = !f.IsMoveOnly;
			var quaternion = Quaternion.RotationYawPitchRoll(MathHelper.ToRadians(f.Rotation.Y), MathHelper.ToRadians(f.Rotation.X), MathHelper.ToRadians(f.Rotation.Z));
			var offset = Matrix.Translation(-f.Position) *
						 Matrix.RotationQuaternion(quaternion) *
						 Matrix.Translation(f.Position);

			if (reset != null)
			{
				reset();
				reset = null;
			}

			switch (mode)
			{
				case EditMode.AccessoryMode:
					var acc = this.Scene.ActiveAccessory;

					if (acc != null)
					{
						foreach (var i in acc.SelectedLayers)
						{
							var local = i.CurrentLocalMotion;
							var newLocal = new MotionData
							(
								Vector3.TransformCoordinate(local.Move, offset),
								rotate ? Quaternion.Multiply(quaternion, local.Rotation) : local.Rotation
							);

							reset += () => i.CurrentLocalMotion = local;
							i.CurrentLocalMotion = newLocal;
						}

						if (!previewOnly)
							foreach (var i in acc.Layers.SelectMany(_ => _.SelectedFrames))
							{
								var resetPosition = i.Position;
								var resetQuaternion = i.Quaternion;

								reset += () =>
								{
									i.Position = resetPosition;
									i.Quaternion = resetQuaternion;
								};

								if (rotate)
									i.Quaternion = Quaternion.Multiply(quaternion, i.Quaternion);

								i.Position = Vector3.TransformCoordinate(i.Position, offset);
							}
					}

					break;
				case EditMode.EffectMode:
					var eff = this.Scene.ActiveEffect;

					if (eff != null)
					{
						var local = eff.CurrentMotion;
						var newLocal = new MotionData
						(
							Vector3.TransformCoordinate(local.Move, offset),
							rotate ? Quaternion.Multiply(quaternion, local.Rotation) : local.Rotation
						);

						reset += () => eff.CurrentMotion = local;
						eff.CurrentMotion = newLocal;

						if (!previewOnly)
							foreach (var i in eff.SelectedFrames)
							{
								var resetPosition = i.Position;
								var resetQuaternion = i.Quaternion;

								reset += () =>
								{
									i.Position = resetPosition;
									i.Quaternion = resetQuaternion;
								};

								if (rotate)
									i.Quaternion = Quaternion.Multiply(quaternion, i.Quaternion);

								i.Position = Vector3.TransformCoordinate(i.Position, offset);
							}
					}

					break;
				case EditMode.ModelMode:
					var model = this.Scene.ActiveModel;

					if (model != null)
						foreach (var bone in model.Bones)
						{
							var initialPosition = Matrix.Translation(bone.InitialPosition);

							if (bone.ParentBoneID != -1)
								initialPosition *= GetMotion(model.Bones, bone);

							var offsetWithInitialPosition = initialPosition * offset * Matrix.Invert(initialPosition);
							Vector3 scale;
							Quaternion rotation;
							Vector3 translation;

							offsetWithInitialPosition.Decompose(out scale, out rotation, out translation);

							foreach (var i in bone.SelectedLayers)
							{
								var local = i.CurrentLocalMotion;
								var newLocal = new MotionData
								(
									(bone.BoneFlags & BoneType.XYZ) != 0 ? Vector3.TransformCoordinate(local.Move, offsetWithInitialPosition) : local.Move,
									(bone.BoneFlags & BoneType.Rotate) != 0 && rotate ? Quaternion.Multiply(rotation, local.Rotation) : local.Rotation
								);

								reset += () => i.CurrentLocalMotion = local;
								i.CurrentLocalMotion = newLocal;
							}

							if (!previewOnly)
								foreach (var i in bone.Layers.SelectMany(_ => _.SelectedFrames))
								{
									var resetPosition = i.Position;
									var resetQuaternion = i.Quaternion;

									reset += () =>
									{
										i.Position = resetPosition;
										i.Quaternion = resetQuaternion;
									};

									if (rotate)
										i.Quaternion = Quaternion.Multiply(quaternion, i.Quaternion);

									i.Position = Vector3.TransformCoordinate(i.Position, offsetWithInitialPosition);
								}
						}

					break;
			}
		}

		Matrix GetMotion(BoneCollection bones, Bone bone)
		{
			var b = bones[bone.ParentBoneID];
			var rt = Matrix.RotationQuaternion(b.CurrentLocalMotion.Rotation) * Matrix.Translation(b.CurrentLocalMotion.Move + b.InitialPosition);

			if (b.ParentBoneID != -1)
				rt *= GetMotion(bones, b);

			return rt;
		}

		public override Image Image
		{
			get
			{
				return Resources.ApplyRotationPlugin32;
			}
		}

		public override Image SmallImage
		{
			get
			{
				return Resources.ApplyRotationPlugin20;
			}
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Apply\r\nRotation");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("任意中心\r\n回転付加");
			}
		}

		public override string Description
		{
			get
			{
				return Util.Bilingual(this.Scene.Language, "任意の点を中心に位置および角度を回転します。", "Rotates the selected motion by the specified rotation and origin.");
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("fd853cb9-6b70-4bed-98ce-906e0431db5e");
			}
		}
	}
}
