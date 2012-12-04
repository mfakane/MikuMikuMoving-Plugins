using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	class IK
	{
		object ik;

		public int TargetBoneIndex
		{
			get
			{
				return (int)ik.Member("TargetBoneIndex");
			}
		}

		public int LoopCount
		{
			get
			{
				return (int)ik.Member("LoopCount");
			}
		}

		public float RestrictRadian
		{
			get
			{
				return (float)ik.Member("RestrictRadian");
			}
		}

		public IKLink[] Links
		{
			get;
			private set;
		}

		public IK(object realBone)
		{
			this.ik = realBone.Member("IK");
			this.Links = ((object[])this.ik.Member("Links")).Select(_ => new IKLink(_)).ToArray();
		}
	}
}
