using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin;

public static class InterpolatePointExtension
{
    public static InterpolatePoint Invert(this InterpolatePoint interpolatePoint) =>
        new (127 - interpolatePoint.X, 127 - interpolatePoint.Y);
}