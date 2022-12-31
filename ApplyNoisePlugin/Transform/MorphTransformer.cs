using System.Collections.Generic;
using System.Linq;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public class MorphTransformer : KeyFrameTransformer<MorphFrameData>
{
    readonly Morph morph;

    public override bool CanApplyTranslation => false;

    public override bool CanTranslateByLocal => false;

    public override bool CanApplyRotation => false;

    public override bool CanRotateByLocal => false;

    public override bool CanApplyWeight => true;
    
    MorphTransformer(Morph morph)
        : base(morph.Frames.GetKeyFrames()) =>
        this.morph = morph;

    public static IEnumerable<MorphTransformer> FromModel(Model model) =>
        model.Morphs
            .Where(morph => morph.SelectedFrames.Any())
            .Select(morph => new MorphTransformer(morph));
    
    protected override void ReplaceAllKeyFrames(IEnumerable<MorphFrameData> keyFrames) =>
        morph.Frames.ReplaceAllKeyFrames(keyFrames.ToList());

    protected override MorphFrameData GetFrame(long frameNumber) =>
        morph.Frames.GetFrame(frameNumber);

    protected override void ApplyNoiseToKeyFrame(MorphFrameData keyFrame, NoiseValue value, bool translateByLocal, bool rotateByLocal, bool normalizeWeight)
    {
        var newWeight = keyFrame.Weight + value.Weight;
        
        keyFrame.Weight = normalizeWeight ? MathHelper.Clamp(newWeight, 0, 1) : newWeight;
    }
}