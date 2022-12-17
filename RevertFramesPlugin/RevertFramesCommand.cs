using System;
using System.Collections.Generic;
using System.Linq;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace RevertFramesPlugin
{
	public class RevertFramesCommand : CommandBase
	{
		public override void Run(CommandArgs e)
		{
			e.Cancel = true;

			switch (this.Scene.Mode)
			{
				case EditMode.AccessoryMode:
					var acc = this.Scene.ActiveAccessory;

					if (acc != null)
						using (new UndoBlock(this.Scene))
						{
							var layers = acc.Layers.Where(_ => _.SelectedFrames.Any()).ToArray();

							if (!layers.Any())
								return;

							e.Cancel = false;

							var beginFrame = layers.SelectMany(_ => _.SelectedFrames).Min(_ => _.FrameNumber);
							var endFrame = layers.SelectMany(_ => _.SelectedFrames).Max(_ => _.FrameNumber);

							foreach (var i in layers)
								ProcessMotionFrames(beginFrame, endFrame, i.Frames, i.Frames.GetSelectedKeyFrames());
						}

					break;
				case EditMode.CameraMode:
					using (new UndoBlock(this.Scene))
					{
						var cameraLayers = this.Scene.Cameras.SelectMany(_ => _.Layers).Where(_ => _.Frames.Any(f => f.Selected)).ToArray();

						if (!cameraLayers.Any())
							return;

						var beginFrame = cameraLayers.SelectMany(_ => _.Frames).Where(_ => _.Selected).Min(_ => _.FrameNumber);
						var endFrame = cameraLayers.SelectMany(_ => _.Frames).Where(_ => _.Selected).Max(_ => _.FrameNumber);

						e.Cancel = false;

						foreach (var i in cameraLayers)
						{
							var selectedFrames = i.Frames.GetSelectedKeyFrames();

							if (!selectedFrames.Any())
								continue;

							foreach (var j in selectedFrames)
								i.Frames.RemoveKeyFrame(j.FrameNumber);

							foreach (var j in RevertFrames
							(
								selectedFrames,
								beginFrame,
								endFrame,
								_ => _.FrameNumber,
								(_, value) => new CameraFrameData(value, _.Position, _.Angle, _.Radius, _.Fov),
								_ => new[]
									{
										_.InterpolDistA,
										_.InterpolDistB,
										_.InterpolFovA,
										_.InterpolFovB,
										_.InterpolMoveA,
										_.InterpolMoveB,
										_.InterpolRoteA,
										_.InterpolRoteB,
									},
								(_, value) => new CameraFrameData(_.FrameNumber, _.Position, _.Angle, _.Radius, _.Fov)
								{
									InterpolDistA = value[0],
									InterpolDistB = value[1],
									InterpolFovA = value[2],
									InterpolFovB = value[3],
									InterpolMoveA = value[4],
									InterpolMoveB = value[5],
									InterpolRoteA = value[6],
									InterpolRoteB = value[7],
									Selected = true,
								}
							))
								i.Frames.AddKeyFrame((CameraFrameData)j);
						}
					}

					break;
				case EditMode.EffectMode:
					var effect = this.Scene.ActiveEffect;

					if (effect != null)
						using (new UndoBlock(this.Scene))
						{
							e.Cancel = false;

							var selectedFrames = effect.Frames.GetSelectedKeyFrames();

							ProcessMotionFrames(selectedFrames.Min(_ => _.FrameNumber), selectedFrames.Max(_ => _.FrameNumber), effect.Frames, selectedFrames);
						}

					break;
				case EditMode.ModelMode:
					var model = this.Scene.ActiveModel;

					if (model != null)
						using (new UndoBlock(this.Scene))
						{
							var layers = model.Bones.SelectMany(_ => _.Layers).Where(_ => _.Frames.Any(f => f.Selected)).ToArray();

							if (!layers.Any() && !model.Morphs.Any(_ => _.SelectedFrames.Any()))
								return;

							e.Cancel = false;

							var beginFrame = layers.SelectMany(_ => _.Frames).Where(_ => _.Selected).Select(_ => _.FrameNumber).Concat(model.Morphs.SelectMany(_ => _.Frames).Where(_ => _.Selected).Select(_ => _.FrameNumber)).Min();
							var endFrame = layers.SelectMany(_ => _.Frames).Where(_ => _.Selected).Select(_ => _.FrameNumber).Concat(model.Morphs.SelectMany(_ => _.Frames).Where(_ => _.Selected).Select(_ => _.FrameNumber)).Max();

							foreach (var i in model.Morphs)
							{
								var selectedFrames = i.Frames.GetSelectedKeyFrames();

								if (!selectedFrames.Any())
									continue;

								foreach (var j in selectedFrames)
									i.Frames.RemoveKeyFrame(j.FrameNumber);

								foreach (var j in RevertFrames
								(
									selectedFrames,
									beginFrame,
									endFrame,
									_ => _.FrameNumber,
									(_, value) => new MorphFrameData(value, _.Weight),
									_ => new InterpolatePoint[0],
									(_, value) => new MorphFrameData(_.FrameNumber, _.Weight)
									{
										Selected = true,
									}
								))
									i.Frames.AddKeyFrame((MorphFrameData)j);
							}

							foreach (var i in layers)
								ProcessMotionFrames(beginFrame, endFrame, i.Frames, i.Frames.GetSelectedKeyFrames());
						}

					break;
			}
		}

		static void ProcessMotionFrames(long beginFrame, long endFrame, MotionFrameCollection collection, List<MotionFrameData> selectedFrames)
		{
			if (!selectedFrames.Any())
				return;

			foreach (var j in selectedFrames)
				collection.RemoveKeyFrame(j.FrameNumber);

			foreach (var j in RevertFrames
			(
				selectedFrames,
				beginFrame,
				endFrame,
				_ => _.FrameNumber,
				(_, value) => new MotionFrameData(value, _.Position, _.Quaternion),
				_ => new[]
				{
					_.InterpolRA,
					_.InterpolRB,
					_.InterpolXA,
					_.InterpolXB,
					_.InterpolYA,
					_.InterpolYB,
					_.InterpolZA,
					_.InterpolZB
				},
				(_, value) => new MotionFrameData(_.FrameNumber, _.Position, _.Quaternion)
				{
					InterpolRA = value[0],
					InterpolRB = value[1],
					InterpolXA = value[2],
					InterpolXB = value[3],
					InterpolYA = value[4],
					InterpolYB = value[5],
					InterpolZA = value[6],
					InterpolZB = value[7],
					Selected = true,
				}
			))
				collection.AddKeyFrame((MotionFrameData)j);
		}

		static IEnumerable<T> RevertFrames<T>(IList<T> keyFrames, long beginFrame, long endFrame, Func<T, long> getFrameNumber, Func<T, long, T> setFrameNumber, Func<T, InterpolatePoint[]> getInterpolation, Func<T, InterpolatePoint[], T> setInterpolation)
		{
			var frameNumbers = keyFrames.Select(getFrameNumber).Select(_ => beginFrame + (endFrame - _)).ToArray();
			var interpol = new[] { getInterpolation(keyFrames.First()) }.Concat(keyFrames.Skip(1).Select(getInterpolation).Select(_ =>
			{
				for (int i = 0; i < _.Length; i += 2)
				{
					var a = _[i];
					var b = _[i + 1];

					_[i] = new InterpolatePoint(127 - b.X, 127 - b.Y);
					_[i + 1] = new InterpolatePoint(127 - a.X, 127 - a.Y);
				}

				return _;
			}).Reverse()).ToArray();

			return keyFrames.Zip(frameNumbers, setFrameNumber).Reverse().Zip(interpol, setInterpolation);
		}

		public override string EnglishText
		{
			get
			{
				return Util.EnvorinmentNewLine("Revert\r\nFrames");
			}
		}

		public override string Text
		{
			get
			{
				return Util.EnvorinmentNewLine("フレーム\r\n時間反転");
			}
		}

		public override string Description
		{
			get
			{
				return "選択されたキーフレームを時間軸方向に反転します。";
			}
		}

		public override Guid GUID
		{
			get
			{
				return new Guid("2abcad24-ce8a-476f-8fcd-4590ab9497e4");
			}
		}
	}
}
