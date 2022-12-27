using System;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;

public class PropertyChangedEventArgs : EventArgs
{
    public CaptionPropertyKind Kind { get; }
    public AnimationMode Mode { get; }
    public bool EaseIn { get; }
    public bool EaseOut { get; }
    public int IterationDurationFrames { get; }
    public float FromValue { get; }
    public float ToValue { get; }

    public PropertyChangedEventArgs(CaptionPropertyKind kind, AnimationMode mode, bool easeIn, bool easeOut, int iterationDurationFrames, float fromValue, float toValue)
    {
        Kind = kind;
        Mode = mode;
        EaseIn = easeIn;
        EaseOut = easeOut;
        IterationDurationFrames = iterationDurationFrames;
        FromValue = fromValue;
        ToValue = toValue;
    }
}

public enum CaptionPropertyKind
{
    X,
    Y,
    Alpha,
    Rotation,
    FontSize,
    LineSpacing,
    LetterSpacing,
    ShadowDistance,
}