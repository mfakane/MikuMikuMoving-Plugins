using System.Collections.Generic;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

public abstract class MotionTransformer : KeyFrameTransformer<MotionFrameData, MotionFrameInterpolation>
{
    protected MotionTransformer(IReadOnlyCollection<MotionFrameData> keyFrames)
        : base(keyFrames)
    {
    }

    protected override MotionFrameInterpolation GetInterpolation(MotionFrameData keyFrame) =>
        MotionFrameInterpolation.FromFrame(keyFrame);

    protected override MotionFrameInterpolation GetInvertedInterpolation(MotionFrameData keyFrame) =>
        MotionFrameInterpolation.FromFrame(keyFrame).Invert();

    protected override void SetInterpolation(MotionFrameData keyFrame, MotionFrameInterpolation interpolation)
    {
        keyFrame.InterpolRA = interpolation.Ra;
        keyFrame.InterpolRB = interpolation.Rb;
        keyFrame.InterpolXA = interpolation.Xa;
        keyFrame.InterpolXB = interpolation.Xb;
        keyFrame.InterpolYA = interpolation.Ya;
        keyFrame.InterpolYB = interpolation.Yb;
        keyFrame.InterpolZA = interpolation.Za;
        keyFrame.InterpolZB = interpolation.Zb;
    }
}

public record MotionFrameInterpolation(
    InterpolatePoint Ra,
    InterpolatePoint Rb,
    InterpolatePoint Xa,
    InterpolatePoint Xb,
    InterpolatePoint Ya,
    InterpolatePoint Yb,
    InterpolatePoint Za,
    InterpolatePoint Zb
)
{
    public static MotionFrameInterpolation FromFrame(MotionFrameData frame) =>
        new(
            frame.InterpolRA,
            frame.InterpolRB,
            frame.InterpolXA,
            frame.InterpolXB,
            frame.InterpolYA,
            frame.InterpolYB,
            frame.InterpolZA,
            frame.InterpolZB
        );
        
    public MotionFrameInterpolation Invert() =>
        new(
            this.Rb.Invert(),
            this.Ra.Invert(),
            this.Xb.Invert(),
            this.Xa.Invert(),
            this.Yb.Invert(),
            this.Ya.Invert(),
            this.Zb.Invert(),
            this.Za.Invert()
        );
}