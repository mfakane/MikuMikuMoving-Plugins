using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public class MorphTransformer : KeyFrameTransformer<MorphFrameData, float>
{
    readonly Morph morph;
    
    MorphTransformer(Morph morph)
        : base(morph.Frames.GetSelectedKeyFrames(), morph.CurrentWeight) =>
        this.morph = morph;
    
    public static IEnumerable<MorphTransformer> FromModel(Model model) =>
        model.Morphs.Select(x => new MorphTransformer(x));

    protected override void WriteKeyFrames(VmdDocument vmdDocument, IReadOnlyCollection<MorphFrameData> keyFrames, long fromFrame, long toFrame)
    {
        foreach (var keyFrame in keyFrames)
            vmdDocument.MorphFrames.Add(new()
            {
                FrameTime = (uint)(keyFrame.FrameNumber - fromFrame),
                Name = morph.Name,
                Weight = keyFrame.Weight,
            });
    }

    protected override bool WriteMotion(VpdDocument vpdDocument, float motion, bool changedOnly)
    {
        const float precision = 0.000001f;

        if (changedOnly && motion <= precision) return false;
        
        vpdDocument.Morphs.Add(new()
        {
            MorphName = morph.Name,
            Weight = motion,
        });

        return true;
    }
}