using System.Collections.Generic;
using System.Linq;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

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
    
    public void RevertFrames(long fromFrame, long toFrame)
    {
        foreach (var transformer in transformers)
            transformer.RevertFrames(fromFrame, toFrame);
    }
}