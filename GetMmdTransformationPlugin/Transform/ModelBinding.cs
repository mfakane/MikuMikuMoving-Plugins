using System.Collections.Generic;
using System.Linq;
using Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Transform;

public record ModelBinding(
    MmdModel MmdModel,
    IReadOnlyDictionary<int, BoneBinding> Bones,
    IReadOnlyDictionary<int, MorphBinding> Morphs
)
{
    public static ModelBinding Create(Model model, MmdModel mmdModel)
    {
        var mmdBonesByName = mmdModel.Bones.ToDictionary(x => x.Name);
        var mmdMorphsByName = mmdModel.Morphs.ToDictionary(x => x.Name);

        return new ModelBinding(
            mmdModel,
            model.Bones
                .Select((x, index) => mmdBonesByName.TryGetValue(x.Name, out var mmdBone)
                    ? new BoneBinding(index, x, mmdBone)
                    : null)
                .Where(x => x != null)
                .ToDictionary(x => x!.Index)!,
            model.Morphs.Select((x, index) => mmdMorphsByName.TryGetValue(x.Name, out var mmdMorph)
                    ? new MorphBinding(index, x, mmdMorph)
                    : null)
                .Where(x => x != null)
                .ToDictionary(x => x!.Index)!
        );
    }
}

public record BoneBinding(
    int Index,
    Bone Bone,
    MmdBone MmdBone
);

public record MorphBinding(
    int Index,
    Morph Morph,
    MmdMorph MmdMorph
);