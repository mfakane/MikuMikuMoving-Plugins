using System;
using System.Reflection;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin;

static class CaptionExtensions
{
    public static int GetIndex(this ICaption caption)
    {
        var cCaption = caption.GetType();

        var property = cCaption.GetProperty("Index");
        if (property != null)
            return ((Func<int>)Delegate.CreateDelegate(typeof(Func<int>), caption, property.GetMethod))();
        
        var field = cCaption.GetField("Index", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
            return (int)field.GetValue(caption);

        return -1;
    }

    public static FrameTime GetTimeFromFrame(this ICaption caption, float currentFrame) =>
        new(caption.StartFrame, caption.DurationFrame, currentFrame);
    
    public static FrameTime GetTimeFromProgress(this ICaption caption, float progress) =>
        new(caption.StartFrame, caption.DurationFrame, (float)(caption.StartFrame + caption.DurationFrame * progress));

    public static Caption Clone(this ICaption caption) =>
        new(caption.Text)
        {
            Alpha = caption.Alpha,
            DrawShadow = caption.DrawShadow,
            DrawTextBorder = caption.DrawTextBorder,
            DurationFrame = caption.DurationFrame,
            FadeInFrame = caption.FadeInFrame,
            FadeOutFrame = caption.FadeOutFrame,
            FontFamily = caption.FontFamily,
            FontSize = caption.FontSize,
            FontStyle = caption.FontStyle,
            HorizontalAlignment = caption.HorizontalAlignment,
            Index = caption.GetIndex(),
            LetterSpace = caption.LetterSpace,
            LineSpace = caption.LineSpace,
            Location = caption.Location,
            Rotate = caption.Rotate,
            ShadowDistance = caption.ShadowDistance,
            StartFrame = caption.StartFrame,
            Text = caption.Text,
            TextBorderColor = caption.TextBorderColor,
            TextBorderWeight = caption.TextBorderWeight,
            TextColor = caption.TextColor,
        };

    // public static object GetRealCaption(this ICaption caption)
    // {
    //     if (caption.GetType().FullName != "MikuMikuMoving.Plugin.CCaption")
    //         throw new ArgumentException();
    //
    //     return caption.Member("Controller").Member("CaptionManager").Member("CaptionList").Member("Item", caption.Member("Index"));
    // }
}