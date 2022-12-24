using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin;

public static class InterpolatePointExtension
{
    public static VmdInterpolationPoint ToVmd(this InterpolatePoint interpolatePoint) =>
        new ((byte)interpolatePoint.X, (byte)interpolatePoint.Y);
}