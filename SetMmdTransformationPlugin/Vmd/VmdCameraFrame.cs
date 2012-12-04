using System;
using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class VmdCameraFrame
	{
		public uint FrameTime
		{
			get;
			set;
		}

		public float Radius
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public float[] Angle
		{
			get;
			set;
		}

		public VmdInterpolationPoint[] XInterpolation
		{
			get;
			set;
		}

		public VmdInterpolationPoint[] YInterpolation
		{
			get;
			set;
		}

		public VmdInterpolationPoint[] ZInterpolation
		{
			get;
			set;
		}

		public VmdInterpolationPoint[] AngleInterpolation
		{
			get;
			set;
		}

		public VmdInterpolationPoint[] RadiusInterpolation
		{
			get;
			set;
		}

		public VmdInterpolationPoint[] FovInterpolation
		{
			get;
			set;
		}

		public int FovInDegree
		{
			get;
			set;
		}

		public bool Ortho
		{
			get;
			set;
		}

		public VmdCameraFrame()
		{
			this.Radius = 50;
			this.Position = new[] { 0f, 10, 0 };
			this.Angle = new[] { 0, (float)Math.PI, 0 };
			this.FovInDegree = 30;
			this.Ortho = false;
			this.XInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
			this.YInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
			this.ZInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
			this.AngleInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
			this.RadiusInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
			this.FovInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
		}

		public static VmdCameraFrame Parse(BinaryReader br, VmdVersion version)
		{
			var rt = new VmdCameraFrame
			{
				FrameTime = br.ReadUInt32(),
				Radius = br.ReadSingle(),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Angle = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
			};

			if (version == VmdVersion.MMDVer3)
			{
				rt.XInterpolation = ReadInterpolationPair(br);
				rt.YInterpolation = ReadInterpolationPair(br);
				rt.ZInterpolation = ReadInterpolationPair(br);
				rt.AngleInterpolation = ReadInterpolationPair(br);
				rt.RadiusInterpolation = ReadInterpolationPair(br);
				rt.FovInterpolation = ReadInterpolationPair(br);
				rt.FovInDegree = br.ReadInt32();
				rt.Ortho = br.ReadBoolean();
			}
			else
				rt.AngleInterpolation = ReadInterpolationPair(br);

			return rt;
		}

		public void Write(BinaryWriter bw, VmdVersion version)
		{
			bw.Write(this.FrameTime);
			bw.Write(this.Radius);
			this.Position.ForEach(bw.Write);
			this.Angle.ForEach(bw.Write);

			if (version == VmdVersion.MMDVer3)
			{
				WriteInterpolationPair(bw, this.XInterpolation);
				WriteInterpolationPair(bw, this.YInterpolation);
				WriteInterpolationPair(bw, this.ZInterpolation);
				WriteInterpolationPair(bw, this.AngleInterpolation);
				WriteInterpolationPair(bw, this.RadiusInterpolation);
				WriteInterpolationPair(bw, this.FovInterpolation);
				bw.Write(this.FovInDegree);
				bw.Write(this.Ortho);
			}
			else
				WriteInterpolationPair(bw, this.AngleInterpolation);
		}

		static VmdInterpolationPoint[] ReadInterpolationPair(BinaryReader br)
		{
			var xxyy = br.ReadBytes(4);

			return new[]
			{
				new VmdInterpolationPoint(xxyy[0], xxyy[2]),
				new VmdInterpolationPoint(xxyy[1], xxyy[3]),
			};
		}

		static void WriteInterpolationPair(BinaryWriter bw, VmdInterpolationPoint[] pair)
		{
			bw.Write(pair[0].X);
			bw.Write(pair[1].X);
			bw.Write(pair[0].Y);
			bw.Write(pair[1].Y);
		}
	}
}
