using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	static class BoneEx
	{
		public static object GetRealBone(this Bone bone)
		{
			return bone.Member("Controller").Member("ObjectList").Member("Item", bone.Member("ModelID")).Member("Bones").Member("GetValue", new[] { new[] { bone.BoneID } });
		}

		public static IK GetIK(this Bone bone)
		{
			return new IK(GetRealBone(bone));
		}
	}
}
