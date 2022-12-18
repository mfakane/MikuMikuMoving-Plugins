using System;
using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin;

public class ValueChangedEventArgs : EventArgs
{
    public Vector3 Position { get; init; }
    public Vector3 Rotation { get; init; }
    public bool IsMoveOnly { get; init; }
}