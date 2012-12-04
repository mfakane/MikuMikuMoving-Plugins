using System.IO;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public struct AnimationFrame
	{
		public float FrameAmount
		{
			get;
			set;
		}

		public float Value
		{
			get;
			set;
		}

		public AnimationFrame(float frameAmount, float value)
			: this()
		{
			this.FrameAmount = frameAmount;
			this.Value = value;
		}

		public static AnimationFrame Lerp(AnimationFrame a, AnimationFrame b, float amount)
		{
			return new AnimationFrame
			(
				MathHelper.Lerp(a.FrameAmount, b.FrameAmount, amount),
				MathHelper.Lerp(a.Value, b.Value, amount)
			);
		}

		public static AnimationFrame Parse(byte version, BinaryReader br)
		{
			return new AnimationFrame(br.ReadSingle(), br.ReadSingle());
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameAmount);
			bw.Write(this.Value);
		}
	}
}
