using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public class MorphTransformer : ITransformer
{
    readonly Morph morph;

    MorphTransformer(Morph morph)
    {
        this.morph = morph;
    }
    
    public static IEnumerable<MorphTransformer> FromModel(Model model) =>
        model.Morphs
            .Where(morph => morph.Selected || morph.SelectedFrames.Any())
            .Select(morph => new MorphTransformer(morph));

    public IValueTransformer GetWeightTransformer() =>
        new MorphValueTransformer(morph);

    IValueTransformer6? ITransformer.GetPositionRotationTransformer() => null;
    IValueTransformer3? ITransformer.GetPositionOnlyTransformer() => null;
    IValueTransformer3? ITransformer.GetRotationOnlyTransformer() => null;
    IValueTransformer? ITransformer.GetDistanceTransformer() => null;
    IValueTransformer3? ITransformer.GetColorTransformer() => null;
    
    class MorphValueTransformer : IValueTransformer
    {
        readonly Morph morph;
        readonly float initialWeight;

        public MorphValueTransformer(Morph morph)
        {
            this.morph = morph;
            initialWeight = morph.CurrentWeight;
        }

        public void PreviewTransform(float amount) =>
            morph.CurrentWeight = initialWeight + amount;

        public void SaveTransform(float amount)
        {
            var keyFrames = morph.Frames.GetKeyFrames();
            
            foreach (var frame in keyFrames.Where(frame => frame.Selected))
            {
                frame.Weight += amount;
                frame.Selected = true;
            }
            
            morph.Frames.ReplaceAllKeyFrames(keyFrames);
        }

        public void ResetPreview() =>
            morph.CurrentWeight = initialWeight;
    }
}