using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public class ModelTransformer : CompositeTransformer
{
    readonly Model model;

    ModelTransformer(Model model, IReadOnlyCollection<ITransformer> transformers)
        : base(transformers)
    {
        this.model = model;
    }

    public static ITransformer? Create(Model model)
    {
        var transformers = BoneTransformer.FromModel(model)
            .Cast<ITransformer>()
            .Concat(MorphTransformer.FromModel(model))
            .ToArray();

        return transformers.Any() ? new ModelTransformer(model, transformers) : null;
    }
    
    public override bool WriteTo(VmdDocument vmdDocument, long fromFrame, long toFrame)
    {
        vmdDocument.ModelName = model.Name;
        return base.WriteTo(vmdDocument, fromFrame, toFrame);
    }

    public override bool WriteTo(VpdDocument vpdDocument, bool changedOnly)
    {
        vpdDocument.ParentFileName = "miku.osm";
        return base.WriteTo(vpdDocument, changedOnly);
    }
}