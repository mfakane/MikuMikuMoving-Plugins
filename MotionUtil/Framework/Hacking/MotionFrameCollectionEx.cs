using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	static class MotionFrameCollectionEx
	{
		public static object GetMotionSequence(this MotionFrameCollection collection)
		{
			return collection.Member("Controller").Member("ObjectList").Member("Item", collection.Member("ModelID")).Member("sequence");
		}

		public static MotionFrameData GetFrameEx(this MotionFrameCollection collection, long frameNumber)
		{
			return FromMotionFrameData(collection.GetMotionSequence().Member("GetMotion", (int)collection.Member("BoneID"), (int)collection.Member("LayerID"), frameNumber));
		}

		static MotionFrameData FromMotionFrameData(object obj)
		{
			return new MotionFrameData((long)obj.Member("frameNumber"), SlimDX.FromVector3(obj.Member("position")), SlimDX.FromQuaternion(obj.Member("quaternion")))
			{
				InterpolRA = FromPoint(obj.Member("interpolRA")),
				InterpolRB = FromPoint(obj.Member("interpolRB")),
				InterpolXA = FromPoint(obj.Member("interpolXA")),
				InterpolXB = FromPoint(obj.Member("interpolXB")),
				InterpolYA = FromPoint(obj.Member("interpolYA")),
				InterpolYB = FromPoint(obj.Member("interpolYB")),
				InterpolZA = FromPoint(obj.Member("interpolZA")),
				InterpolZB = FromPoint(obj.Member("interpolZB")),
				Selected = (bool)obj.Member("Selected"),
			};
		}

		static InterpolatePoint FromPoint(object obj)
		{
			return new InterpolatePoint((int)obj.Member("X"), (int)obj.Member("Y"));
		}
	}
}
