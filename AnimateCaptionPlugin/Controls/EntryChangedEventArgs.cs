using System;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;

public class EntryChangedEventArgs : EventArgs
{
    public float BeginValue { get; }
    public float EndValue { get; }
    public AnimationMode Mode { get; }
    public int IterationDuration { get; }
    public bool EaseIn { get; }
    public bool EaseOut { get; }
    
    public EntryChangedEventArgs(float beginValue, float endValue, AnimationMode mode, int iterationDuration, bool easeIn, bool easeOut)
    {
        BeginValue = beginValue;
        EndValue = endValue;
        Mode = mode;
        IterationDuration = iterationDuration;
        EaseIn = easeIn;
        EaseOut = easeOut;
    }
}