using DxMath;

namespace Linearstar.MikuMikuMoving.Framework
{
	class IKLink
	{
		object link;

		public int LinkBoneIndex
		{
			get
			{
				return (int)link.Member("LinkBoneIndex");
			}
		}

		public bool Restrict
		{
			get
			{
				return (bool)link.Member("Restrict");
			}
		}

		public Vector3 Upper
		{
			get
			{
				return SlimDX.FromVector3(link.Member("Upper"));
			}
		}

		public Vector3 Lower
		{
			get
			{
				return SlimDX.FromVector3(link.Member("Lower"));
			}
		}

		public IKLink(object link)
		{
			this.link = link;
		}
	}
}
