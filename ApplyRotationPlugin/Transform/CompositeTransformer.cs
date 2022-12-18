using System.Collections.Generic;
using System.Linq;
using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin.Transform;

public class CompositeTransformer : ITransformer
{
    readonly IReadOnlyCollection<ITransformer> transformers;

    CompositeTransformer(IReadOnlyCollection<ITransformer> transformers) =>
        this.transformers = transformers;
    
    public static ITransformer? Create(IReadOnlyCollection<ITransformer> transformers) =>
        transformers.Any() ? new CompositeTransformer(transformers) : null;

    public void PreviewTransform(Vector3 origin, Vector3 rotation, bool translateOnly)
    {
        foreach (var transformer in transformers)
            transformer.PreviewTransform(origin, rotation, translateOnly);
    }

    public void SaveTransform(Vector3 origin, Vector3 rotation, bool translateOnly)
    {
        foreach (var transformer in transformers)
            transformer.SaveTransform(origin, rotation, translateOnly);
    }

    public void ResetPreview()
    {
        foreach (var transformer in transformers)
            transformer.ResetPreview();
    }
}