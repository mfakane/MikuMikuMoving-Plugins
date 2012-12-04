using System;
using System.IO;
using System.Linq;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public class AnimationData
	{
		public int Id
		{
			get;
			set;
		}

		public AnimationEntry X
		{
			get;
			private set;
		}

		public AnimationEntry Y
		{
			get;
			private set;
		}

		public AnimationEntry Alpha
		{
			get;
			private set;
		}

		public AnimationEntry Rotation
		{
			get;
			private set;
		}

		public AnimationEntry FontSize
		{
			get;
			private set;
		}

		public AnimationEntry LineSpacing
		{
			get;
			private set;
		}

		public AnimationEntry LetterSpacing
		{
			get;
			private set;
		}

		public AnimationEntry ShadowDistance
		{
			get;
			private set;
		}

		public AnimationData()
		{
			this.X = new AnimationEntry();
			this.Y = new AnimationEntry();
			this.Alpha = new AnimationEntry();
			this.Rotation = new AnimationEntry();
			this.FontSize = new AnimationEntry();
			this.LineSpacing = new AnimationEntry();
			this.LetterSpacing = new AnimationEntry();
			this.ShadowDistance = new AnimationEntry();
		}

		public AnimationData(ICaption caption)
			: this()
		{
			Add(0, Values.FromCaption(caption));
			Add(1, Values.FromCaption(caption));
		}

		public static AnimationData Parse(byte version, BinaryReader br)
		{
			return new AnimationData
			{
				Id = br.ReadInt32(),
				X = AnimationEntry.Parse(version, br),
				Y = AnimationEntry.Parse(version, br),
				Alpha = AnimationEntry.Parse(version, br),
				Rotation = AnimationEntry.Parse(version, br),
				FontSize = AnimationEntry.Parse(version, br),
				LineSpacing = AnimationEntry.Parse(version, br),
				LetterSpacing = AnimationEntry.Parse(version, br),
				ShadowDistance = AnimationEntry.Parse(version, br),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.Id);

			foreach (var i in new[]
			{
				this.X,
				this.Y,
				this.Alpha,
				this.Rotation,
				this.FontSize,
				this.LineSpacing,
				this.LetterSpacing,
				this.ShadowDistance,
			})
				i.Write(bw);
		}

		public void AddFrame(ICaption caption, double frame)
		{
			var startFrame = caption.StartFrame;
			var duration = caption.DurationFrame;

			AddSorted(this.X, startFrame, duration, frame);
			AddSorted(this.Y, startFrame, duration, frame);
			AddSorted(this.Alpha, startFrame, duration, frame);
			AddSorted(this.Rotation, startFrame, duration, frame);
			AddSorted(this.FontSize, startFrame, duration, frame);
			AddSorted(this.LineSpacing, startFrame, duration, frame);
			AddSorted(this.LetterSpacing, startFrame, duration, frame);
			AddSorted(this.ShadowDistance, startFrame, duration, frame);
		}

		public void RemoveFrame(int index)
		{
			foreach (var i in new[]
			{
				this.X,
				this.Y,
				this.Alpha,
				this.Rotation,
				this.FontSize,
				this.LineSpacing,
				this.LetterSpacing,
				this.ShadowDistance,
			})
			{
				i.Frames.Remove(i.Frames.Nodes().Skip(index).First());
				i.Frames.First.Value = new AnimationFrame(0, i.Frames.First.Value.Value);
				i.Frames.Last.Value = new AnimationFrame(1, i.Frames.Last.Value.Value);
			}
		}

		public void MoveFrame(int index, float frameAmount)
		{
			foreach (var i in new[]
			{
				this.X,
				this.Y,
				this.Alpha,
				this.Rotation,
				this.FontSize,
				this.LineSpacing,
				this.LetterSpacing,
				this.ShadowDistance,
			})
			{
				var n = i.Frames.Nodes().Skip(index).First();

				n.Value = new AnimationFrame(frameAmount, n.Value.Value);

				var prevHigher = i.Frames.Nodes().Take(index).FirstOrDefault(_ => _.Value.FrameAmount > frameAmount);

				if (prevHigher != null)
				{
					i.Frames.Remove(n);
					i.Frames.AddBefore(prevHigher, n);
				}

				var nextLower = i.Frames.Nodes().Skip(index).LastOrDefault(_ => _.Value.FrameAmount < frameAmount);

				if (nextLower != null)
				{
					i.Frames.Remove(n);
					i.Frames.AddAfter(nextLower, n);
				}
			}
		}

		void AddSorted(AnimationEntry entry, double startFrame, double duration, double frame)
		{
			var frameAmount = (float)((frame - startFrame) / duration);
			var node = entry.Frames.Nodes().LastOrDefault(_ => _.Value.FrameAmount > frameAmount) ?? entry.Frames.Last;

			entry.Frames.AddBefore(node, new AnimationFrame(frameAmount, entry.Lerp(startFrame, duration, frame).Value));
		}

		void Add(float frameAmount, Values values)
		{
			this.X.Frames.AddLast(new AnimationFrame(frameAmount, values.X));
			this.Y.Frames.AddLast(new AnimationFrame(frameAmount, values.Y));
			this.Alpha.Frames.AddLast(new AnimationFrame(frameAmount, values.Alpha));
			this.Rotation.Frames.AddLast(new AnimationFrame(frameAmount, values.Rotation));
			this.FontSize.Frames.AddLast(new AnimationFrame(frameAmount, values.FontSize));
			this.LineSpacing.Frames.AddLast(new AnimationFrame(frameAmount, values.LineSpacing));
			this.LetterSpacing.Frames.AddLast(new AnimationFrame(frameAmount, values.LetterSpacing));
			this.ShadowDistance.Frames.AddLast(new AnimationFrame(frameAmount, values.ShadowDistance));
		}

		Values Lerp(double startFrame, double duration, double frame)
		{
			return new Values
			{
				X = this.X.Lerp(startFrame, duration, frame).Value,
				Y = this.Y.Lerp(startFrame, duration, frame).Value,
				Alpha = this.Alpha.Lerp(startFrame, duration, frame).Value,
				Rotation = this.Rotation.Lerp(startFrame, duration, frame).Value,
				FontSize = this.FontSize.Lerp(startFrame, duration, frame).Value,
				LineSpacing = this.LineSpacing.Lerp(startFrame, duration, frame).Value,
				LetterSpacing = this.LetterSpacing.Lerp(startFrame, duration, frame).Value,
				ShadowDistance = this.ShadowDistance.Lerp(startFrame, duration, frame).Value,
			};
		}

		public Action<ICaption> ApplyAnimation(ICaption caption, float frame)
		{
			var value = Lerp(caption.StartFrame, caption.DurationFrame, frame);

			value.ToCaption(caption);

			return _ =>
			{
				var diff = Values.FromCaption(caption) - value;

				this.X.Add(diff.X);
				this.Y.Add(diff.Y);
				this.Alpha.Add(diff.Alpha);
				this.Rotation.Add(diff.Rotation);
				this.LineSpacing.Add(diff.LineSpacing);
				this.LetterSpacing.Add(diff.LetterSpacing);
				this.ShadowDistance.Add(diff.ShadowDistance);
			};
		}

		struct Values
		{
			public float X
			{
				get;
				set;
			}

			public float Y
			{
				get;
				set;
			}

			public float Alpha
			{
				get;
				set;
			}

			public float Rotation
			{
				get;
				set;
			}

			public float FontSize
			{
				get;
				set;
			}

			public float LineSpacing
			{
				get;
				set;
			}

			public float LetterSpacing
			{
				get;
				set;
			}

			public float ShadowDistance
			{
				get;
				set;
			}

			public static Values FromCaption(ICaption caption)
			{
				return new Values
				{
					X = caption.Location.X,
					Y = caption.Location.Y,
					Alpha = caption.Alpha,
					Rotation = caption.Rotate,
					FontSize = caption.FontSize,
					LineSpacing = caption.LineSpace,
					LetterSpacing = caption.LetterSpace,
					ShadowDistance = caption.ShadowDistance,
				};
			}

			public void ToCaption(ICaption caption)
			{
				caption.Location = new Vector3(this.X, this.Y, 0);
				caption.Alpha = this.Alpha;
				caption.Rotate = this.Rotation;
				caption.FontSize = Math.Max(this.FontSize, 0.1f);
				caption.LineSpace = this.LineSpacing;
				caption.LetterSpace = this.LetterSpacing;
				caption.ShadowDistance = this.ShadowDistance;
			}

			public static Values operator +(Values a, Values b)
			{
				return new Values
				{
					X = a.X + b.X,
					Y = a.Y + b.Y,
					Alpha = a.Alpha + b.Alpha,
					Rotation = a.Rotation + b.Rotation,
					FontSize = a.FontSize + b.FontSize,
					LineSpacing = a.LineSpacing + b.LineSpacing,
					LetterSpacing = a.LetterSpacing + b.LetterSpacing,
					ShadowDistance = a.ShadowDistance + b.ShadowDistance,
				};
			}

			public static Values operator -(Values a, Values b)
			{
				return new Values
				{
					X = a.X - b.X,
					Y = a.Y - b.Y,
					Alpha = a.Alpha - b.Alpha,
					Rotation = a.Rotation - b.Rotation,
					FontSize = a.FontSize - b.FontSize,
					LineSpacing = a.LineSpacing - b.LineSpacing,
					LetterSpacing = a.LetterSpacing - b.LetterSpacing,
					ShadowDistance = a.ShadowDistance - b.ShadowDistance,
				};
			}
		}
	}
}
