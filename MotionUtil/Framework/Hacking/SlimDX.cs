using System;
using DxMath;

namespace Linearstar.MikuMikuMoving.Framework
{
	static class SlimDX
	{
		static readonly Type vector3 = Type.GetType("SlimDX.Vector3, SlimDX");
		static readonly Type quaternion = Type.GetType("SlimDX.Quaternion, SlimDX");

		public static Vector3 FromVector3(object value)
		{
			return new Vector3
			(
				(float)vector3.GetField("X").GetValue(value),
				(float)vector3.GetField("Y").GetValue(value),
				(float)vector3.GetField("Z").GetValue(value)
			);
		}

		public static Quaternion FromQuaternion(object value)
		{
			return new Quaternion
			(
				(float)quaternion.GetField("X").GetValue(value),
				(float)quaternion.GetField("Y").GetValue(value),
				(float)quaternion.GetField("Z").GetValue(value),
				(float)quaternion.GetField("W").GetValue(value)
			);
		}
	}
}
