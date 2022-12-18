using System;
using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin;

public class ValueChangedEventArgs : EventArgs
{
    public PositionAndRotation? PositionAndRotation { get; init; }
    public float? Weight { get; init; }
    public float? Distance { get; init; }
    public Vector3? Color { get; init; }
}

public record PositionAndRotation(
    Vector3 Position,
    bool IsPositionLocal,
    Vector3 Rotation,
    bool IsRotationLocal
);