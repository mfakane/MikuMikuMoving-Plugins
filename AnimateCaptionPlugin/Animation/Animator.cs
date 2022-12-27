using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public class Animator
{
    readonly List<AnimationData?> animations;
    readonly Scene scene;

    const byte FormatVersion1 = 1;
    
    public AnimationData? this[ICaption? caption] =>
        caption != null ? animations.ElementAtOrDefault(caption.GetIndex()) : null;
    
    public AnimationData? this[Caption? caption] =>
        caption != null ? animations.ElementAtOrDefault(caption.Index) : null;

    public Animator(Scene scene)
        : this(scene, new List<AnimationData?>())
    {
    }
    
    public Animator(Scene scene, List<AnimationData?> animations)
    {
        this.scene = scene;
        this.animations = animations;
    }

    public static Animator? Parse(Scene scene, BinaryReader br)
    {
        var version = br.ReadByte();

        if (version != FormatVersion1) return null;

        var animations = Enumerable.Range(0, br.ReadInt32())
            .Select(_ =>
            {
                var animation = AnimationData.Parse(version, br, out var captionIndex);

                return new
                {
                    CaptionIndex = captionIndex,
                    Animation = animation,
                };
            })
            .ToDictionary(x => x.CaptionIndex, x => x.Animation);

        return new Animator(
            scene,
            scene.Captions
                .Select((_, i) => animations.TryGetValue(i, out var animation) ? animation : null)
                .ToList()
        );
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(FormatVersion1);

        foreach (var (caption, animation) in GetAnimatedCaptions())
            animation.Write(bw, caption.GetIndex());
    }
    
    public void OnEnabled()
    {
    }

    public void OnDisabled()
    {
        foreach (var (caption, animation) in GetAnimatedCaptions())
            animation.Reset(caption);
    }

    public void OnFrameUpdated(float currentFrame, ICaption? selectedCaption)
    {
        foreach (var (caption, animation) in GetAnimatedCaptions())
            animation.Apply(caption, currentFrame);
    }

    public AnimationData? OnCaptionChanged(ICaption caption, CaptionProperties fromValues, CaptionProperties toValues)
    {
        if (caption == null) throw new ArgumentNullException(nameof(caption));
        Debug.WriteLine($"{nameof(OnCaptionChanged)} {caption.Text}");
        
        var captionIndex = caption.GetIndex();

        if (animations.ElementAtOrDefault(captionIndex) is not { } animation)
            return null;
        
        animation.UpdateProperties(fromValues, toValues);
        
        return animation;
    }
    
    public void OnCaptionSelected(ICaption caption)
    {
        Debug.WriteLine($"{nameof(OnCaptionSelected)} {caption.Text}");
    }

    public void OnCaptionDeselected(Caption caption)
    {
        Debug.WriteLine($"{nameof(OnCaptionDeselected)} {caption.Text}");
    }

    public void OnCaptionDeleted(int captionIndex)
    {
        Debug.WriteLine($"{nameof(OnCaptionDeleted)} {captionIndex}");
        
        if (animations.Count > captionIndex)
            animations.RemoveAt(captionIndex);
    }

    public AnimationData EnableCaptionAnimation(ICaption caption)
    {
        var index = caption.GetIndex();
        
        if (animations.Count <= index) animations.Add(null);
        
        return animations[index] = new AnimationData(caption);
    }

    public void DisableCaptionAnimation(ICaption caption) =>
        animations[caption.GetIndex()] = null;
    
    IEnumerable<CaptionWithAnimationData> GetAnimatedCaptions() =>
        animations
            .Select((x, i) => new
            {
                Caption = scene.Captions.ElementAtOrDefault(i),
                Data = x,
            })
            .Where(x => x.Caption != null && x.Data != null)
            .Select(x => new CaptionWithAnimationData(x.Caption!, x.Data!));

    record CaptionWithAnimationData(ICaption Caption, AnimationData Animation);
}