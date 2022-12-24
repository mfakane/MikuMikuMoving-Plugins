using DxMath;

namespace Linearstar.MikuMikuMoving.Framework;

public static class QuaternionExtensions
{
    public static void Deconstruct(this Quaternion quaternion, out float x, out float y, out float z, out float w)
    {
        x = quaternion.X;
        y = quaternion.Y;
        z = quaternion.Z;
        w = quaternion.W;
    }
    
    public static float[] ToArray(this Quaternion quaternion) => 
        new[] { quaternion.X, quaternion.Y, quaternion.Z, quaternion.W };
}