using System;
using DxMath;

namespace Linearstar.MikuMikuMoving.Framework
{
	static class MathHelper
	{
		public static int Lerp(int value1, int value2, float amount)
		{
			return (int)(value1 + (value2 - value1) * amount);
		}

		public static float Lerp(float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}

		public static float ToDegrees(float radians)
		{
			return (float)(radians * 180 / Math.PI);
		}

		public static Vector3 ToDegrees(Vector3 radians)
		{
			return new Vector3(ToDegrees(radians.X), ToDegrees(radians.Y), ToDegrees(radians.Z));
		}

		public static float ToRadians(float degrees)
		{
			return (float)(degrees / 180 * Math.PI);
		}

		public static Vector3 ToRadians(Vector3 degrees)
		{
			return new Vector3(ToRadians(degrees.X), ToRadians(degrees.Y), ToRadians(degrees.Z));
		}

		public static Vector3 ToEulerAngle(Quaternion quaternion)
		{
			return ToEulerAngle(Matrix.RotationQuaternion(quaternion));
		}

		public static Vector3 ToEulerAngle(Matrix rotationMatrix)
		{
			const float halfPi = (float)(Math.PI / 2);

			if (rotationMatrix.M21 == 1)
				return new Vector3(-halfPi, 0, (float)Math.Atan2(rotationMatrix.M21, rotationMatrix.M11));
			else if (rotationMatrix.M21 == -1)
				return new Vector3(halfPi, 0, (float)Math.Atan2(rotationMatrix.M21, rotationMatrix.M11));
			else
			{
				var x = -Math.Asin(rotationMatrix.M32);
				var y = -Math.Atan2(-rotationMatrix.M31, rotationMatrix.M33);
				var z = -Math.Atan2(-rotationMatrix.M12, rotationMatrix.M22);

				return new Vector3(double.IsNaN(x) ? 0 : (float)x, double.IsNaN(y) ? 0 : (float)y, double.IsNaN(z) ? 0 : (float)z);
			}
		}
	}
}
