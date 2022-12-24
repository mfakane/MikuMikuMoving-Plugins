using System.Collections.Generic;
using System.Linq;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Transform;

public class CompositeTransformer : ITransformer
{
    readonly IReadOnlyCollection<ITransformer> transformers;

    CompositeTransformer(IReadOnlyCollection<ITransformer> transformers) => 
        this.transformers = transformers;

    public static ITransformer? Create(IReadOnlyCollection<ITransformer> transformers) =>
        transformers.Any() ? new CompositeTransformer(transformers) : null;

    public void PreviewTransform(ModelBinding binding)
    {
        foreach (var transformer in transformers)
            transformer.PreviewTransform(binding);
    }

    public void SaveTransform(ModelBinding binding)
    {
        foreach (var transformer in transformers)
            transformer.SaveTransform(binding);
    }

    public void ResetPreview()
    {
        foreach (var transformer in transformers)
            transformer.ResetPreview();
    }
}