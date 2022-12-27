using System;
using DxMath;

namespace Linearstar.MikuMikuMoving.Framework;

static class MathHelper
{
	public static int Clamp(int value, int minimum, int maximum) =>
		Math.Max(minimum, Math.Min(value, maximum));
	
	public static float Clamp(float value, float minimum, float maximum) =>
		Math.Max(minimum, Math.Min(value, maximum));
	
	public static int Lerp(int value1, int value2, float amount) => 
		(int)(value1 + (value2 - value1) * amount);

	public static float Lerp(float value1, float value2, float amount) => 
		value1 + (value2 - value1) * amount;

	public static float ToDegrees(float radians) => 
		(float)(radians * 180 / Math.PI);

	public static Vector3 ToDegrees(Vector3 radians) => 
		new(ToDegrees(radians.X), ToDegrees(radians.Y), ToDegrees(radians.Z));

	public static float ToRadians(float degrees) => 
		(float)(degrees / 180 * Math.PI);

	public static Vector3 ToRadians(Vector3 degrees) => 
		new(ToRadians(degrees.X), ToRadians(degrees.Y), ToRadians(degrees.Z));

	public static Vector3 ToEulerAngle(Quaternion quaternion) => 
		ToEulerAngle(Matrix.RotationQuaternion(quaternion));

	public static Vector3 ToEulerAngle(Matrix rotationMatrix)
	{
		const float halfPi = (float)(Math.PI / 2);
		const float tolerance = float.Epsilon * 10;

		if (Math.Abs(rotationMatrix.M21 - 1) < tolerance)
			return new(-halfPi, 0, (float)Math.Atan2(rotationMatrix.M21, rotationMatrix.M11));
		
		if (Math.Abs(rotationMatrix.M21 + 1) < tolerance)
			return new(halfPi, 0, (float)Math.Atan2(rotationMatrix.M21, rotationMatrix.M11));
		
		var x = -Math.Asin(rotationMatrix.M32);
		var y = -Math.Atan2(-rotationMatrix.M31, rotationMatrix.M33);
		var z = -Math.Atan2(-rotationMatrix.M12, rotationMatrix.M22);

		return new(double.IsNaN(x) ? 0 : (float)x, double.IsNaN(y) ? 0 : (float)y, double.IsNaN(z) ? 0 : (float)z);
	}
}