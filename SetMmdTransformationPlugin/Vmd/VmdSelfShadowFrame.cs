using System.IO;

namespace Linearstar.Keystone.IO.MikuMikuDance
{
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
			this.Model = VmdSelfShadowModel.Model1;
			this.Distance = (10000 - 8875) / 100000;
		}

		public static VmdSelfShadowFrame Parse(BinaryReader br)
		{
			return new VmdSelfShadowFrame
			{
				FrameTime = br.ReadUInt32(),
				Model = (VmdSelfShadowModel)br.ReadByte(),
				Distance = br.ReadSingle(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			bw.Write((byte)this.Model);
			bw.Write(this.Distance);
		}
	}
}
