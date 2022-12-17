using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin
{
	public class ApplyNoiseCommand : CommandBase
	{
		int keyFrameInterval = 5;
		int noiseValueInterval = 5;
		int keyShiftWidth = 0;
		bool isPositionLocal;
		bool isRotationLocal;
		NoiseValue noiseValue = new NoiseValue
		{
			PositionWidth = new Vector3(5, 5, 5),
			RotationWidth = new Vector3(5, 5, 5),
			GravityWidth = 10,
			GravityDirectionWidth = new Vector3(1, 1, 1),
		};

		public override void Run(CommandArgs e)
		{
			var r = new Random();
			var mode = this.Scene.Mode;
			var isModelOrAccessoryOrEffect = mode == EditMode.ModelMode || mode == EditMode.AccessoryMode || mode == EditMode.EffectMode;
			var isCameraSelected = this.Scene.Cameras.SelectMany(_ => _.Layers).SelectMany(_ => _.SelectedFrames).Any();
			var isEnvironmentSelected = this.Scene.SelectedPropertyFrames.Any();

			if (!isModelOrAccessoryOrEffect && !isCameraSelected && !isEnvironmentSelected)
			{
				MessageBox.Show(this.ApplicationForm, Util.Bilingual
				(
					this.Scene.Language,
					"対象のモデル、カメラ、アクセサリ、またはエフェクトがありません。\r\n対象のキーフレームを選択してから実行してください。",
					"No target selected.\r\nPlease select one or more keyframes to apply some noise."
				), Util.Bilingual(this.Scene.Language, this.Text, this.EnglishText), MessageBoxButtons.OK, MessageBoxIcon.Information);

				e.Cancel = true;

				return;
			}

			using (var f = new ApplyNoiseForm
			{
				KeyFrameInterval = keyFrameInterval,
				KeyShiftWidth = keyShiftWidth,
				NoiseValueInterval = noiseValueInterval,
				NoiseValue = noiseValue,

				IsPositionEnabled = isModelOrAccessoryOrEffect || isCameraSelected,
				IsPositionLocalVisible = isModelOrAccessoryOrEffect,
				IsRotationEnabled = isModelOrAccessoryOrEffect || isCameraSelected,
				IsRotationLocalVisible = isModelOrAccessoryOrEffect,
				IsEnvironmentEnabled = isEnvironmentSelected,
				IsPositionLocal = isPositionLocal,
				IsRotationLocal = isRotationLocal,
			})
			{
				if (f.ShowDialog() != DialogResult.OK)
				{
					e.Cancel = true;

					return;
				}

				keyFrameInterval = f.KeyFrameInterval;
				noiseValueInterval = f.NoiseValueInterval;
				noiseValue = f.NoiseValue;
				keyShiftWidth = f.KeyShiftWidth;
				isPositionLocal = f.IsPositionLocal;
				isRotationLocal = f.IsRotationLocal;

				using (new UndoBlock(this.Scene))
					switch (this.Scene.Mode)
					{
						case EditMode.AccessoryMode:
							var acc = this.Scene.ActiveAccessory;

							if (acc != null)
								foreach (var layer in acc.Layers.Where(_ => _.SelectedFrames.Any()))
									ProcessLayer(r, layer.SelectedFrames.First().FrameNumber, layer.SelectedFrames.Last().FrameNumber, layer.Frames, isPositionLocal, isRotationLocal, null);

							break;
						case EditMode.CameraMode:
							var camera = this.Scene.ActiveCamera;
							var env = this.Scene.SelectedPropertyFrames;

							if (camera != null)
								foreach (var layer in camera.Layers.Where(_ => _.SelectedFrames.Any()))
									ProcessCameraLayer(r, layer.SelectedFrames.First().FrameNumber, layer.SelectedFrames.Last().FrameNumber, layer.Frames);

							if (env.Any())
								ProcessEnvironmentLayer(r, env.First().FrameNumber, env.Last().FrameNumber, this.Scene.PropertyFrames);

							break;
						case EditMode.EffectMode:
							var eff = this.Scene.ActiveEffect;

							if (eff != null)
								ProcessLayer(r, eff.SelectedFrames.First().FrameNumber, eff.SelectedFrames.Last().FrameNumber, eff.Frames, isPositionLocal, isRotationLocal, null);

							break;
						case EditMode.ModelMode:
							var model = this.Scene.ActiveModel;

							if (model != null)
								foreach (var bone in model.Bones)
									foreach (var layer in bone.Layers.Where(_ => _.SelectedFrames.Any()))
										ProcessLayer(r, layer.SelectedFrames.First().FrameNumber, layer.SelectedFrames.Last().FrameNumber, layer.Frames, isPositionLocal, isRotationLocal, bone);

							break;
					}
			}
		}

		void ProcessLayer(Random r, long beginFrame, long endFrame, MotionFrameCollection frames, bool isPositionLocal, bool isRotationLocal, Bone localBone)
		{
			var newKeyFrames = ProcessFrames(r, beginFrame, endFrame, keyFrameInterval, noiseValueInterval, new HashSet<long>(frames.Where(_ => _.Selected).Select(_ => _.FrameNumber)))
				.Select(_ =>
				{
					var baseFrame = frames.GetFrame(_.Key);
					var localRotation = GetLocalRotation(isRotationLocal, localBone, _.Value.RotationWidth, _.Value.RotationQuaternion);

					baseFrame.FrameNumber = Math.Max(_.Key + GetNoise(r, keyShiftWidth), 0);
					baseFrame.Quaternion = isRotationLocal ? Quaternion.Multiply(baseFrame.Quaternion, _.Value.RotationQuaternion) : Quaternion.Multiply(_.Value.RotationQuaternion, baseFrame.Quaternion);
					baseFrame.Position += isPositionLocal ? GetLocalPosition(isPositionLocal, localBone, _.Value.PositionWidth, baseFrame.Quaternion) : _.Value.PositionWidth;

					baseFrame.Selected = true;

					return baseFrame;
				})
				.ToArray();

			foreach (var i in frames.Where(_ => _.Selected))
				frames.RemoveKeyFrame(i.FrameNumber);

			frames.AddKeyFrame(newKeyFrames.ToLookup(_ => _.FrameNumber)
										   .Select(_ => _.Last())
										   .ToList());
		}

		void ProcessEnvironmentLayer(Random r, long beginFrame, long endFrame, SceneFrameCollection frames)
		{
			var newKeyFrames = ProcessFrames(r, beginFrame, endFrame, keyFrameInterval, noiseValueInterval, new HashSet<long>(frames.Where(_ => _.Selected).Select(_ => _.FrameNumber)))
				.Select(_ =>
				{
					var baseFrame = frames.GetFrame(_.Key);

					baseFrame.FrameNumber = Math.Max(_.Key + GetNoise(r, keyShiftWidth), 0);
					baseFrame.Gravity += _.Value.GravityWidth;
					baseFrame.GravityDirection += _.Value.GravityDirectionWidth;
					baseFrame.Selected = true;

					return baseFrame;
				})
				.ToArray();

			foreach (var i in frames.Where(_ => _.Selected))
				frames.RemoveKeyFrame(i.FrameNumber);

			frames.AddKeyFrame(newKeyFrames.ToLookup(_ => _.FrameNumber)
										   .Select(_ => _.Last())
										   .ToList());
		}

		void ProcessCameraLayer(Random r, long beginFrame, long endFrame, CameraFrameCollection frames)
		{
			var existing = frames.ToList();
			var newKeyFrames = ProcessFrames(r, beginFrame, endFrame, keyFrameInterval, noiseValueInterval, new HashSet<long>(frames.Where(_ => _.Selected).Select(_ => _.FrameNumber)))
				.Select(_ =>
				{
					var baseFrame = frames.GetFrame(_.Key);

					baseFrame.FrameNumber = Math.Max(_.Key + GetNoise(r, keyShiftWidth), 0);
					baseFrame.Position += _.Value.PositionWidth;
					baseFrame.Angle += MathHelper.ToRadians(_.Value.RotationWidth);
					baseFrame.Selected = true;

					return baseFrame;
				})
				.ToArray();

			foreach (var i in frames.Where(_ => _.Selected))
				frames.RemoveKeyFrame(i.FrameNumber);

			frames.AddKeyFrame(newKeyFrames.ToLookup(_ => _.FrameNumber)
										   .Select(_ => _.Last())
										   .ToList());
		}

		IEnumerable<KeyValuePair<long, NoiseValue>> ProcessFrames(Random r, long beginFrame, long endFrame, int keyFrameInterval, int noiseValueInterval, HashSet<long> existingFrames)
		{
			Func<long, NoiseValue> getNoiseValue = currentFrame => new NoiseValue
			{
				PositionWidth = GetNoise(r, noiseValue.PositionWidth),
				RotationWidth = GetNoise(r, noiseValue.RotationWidth),
				GravityWidth = GetNoise(r, noiseValue.GravityWidth),
				GravityDirectionWidth = GetNoise(r, noiseValue.GravityDirectionWidth),
			};

			var nextKeyFrame = beginFrame + keyFrameInterval;
			var nextNoiseFrame = beginFrame + noiseValueInterval;

			var currentNoiseFrame = beginFrame;
			var currentNoiseValue = getNoiseValue(currentNoiseFrame);
			var nextNoiseValue = getNoiseValue(nextNoiseFrame);

			for (var i = beginFrame; i <= endFrame; i++)
			{
				if (i >= nextNoiseFrame)
				{
					// update noise value
					currentNoiseFrame = i;
					currentNoiseValue = nextNoiseValue;
					nextNoiseValue = getNoiseValue(nextNoiseFrame += noiseValueInterval);
				}

				if (keyFrameInterval == 0)
				{
					// overwrite exisitng key frame only
					if (existingFrames.Remove(i))
						yield return new KeyValuePair<long, NoiseValue>(i, currentNoiseValue.Interpolate(nextNoiseValue, (float)(i - currentNoiseFrame) / (nextNoiseFrame - currentNoiseFrame)));
				}
				else
					if (i >= nextKeyFrame ||
						i == beginFrame)
					{
						// create key frame
						yield return new KeyValuePair<long, NoiseValue>(i, currentNoiseValue.Interpolate(nextNoiseValue, (float)(i - currentNoiseFrame) / (nextNoiseFrame - currentNoiseFrame)));

						if (i > beginFrame)
							nextKeyFrame += keyFrameInterval;
					}
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

		static Vector3 GetNoise(Random r, Vector3 width)
		{
			return new Vector3(GetNoise(r, width.X), GetNoise(r, width.Y), GetNoise(r, width.Z));
		}

		static float GetNoise(Random r, float width)
		{
			return (float)(r.NextDouble() * width - width / 2);
		}

		static int GetNoise(Random r, int width)
		{
			return r.Next(-width / 2, (int)(width / 2.0 + 0.5));
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
				return Util.EnvorinmentNewLine("Apply\r\nNoise");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("ノイズ\r\n付加");
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("3c92a157-b847-4eb6-9e2d-a1df9786dfcc");
			}
		}
	}
}
