using System.Collections.Generic;
using System.Linq;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public class CompositeTransformer : ITransformer
{
    readonly IReadOnlyCollection<ITransformer> transformers;

    public long? SelectedMinimumFrameNumber =>
        transformers.Min(x => x.SelectedMinimumFrameNumber);

    public long? SelectedMaximumFrameNumber =>
        transformers.Max(x => x.SelectedMaximumFrameNumber);

    public bool CanApplyTranslation =>
        transformers.Any(x => x.CanApplyTranslation);

    public bool CanTranslateByLocal =>
        transformers.Any(x => x.CanTranslateByLocal);

    public bool CanApplyRotation =>
        transformers.Any(x => x.CanApplyRotation);

    public bool CanRotateByLocal => 
        transformers.Any(x => x.CanRotateByLocal);

    CompositeTransformer(IReadOnlyCollection<ITransformer> transformers) =>
        this.transformers = transformers;
    
    public static ITransformer? Create(IReadOnlyCollection<ITransformer> transformers) =>
        transformers.Any() ? new CompositeTransformer(transformers) : null;
    
    public void ApplyNoise(NoiseContext context, bool translateByLocal, bool rotateByLocal)
    {
        foreach (var transformer in transformers)
            transformer.ApplyNoise(context, translateByLocal, rotateByLocal);
    }
}