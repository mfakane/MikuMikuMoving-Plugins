using System;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;

public class ValueChangedEventArgs<T> : EventArgs
{
    public T Value { get; }

    public ValueChangedEventArgs(T value)
    {
        Value = value;
    }
}