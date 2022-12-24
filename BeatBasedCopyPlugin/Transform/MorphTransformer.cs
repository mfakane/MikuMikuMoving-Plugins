using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public class MorphTransformer : KeyFrameTransformer<MorphFrameData>
{
    readonly Morph morph;

    MorphTransformer(Morph morph)
        : base(morph.Frames.GetKeyFrames()) =>
        this.morph = morph;
    
    public static IEnumerable<MorphTransformer> FromModel(Model model) =>
        model.Morphs
            .Where(morph => morph.SelectedFrames.Any())
            .Select(morph => new MorphTransformer(morph));

    protected override void ReplaceAllKeyFrames(IEnumerable<MorphFrameData> keyFrames) => 
        morph.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
}