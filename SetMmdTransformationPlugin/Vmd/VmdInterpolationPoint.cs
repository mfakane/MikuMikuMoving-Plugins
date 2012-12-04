using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public struct VmdInterpolationPoint
	{
		public static readonly VmdInterpolationPoint DefaultA = new VmdInterpolationPoint(20, 20);
		public static readonly VmdInterpolationPoint DefaultB = new VmdInterpolationPoint(107, 107);

		public byte X;
		public byte Y;

		public VmdInterpolationPoint(byte x, byte y)
		{
			if (x < 0 || x > 128)
				throw new ArgumentOutOfRangeException("x must be between 0 and 127.");

			if (y < 0 || y > 128)
				throw new ArgumentOutOfRangeException("y must be between 0 and 127.");

			this.X = x;
			this.Y = y;
		}

		public override string ToString()
		{
			return "{X:" + this.X + " Y:" + this.Y + "}";
		}
	}
}
