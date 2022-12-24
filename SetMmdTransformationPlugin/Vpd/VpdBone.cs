using System.Collections.Generic;
using System.Linq;

namespace Linearstar.Keystone.IO.MikuMikuDance;

public class VpdBone
{
	public string BoneName
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

	public VpdBone()
	{
		Position = new[] { 0f, 0, 0 };
		Quaternion = new[] { 0f, 0, 0, 1 };
	}

	public static VpdBone Parse(IEnumerable<string> block)
	{
		var rt = new VpdBone();

		foreach (var i in block)
			if (i.StartsWith("Bone") && i.Contains("{"))
				rt.BoneName = i.Split(new[] { '{' }, 2).Last();
			else if (i == "}")
				continue;
			else
			{
				var fl = i.Split(new[] { ';' }, 2).First().Split(',').Select(float.Parse).ToArray();

				if (fl.Length == 4)
					rt.Quaternion = fl;
				else
					rt.Position = fl;
			}

		return rt;
	}

	public string GetFormattedText(int index)
	{
		return string.Format
		(
			"Bone{0}{{{1}\r\n  {2};\t\t\t\t// trans x,y,z\r\n  {3};\t\t// Quaternion x,y,z,w\r\n}}",
			index,
			BoneName,
			string.Join(",", Position.Select(x => x.ToString("0.000000")).ToArray()),
			string.Join(",", Quaternion.Select(x => x.ToString("0.000000")).ToArray())
		);
	}
}