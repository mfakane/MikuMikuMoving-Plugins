using System;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public readonly record struct AnimationPair(
    IAnimationKeyFrame FromKeyFrame,
    IAnimationKeyFrame ToKeyFrame
)
{
    public AnimationFrame FromFrame => FromKeyFrame.Frame;
    public AnimationFrame ToFrame => ToKeyFrame.Frame;

    public float GetAmountFromTime(FrameTime elapsedTime) =>
        (elapsedTime.Progress - FromFrame.Progress) / (ToFrame.Progress - FromFrame.Progress);

    public AnimationFrame Lerp(float amount) =>
        AnimationFrame.Lerp(FromFrame, ToFrame, amount);

    public AnimationFrame GetAcceleratedFrame(FrameTime elapsedTime) =>
        new(elapsedTime.Progress, FromFrame.Value + ToFrame.Value * elapsedTime.ElapsedFrames);

    public float MinValue() => Math.Min(FromFrame.Value, ToFrame.Value);

    public float MaxValue() => Math.Max(FromFrame.Value, ToFrame.Value);
}

public interface IAnimationKeyFrame
{
    AnimationFrame Frame { get; }
    void SetValue(float value);
}