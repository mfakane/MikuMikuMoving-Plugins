using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public readonly record struct FrameTime(
    double StartFrame,
    double DurationFrames,
    float CurrentFrame
)
{
    public float Progress => DurationFrames == 0 ? 0 : (float)(ElapsedFrames / DurationFrames);
    
    public float ElapsedFrames => (float)(CurrentFrame - StartFrame);

    public FrameTime AtProgress(float progress) =>
        this with
        {
            CurrentFrame = (float)(StartFrame + DurationFrames * progress),
        };
    
    public FrameTime Normalize() =>
        this with
        {
            CurrentFrame = MathHelper.Clamp(CurrentFrame, (float)StartFrame, (float)(StartFrame + DurationFrames)),
        };
}