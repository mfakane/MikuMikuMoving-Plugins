using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
	public class VmdLightFrame
	{
		public uint FrameTime
		{
			get;
			set;
		}

		public float[] Color
		{
			get;
			set;
		}

		public float[] Position
		{
			get;
			set;
		}

		public VmdLightFrame()
		{
			this.Color = new[] { 0.6f, 0.6f, 0.6f };
			this.Position = new[] { -0.5f, -1, 0.5f };
		}

		public static VmdLightFrame Parse(BinaryReader br)
		{
			return new VmdLightFrame
			{
				FrameTime = br.ReadUInt32(),
				Color = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			this.Color.ForEach(bw.Write);
			this.Position.ForEach(bw.Write);
		}
	}
}
