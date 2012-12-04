using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Properties;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin
{
	public class ApplyOffsetCommand : CommandBase
	{
		public override void Run(CommandArgs e)
		{
			var mode = this.Scene.Mode;
			var isBoneSelected = mode == EditMode.ModelMode && this.Scene.ActiveModel != null && (this.Scene.ActiveModel.Bones.SelectMany(_ => _.SelectedLayers).Any() || this.Scene.ActiveModel.Bones.SelectMany(_ => _.Layers).SelectMany(_ => _.SelectedFrames).Any());
			var isAvailable = new[]
			{
				mode != EditMode.None && mode != EditMode.ModelMode
					|| mode == EditMode.CameraMode
					|| isBoneSelected,
				isBoneSelected || mode == EditMode.AccessoryMode || (mode == EditMode.CameraMode && this.Scene.ActiveCamera != null) || mode == EditMode.EffectMode,
				mode == EditMode.ModelMode && this.Scene.ActiveModel != null && this.Scene.ActiveModel.Morphs.Any(_ => _.Selected || _.SelectedFrames.Any()),
				mode == EditMode.CameraMode && (this.Scene.ActiveCamera != null || this.Scene.Cameras.Any(_ => _.Layers.SelectMany(__ => __.SelectedFrames).Any())),
				mode == EditMode.CameraMode && (this.Scene.ActiveLight != null || this.Scene.Lights.Any(_ => _.SelectedFrames.Any())),
			};
			Action reset = null;

			if (!isAvailable.Any(_ => _))
			{
				if (mode == EditMode.None)
					MessageBox.Show(this.ApplicationForm, "対象がありません。\r\n対象のキーフレームを選択してから実行してください。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

				e.Cancel = true;

				return;
			}

			using (var f = new ApplyOffsetForm
			{
				IsPositionVisible = isAvailable[0],
				IsPositionLocalVisible = mode != EditMode.CameraMode && mode != EditMode.CaptionMode || isAvailable[3],
				IsRotationVisible = isAvailable[1],
				IsRotationLocalVisible = mode == EditMode.ModelMode || mode == EditMode.AccessoryMode || mode == EditMode.EffectMode,
				IsWeightVisible = isAvailable[2],
				IsDistanceVisible = isAvailable[3],
				IsColorVisible = isAvailable[4],
			})
			{
				f.ValueChanged += (sender, e2) => Apply(mode, f, true, ref reset);

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

				using (new UndoBlock(this.Scene))
					Apply(mode, f, false, ref reset);
			}
		}

		static Matrix GetLocalMatrix(Bone bone)
		{
			if ((bone.BoneFlags & BoneType.LocalAxis) == 0 &&
				bone.LocalAxisX == Vector3.UnitX &&
				bone.LocalAxisY == Vector3.UnitY &&
				bone.LocalAxisZ == Vector3.UnitZ)
				return Matrix.Identity;

			return Matrix.LookAtLH(Vector3.Zero, bone.LocalAxisZ, bone.LocalAxisY);
		}

		static Vector3 GetLocalPosition(bool isLocal, Bone bone, Vector3 position, Quaternion quaternion)
		{
			if (!isLocal)
				return position;

			var rt = Vector3.Transform(bone == null ? position : Vector3.TransformCoordinate(position, GetLocalMatrix(bone)), quaternion);

			return new Vector3(rt.X, rt.Y, rt.Z);
		}

		static Quaternion GetLocalRotation(bool isLocal, Bone bone, Vector3 rotation, Quaternion quaternion)
		{
			if (!isLocal ||
				(bone.BoneFlags & BoneType.LocalAxis) == 0 &&
				bone.LocalAxisX == Vector3.UnitX &&
				bone.LocalAxisY == Vector3.UnitY &&
				bone.LocalAxisZ == Vector3.UnitZ)
				return quaternion;

			return Quaternion.Multiply(Quaternion.Multiply(Quaternion.RotationAxis(bone.LocalAxisY, rotation.Y), Quaternion.RotationAxis(bone.LocalAxisX, rotation.X)), Quaternion.RotationAxis(bone.LocalAxisZ, rotation.Z));
		}

		static Vector3 GetLocalCameraPosition(bool isLocal, Vector3 position, Vector3 rotation)
		{
			if (!isLocal)
				return position;

			var rt = Vector3.Transform(new Vector3(-position.X, position.Y, -position.Z), Quaternion.RotationYawPitchRoll(-rotation.Y, -rotation.X, rotation.Z));

			return new Vector3(rt.X, rt.Y, rt.Z);
		}

		void Apply(EditMode mode, ApplyOffsetForm f, bool previewOnly, ref Action reset)
		{
			var offset = new
			{
				f.Position,
				f.IsPositionLocal,
				Rotation = MathHelper.ToRadians(f.Rotation),
				f.IsRotationLocal,
				Quaternion = Quaternion.RotationYawPitchRoll(MathHelper.ToRadians(f.Rotation.Y), MathHelper.ToRadians(f.Rotation.X), MathHelper.ToRadians(f.Rotation.Z)),
				f.Weight,
				f.Distance,
				f.Color,
			};

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
						var rotationOffset = new Lazy<Quaternion>(() => GetLocalRotation(offset.IsRotationLocal, null, offset.Rotation, offset.Quaternion));

						foreach (var i in acc.SelectedLayers)
						{
							var local = i.CurrentLocalMotion;
							var newLocal = new MotionData(local.Move, offset.IsRotationLocal ? Quaternion.Multiply(local.Rotation, offset.Quaternion) : Quaternion.Multiply(offset.Quaternion, local.Rotation));

							newLocal.Move += GetLocalPosition(offset.IsPositionLocal, null, offset.Position, newLocal.Rotation);

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
								i.Quaternion = offset.IsRotationLocal ? Quaternion.Multiply(i.Quaternion, offset.Quaternion) : Quaternion.Multiply(offset.Quaternion, i.Quaternion);
								i.Position += GetLocalPosition(offset.IsPositionLocal, null, offset.Position, i.Quaternion);
							}
					}

					break;
				case EditMode.None:
				case EditMode.CameraMode:
					foreach (var camera in this.Scene.Cameras.Where(_ => _.Layers.SelectMany(__ => __.SelectedFrames).Any()).DefaultIfEmpty(this.Scene.ActiveCamera).Where(_ => _ != null))
					{
						var local = camera.CurrentMotion;
						var newLocal = new CameraMotionData(local.Position, local.Angle + offset.Rotation, local.Radius + offset.Distance, local.Fov);

						newLocal.Position += GetLocalCameraPosition(offset.IsPositionLocal, offset.Position, newLocal.Angle);

						reset += () => camera.CurrentMotion = local;
						camera.CurrentMotion = newLocal;

						if (!previewOnly)
							foreach (var i in camera.Layers.SelectMany(_ => _.SelectedFrames))
							{
								var resetPosition = i.Position;
								var resetAngle = i.Angle;
								var resetRadius = i.Radius;

								reset += () =>
								{
									i.Position = resetPosition;
									i.Angle = resetAngle;
									i.Radius = resetRadius;
								};
								i.Angle += offset.Rotation;
								i.Radius += offset.Distance;
								i.Position += GetLocalCameraPosition(offset.IsPositionLocal, offset.Position, i.Angle);
							}
					}

					foreach (var light in this.Scene.Lights.Where(_ => _.SelectedFrames.Any()).DefaultIfEmpty(this.Scene.ActiveLight).Where(_ => _ != null))
					{
						var local = light.CurrentMotion;

						reset += () => light.CurrentMotion = local;
						light.CurrentMotion = new LightMotionData(local.Position + offset.Position, local.Color + offset.Color);

						if (!previewOnly)
							foreach (var i in light.SelectedFrames)
							{
								var resetPosition = i.Position;
								var resetColor = i.Color;

								reset += () =>
								{
									i.Position = resetPosition;
									i.Color = resetColor;
								};
								i.Position += offset.Position;
								i.Color += offset.Color;
							}
					}

					break;
				case EditMode.CaptionMode:
					foreach (var i in this.Scene.SelectedCaptions)
					{
						var resetLocation = i.Location;

						reset += () => i.Location = resetLocation;
						i.Location += offset.Position;
					}

					break;
				case EditMode.EffectMode:
					var eff = this.Scene.ActiveEffect;

					if (eff != null)
					{
						var local = eff.CurrentMotion;
						var newLocal = new MotionData(local.Move, offset.IsRotationLocal ? Quaternion.Multiply(local.Rotation, offset.Quaternion) : Quaternion.Multiply(offset.Quaternion, local.Rotation));

						newLocal.Move += GetLocalPosition(offset.IsPositionLocal, null, offset.Position, newLocal.Rotation);

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
								i.Quaternion = offset.IsRotationLocal ? Quaternion.Multiply(i.Quaternion, offset.Quaternion) : Quaternion.Multiply(offset.Quaternion, i.Quaternion);
								i.Position += GetLocalPosition(offset.IsPositionLocal, null, offset.Position, i.Quaternion);
							}
					}

					break;
				case EditMode.ModelMode:
					var model = this.Scene.ActiveModel;

					if (model != null)
					{
						foreach (var bone in model.Bones)
						{
							var rotationOffset = new Lazy<Quaternion>(() => GetLocalRotation(offset.IsRotationLocal, bone, offset.Rotation, offset.Quaternion));

							foreach (var i in bone.SelectedLayers)
							{
								var local = i.CurrentLocalMotion;
								var newLocal = new MotionData(local.Move, (bone.BoneFlags & BoneType.Rotate) != 0
									? offset.IsRotationLocal ? Quaternion.Multiply(local.Rotation, rotationOffset.Value) : Quaternion.Multiply(rotationOffset.Value, local.Rotation)
									: local.Rotation);

								if ((bone.BoneFlags & BoneType.XYZ) != 0)
									newLocal.Move += GetLocalPosition(offset.IsPositionLocal, bone, offset.Position, newLocal.Rotation);

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

									i.Quaternion = offset.IsRotationLocal ? Quaternion.Multiply(i.Quaternion, rotationOffset.Value) : Quaternion.Multiply(rotationOffset.Value, i.Quaternion);
									i.Position += GetLocalPosition(offset.IsPositionLocal, bone, offset.Position, i.Quaternion);
								}
						}

						foreach (var morph in model.Morphs)
						{
							if (morph.Selected)
							{
								var local = morph.CurrentWeight;

								reset += () => morph.CurrentWeight = local;
								morph.CurrentWeight += offset.Weight;
							}

							if (!previewOnly)
								foreach (var i in morph.SelectedFrames)
								{
									var resetWeight = i.Weight;

									reset += () => i.Weight = resetWeight;
									i.Weight += offset.Weight;
								}
						}
					}

					break;
			}
		}

		public override string Description
		{
			get
			{
				return "選択したキーフレームの移動および回転値に指定したオフセットを与えます。";
			}
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Apply\r\nOffset");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("オフセット\r\n付加");
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("9d7c25d5-87e7-4855-9f75-6ca58c317c0f");
			}
		}

		public override Image Image
		{
			get
			{
				return Resources.ApplyOffset32;
			}
		}

		public override Image SmallImage
		{
			get
			{
				return Resources.ApplyOffset20;
			}
		}
	}
}
