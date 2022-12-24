using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Transform;

public class Transformer
{
    public static ITransformer? Create(Model? model) =>
        model != null
            ? CompositeTransformer.Create(BoneTransformer.FromModel(model)
                .Cast<ITransformer>()
                .Concat(MorphTransformer.FromModel(model))
                .ToArray())
            : null;
}