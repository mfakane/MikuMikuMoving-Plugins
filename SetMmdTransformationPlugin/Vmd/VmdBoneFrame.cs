using System.IO;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance;

public class VmdBoneFrame
{
	public string Name
	{
		get;
		set;
	}

	public uint FrameTime
	{
		get;
		set;
	}

	public float[] Position
	{
		get;
		set;
	}

	public float[] Quaternion
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

	public VmdInterpolationPoint[] RotationInterpolation
	{
		get;
		set;
	}

	public VmdBoneFrame()
	{
		Position = new[] { 0f, 0, 0 };
		Quaternion = new[] { 0f, 0, 0, 1 };
		XInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
		YInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
		ZInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
		RotationInterpolation = new[] { VmdInterpolationPoint.DefaultA, VmdInterpolationPoint.DefaultB };
	}

	public static VmdBoneFrame Parse(BinaryReader br)
	{
		var rt = new VmdBoneFrame
		{
			Name = VmdDocument.ReadVmdString(br, 15),
			FrameTime = br.ReadUInt32(),
			Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), },
			Quaternion = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), },
		};
		var ipBuffer = br.ReadBytes(64);
		var ipA = ipBuffer.Take(4)
			.Zip(ipBuffer.Skip(4).Take(4), (x, y) => new VmdInterpolationPoint(x, y));
		var ipB = ipBuffer.Skip(8)
			.Take(4)
			.Zip(ipBuffer.Skip(8 + 4).Take(4), (x, y) => new VmdInterpolationPoint(x, y));
		var ip = ipA.Zip(ipB, (a, b) => new[] { a, b }).ToArray();

		rt.XInterpolation = ip[0];
		rt.YInterpolation = ip[1];
		rt.ZInterpolation = ip[2];
		rt.RotationInterpolation = ip[3];

		return rt;
	}

	public void Write(BinaryWriter bw, VmdVersion version)
	{
		VmdDocument.WriteVmdString(bw, Name, 15, version);
		bw.Write(FrameTime);
		Position.ForEach(bw.Write);
		Quaternion.ForEach(bw.Write);

		var x = XInterpolation;
		var y = YInterpolation;
		var z = ZInterpolation;
		var r = RotationInterpolation;

		var X_x1 = x[0].X;
		var X_y1 = x[0].Y;
		var Y_x1 = y[0].X;
		var Y_y1 = y[0].Y;
		var Z_x1 = z[0].X;
		var Z_y1 = z[0].Y;
		var R_x1 = r[0].X;
		var R_y1 = r[0].Y;
		var X_x2 = x[1].X;
		var X_y2 = x[1].Y;
		var Y_x2 = y[1].X;
		var Y_y2 = y[1].Y;
		var Z_x2 = z[1].X;
		var Z_y2 = z[1].Y;
		var R_x2 = r[1].X;
		var R_y2 = r[1].Y;

		bw.Write(new byte[]
		{
			X_x1, Y_x1, Z_x1, R_x1, X_y1, Y_y1, Z_y1, R_y1, 
			X_x2, Y_x2, Z_x2, R_x2, X_y2, Y_y2, Z_y2, R_y2, 
			Y_x1, Z_x1, R_x1, X_y1, Y_y1, Z_y1, R_y1, X_x2, 
			Y_x2, Z_x2, R_x2, X_y2, Y_y2, Z_y2, R_y2,   01, 
			Z_x1, R_x1, X_y1, Y_y1, Z_y1, R_y1, X_x2, Y_x2, 
			Z_x2, R_x2, X_y2, Y_y2, Z_y2, R_y2,   01,   00, 
			R_x1, X_y1, Y_y1, Z_y1, R_y1, X_x2, Y_x2, Z_x2, 
			R_x2, X_y2, Y_y2, Z_y2, R_y2,   01,   00,   00
		});
	}
}