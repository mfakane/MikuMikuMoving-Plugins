using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance;

public class VmdSelfShadowFrame
{
	public uint FrameTime
	{
		get;
		set;
	}

	public VmdSelfShadowModel Model
	{
		get;
		set;
	}

	public float Distance
	{
		get;
		set;
	}

	public VmdSelfShadowFrame()
	{
		Model = VmdSelfShadowModel.Model1;
		Distance = (10000 - 8875) / 100000;
	}

	public static VmdSelfShadowFrame Parse(BinaryReader br)
	{
		return new()
		{
			FrameTime = br.ReadUInt32(),
			Model = (VmdSelfShadowModel)br.ReadByte(),
			Distance = br.ReadSingle(),
		};
	}

	public void Write(BinaryWriter bw)
	{
		bw.Write(FrameTime);
		bw.Write((byte)Model);
		bw.Write(Distance);
	}
}