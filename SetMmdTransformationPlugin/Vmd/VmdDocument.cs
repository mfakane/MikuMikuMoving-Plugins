using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Linearstar.Keystone.IO.MikuMikuDance;

/// <summary>
/// Vocaloid Motion Data file created by Higuchi_U
/// </summary>
public class VmdDocument
{
	public const string DisplayName = "Vocaloid Motion Data file";
	public const string Filter = "*.vmd";
	public static readonly Encoding Encoding = Encoding.GetEncoding(932);

	public VmdVersion Version
	{
		get;
		set;
	}

	public string ModelName
	{
		get;
		set;
	}

	public IList<VmdBoneFrame> BoneFrames
	{
		get;
		set;
	}

	public IList<VmdMorphFrame> MorphFrames
	{
		get;
		set;
	}

	public IList<VmdCameraFrame> CameraFrames
	{
		get;
		set;
	}

	public IList<VmdLightFrame> LightFrames
	{
		get;
		set;
	}

	public IList<VmdSelfShadowFrame> SelfShadowFrames
	{
		get;
		set;
	}

	public VmdDocument()
	{
		Version = VmdVersion.MMDVer3;
		BoneFrames = new List<VmdBoneFrame>();
		MorphFrames = new List<VmdMorphFrame>();
		CameraFrames = new List<VmdCameraFrame>();
		LightFrames = new List<VmdLightFrame>();
		SelfShadowFrames = new List<VmdSelfShadowFrame>();
	}

	public static VmdDocument Parse(Stream stream)
	{
		var rt = new VmdDocument();
		// leave open
		var br = new BinaryReader(stream);
		var header = ReadVmdString(br, 30);

		if (header == "Vocaloid Motion Data file")
			rt.Version = VmdVersion.MMDVer2;
		else if (header == "Vocaloid Motion Data 0002")
			rt.Version = VmdVersion.MMDVer3;
		else
			throw new InvalidOperationException("invalid format");

		rt.ModelName = ReadVmdString(br, rt.Version == VmdVersion.MMDVer2 ? 10 : 20);

		rt.BoneFrames = ReadBoneFrames(br).ToList();
		rt.MorphFrames = ReadMorphFrames(br).ToList();
		rt.CameraFrames = ReadCameraFrames(br, rt.Version).ToList();
		rt.LightFrames = ReadLightFrames(br).ToList();

		if (br.GetRemainingLength() > 4)
			rt.SelfShadowFrames = ReadSelfShadowFrames(br).ToList();

		return rt;
	}

	static IEnumerable<VmdBoneFrame> ReadBoneFrames(BinaryReader br)
	{
		var count = br.ReadUInt32();

		for (uint i = 0; i < count; i++)
			yield return VmdBoneFrame.Parse(br);
	}

	static IEnumerable<VmdMorphFrame> ReadMorphFrames(BinaryReader br)
	{
		var count = br.ReadUInt32();

		for (uint i = 0; i < count; i++)
			yield return VmdMorphFrame.Parse(br);
	}

	static IEnumerable<VmdCameraFrame> ReadCameraFrames(BinaryReader br, VmdVersion version)
	{
		var count = br.ReadUInt32();

		for (uint i = 0; i < count; i++)
			yield return VmdCameraFrame.Parse(br, version);
	}

	static IEnumerable<VmdLightFrame> ReadLightFrames(BinaryReader br)
	{
		var count = br.ReadUInt32();

		for (uint i = 0; i < count; i++)
			yield return VmdLightFrame.Parse(br);
	}

	static IEnumerable<VmdSelfShadowFrame> ReadSelfShadowFrames(BinaryReader br)
	{
		var count = br.ReadUInt32();

		for (uint i = 0; i < count; i++)
			yield return VmdSelfShadowFrame.Parse(br);
	}

	internal static string ReadVmdString(BinaryReader br, int count)
	{
		return Encoding.GetString(br.ReadBytes(count).TakeWhile(x => x != '\0').ToArray());
	}

	internal static void WriteVmdString(BinaryWriter bw, string s, int count, VmdVersion version)
	{
		var bytes = Encoding.GetBytes(s);

		bw.Write(bytes.Take(count - 1).ToArray());
		bw.Write(Enumerable.Repeat(version == VmdVersion.MMDVer2 ? (byte)0 : (byte)0xFD, Math.Max(count - bytes.Length, 1)).Select((_, idx) => idx == 0 ? (byte)0 : _).ToArray());
	}

	public void Write(Stream stream)
	{
		// leave open
		var bw = new BinaryWriter(stream);

		if (Version == VmdVersion.MMDVer2)
		{
			WriteVmdString(bw, "Vocaloid Motion Data file", 30, VmdVersion.MMDVer2);
			WriteVmdString(bw, ModelName, 10, Version);
		}
		else
		{
			WriteVmdString(bw, "Vocaloid Motion Data 0002", 30, VmdVersion.MMDVer2);
			WriteVmdString(bw, ModelName, 20, Version);
		}

		bw.Write(BoneFrames.Count);
		BoneFrames.ForEach(x => x.Write(bw, Version));
		bw.Write(MorphFrames.Count);
		MorphFrames.ForEach(x => x.Write(bw, Version));
		bw.Write(CameraFrames.Count);
		CameraFrames.ForEach(x => x.Write(bw, Version));
		bw.Write(LightFrames.Count);
		LightFrames.ForEach(x => x.Write(bw));

		if (Version == VmdVersion.MMDVer3)
		{
			bw.Write(SelfShadowFrames.Count);
			SelfShadowFrames.ForEach(x => x.Write(bw));
		}

		bw.Write(0);
	}
}