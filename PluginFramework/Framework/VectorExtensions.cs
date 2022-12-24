using DxMath;

namespace Linearstar.MikuMikuMoving.Framework;

public static class VectorExtensions
{
    public static void Deconstruct(this Vector2 vector, out float x, out float y)
    {
        x = vector.X;
        y = vector.Y;
    }
    
    public static void Deconstruct(this Vector3 vector, out float x, out float y, out float z)
    {
        x = vector.X;
        y = vector.Y;
        z = vector.Z;
    }
    
    public static void Deconstruct(this Vector4 vector, out float x, out float y, out float z, out float w)
    {
        x = vector.X;
        y = vector.Y;
        z = vector.Z;
        w = vector.W;
    }
    
    public static float[] ToArray(this Vector2 vector) =>
        new[] { vector.X, vector.Y };
    
    public static float[] ToArray(this Vector3 vector) =>
        new[] { vector.X, vector.Y, vector.Z };
    
    public static float[] ToArray(this Vector4 vector) => 
        new[] { vector.X, vector.Y, vector.Z, vector.W };
}