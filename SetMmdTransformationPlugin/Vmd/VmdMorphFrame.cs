using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance;

public class VmdMorphFrame
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

	public float Weight
	{
		get;
		set;
	}

	public static VmdMorphFrame Parse(BinaryReader br)
	{
		return new()
		{
			Name = VmdDocument.ReadVmdString(br, 15),
			FrameTime = br.ReadUInt32(),
			Weight = br.ReadSingle(),
		};
	}

	public void Write(BinaryWriter bw, VmdVersion version)
	{
		VmdDocument.WriteVmdString(bw, Name, 15, version);
		bw.Write(FrameTime);
		bw.Write(Weight);
	}
}