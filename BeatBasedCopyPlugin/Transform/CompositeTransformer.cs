using System.Collections.Generic;
using System.Linq;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public class CompositeTransformer : ITransformer
{
    readonly IReadOnlyCollection<ITransformer> transformers;

    public long? SelectedMinimumFrameNumber =>
        transformers.Min(x => x.SelectedMinimumFrameNumber);

    public long? SelectedMaximumFrameNumber =>
        transformers.Max(x => x.SelectedMaximumFrameNumber);
    
    CompositeTransformer(IReadOnlyCollection<ITransformer> transformers) => 
        this.transformers = transformers;

    public static ITransformer? Create(IReadOnlyCollection<ITransformer> transformers) =>
        transformers.Any() ? new CompositeTransformer(transformers) : null;

    public void Copy(BeatContext context, long markerPosition)
    {
        foreach (var transformer in transformers)
            transformer.Copy(context, markerPosition);
    }
}