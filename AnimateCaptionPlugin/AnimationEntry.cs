using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public class AnimationEntry
	{
		public AnimationMode Mode
		{
			get;
			set;
		}

		public bool EaseIn
		{
			get;
			set;
		}

		public bool EaseOut
		{
			get;
			set;
		}

		public int IterationDuration
		{
			get;
			set;
		}

		public LinkedList<AnimationFrame> Frames
		{
			get;
			private set;
		}

		public AnimationEntry()
		{
			this.Frames = new LinkedList<AnimationFrame>();
		}

		public static AnimationEntry Parse(byte version, BinaryReader br)
		{
			return new AnimationEntry
			{
				Mode = (AnimationMode)br.ReadByte(),
				EaseIn = br.ReadBoolean(),
				EaseOut = br.ReadBoolean(),
				IterationDuration = br.ReadInt32(),
				Frames = new LinkedList<AnimationFrame>(Enumerable.Range(0, br.ReadInt32()).Select(_ => AnimationFrame.Parse(version, br))),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write((byte)this.Mode);
			bw.Write(this.EaseIn);
			bw.Write(this.EaseOut);
			bw.Write(this.IterationDuration);
			bw.Write(this.Frames.Count);

			foreach (var i in this.Frames)
				i.Write(bw);
		}

		public void Add(float value)
		{
			switch (this.Mode)
			{
				case AnimationMode.None:
				case AnimationMode.LinearInterpolation:
				case AnimationMode.NoInterpolation:
				case AnimationMode.LinearInterpolationFirstAndLastOnly:
				case AnimationMode.RandomFirstAndLast:
				case AnimationMode.RepeatFirstAndLast:
					this.Frames = new LinkedList<AnimationFrame>(this.Frames.Select(_ => new AnimationFrame(_.FrameAmount, _.Value + value)));

					break;
				case AnimationMode.ByAcceleration:
					this.Frames.First.Value = new AnimationFrame(this.Frames.First.Value.FrameAmount, this.Frames.First.Value.Value + value);

					break;
			}
		}

		public AnimationFrame[] GetBeginEndFramePair(double startFrame, double duration, double currentFrame)
		{
			return GetBeginEndFramePairNodes(startFrame, duration, currentFrame).Select(_ => _.Value).ToArray();
		}

		public AnimationFrame[] SetBeginEndFramePair(double startFrame, double duration, double currentFrame, float[] values)
		{
			var rt = GetBeginEndFramePairNodes(startFrame, duration, currentFrame);

			for (int i = 0; i < rt.Length; i++)
			{
				var f = rt[i].Value;

				rt[i].Value = new AnimationFrame(f.FrameAmount, values[i]);
			}

			return rt.Select(_ => _.Value).ToArray();
		}

		LinkedListNode<AnimationFrame>[] GetBeginEndFramePairNodes(double startFrame, double duration, double currentFrame)
		{
			currentFrame = Math.Max(startFrame, Math.Min(currentFrame, startFrame + duration));

			switch (this.Mode)
			{
				case AnimationMode.None:
					return new[] { this.Frames.First, this.Frames.First };
				case AnimationMode.LinearInterpolation:
				case AnimationMode.NoInterpolation:
					{
						var b = this.Frames.Nodes().FirstOrDefault(_ => startFrame + duration * _.Value.FrameAmount > currentFrame) ?? this.Frames.Last;
						var a = b.Previous ?? this.Frames.First;

						return new[] { a, b };
					}
				case AnimationMode.LinearInterpolationFirstAndLastOnly:
				case AnimationMode.ByAcceleration:
				case AnimationMode.RandomFirstAndLast:
				case AnimationMode.RepeatFirstAndLast:
					return new[] { this.Frames.First, this.Frames.Last };
				default:
					throw new InvalidOperationException();
			}
		}

		float Ease(float amount)
		{
			Func<float, float> easeCore = _ => _ * _;

			return this.EaseIn
				? this.EaseOut
					? amount < 0.5f
						? easeCore(amount * 2) * 0.5f
						: (1 - easeCore((1 - amount) * 2)) * 0.5f + 0.5f
					: easeCore(amount)
				: this.EaseOut
					? 1 - easeCore(1 - amount)
					: amount;
		}

		public AnimationFrame Lerp(double startFrame, double duration, double currentFrame)
		{
			var amount = (float)((currentFrame - startFrame) / duration);

			switch (this.Mode)
			{
				case AnimationMode.None:
					return new AnimationFrame(amount, this.Frames.First.Value.Value);
				case AnimationMode.LinearInterpolation:
					{
						var b = this.Frames.Nodes().FirstOrDefault(_ => startFrame + duration * _.Value.FrameAmount > currentFrame) ?? this.Frames.Last;
						var a = b.Previous.Value;

						return AnimationFrame.Lerp(a, b.Value, Ease((amount - a.FrameAmount) / (b.Value.FrameAmount - a.FrameAmount)));
					}
				case AnimationMode.NoInterpolation:
					return (this.Frames.Nodes().LastOrDefault(_ => startFrame + duration * _.Value.FrameAmount <= currentFrame) ?? this.Frames.First).Value;
				case AnimationMode.LinearInterpolationFirstAndLastOnly:
					return AnimationFrame.Lerp(this.Frames.First.Value, this.Frames.Last.Value, Ease(amount));
				case AnimationMode.ByAcceleration:
					return new AnimationFrame(amount, (float)(this.Frames.First.Value.Value + this.Frames.Last.Value.Value * (currentFrame - startFrame)));
				case AnimationMode.RandomFirstAndLast:
					{
						var r = new Random((int)(startFrame + this.Frames.First.Value.Value));
						var interval = Math.Max(1, this.IterationDuration);
						var times = (int)((currentFrame - startFrame) / interval);
						var remain = (currentFrame - startFrame) % interval;

						for (int i = 0; i < times; i++)
							r.NextDouble();

						var min = Math.Min(this.Frames.First.Value.Value, this.Frames.Last.Value.Value);
						var width = Math.Max(this.Frames.First.Value.Value, this.Frames.Last.Value.Value) - min;
						var current = r.NextDouble() * width + min;
						var next = r.NextDouble() * width + min;

						return new AnimationFrame(amount, MathHelper.Lerp((float)current, (float)next, Ease((float)(remain / interval))));
					}
				case AnimationMode.RepeatFirstAndLast:
					{
						var interval = Math.Max(1, this.IterationDuration);
						var times = (int)((currentFrame - startFrame) / interval);
						var remain = (currentFrame - startFrame) % interval;
						var current = times % 2 == 0 ? this.Frames.Last.Value.Value : this.Frames.First.Value.Value;
						var next = times % 2 != 0 ? this.Frames.Last.Value.Value : this.Frames.First.Value.Value;

						return new AnimationFrame(amount, MathHelper.Lerp((float)current, (float)next, Ease((float)(remain / interval))));
					}
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
