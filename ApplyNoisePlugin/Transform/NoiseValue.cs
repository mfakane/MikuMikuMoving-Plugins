using System;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public record NoiseValue(
    Vector3 Position,
    Vector3 Rotation,
    float Gravity,
    Vector3 GravityDirection
)
{
    public static NoiseValue Default =>
        new(
            new(5),
            MathHelper.ToRadians(new Vector3(5)),
            10,
            new(1)
        );
    
    static Vector3 Random3(Random random, Vector3 width) =>
        new(
            ((float)random.NextDouble() * 2 - 1) * width.X,
            ((float)random.NextDouble() * 2 - 1) * width.Y,
            ((float)random.NextDouble() * 2 - 1) * width.Z
        );

    static float Random(Random random, float width) =>
        ((float)random.NextDouble() * 2 - 1) * width;

    public static NoiseValue Create(Random random, NoiseValue width) =>
        new(
            Random3(random, width.Position),
            Random3(random, width.Rotation),
            Random(random, width.Gravity),
            Random3(random, width.GravityDirection)
        );

    public static NoiseValue Interpolate(NoiseValue a, NoiseValue b, float amount) =>
        new(
            Vector3.Lerp(a.Position, b.Position, amount),
            Vector3.Lerp(a.Rotation, b.Rotation, amount),
            MathHelper.Lerp(a.Gravity, b.Gravity, amount),
            Vector3.Lerp(a.GravityDirection, b.GravityDirection, amount)
        );
}

public record NoiseValueWithFrameNumber(
    long FrameNumber,
    long ShiftedFrameNumber,
    NoiseValue Value
);