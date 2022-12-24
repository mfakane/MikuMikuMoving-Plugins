using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO.MikuMikuDance;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public class CompositeTransformer : ITransformer
{
    readonly IReadOnlyCollection<ITransformer> transformers;
  
    public long? SelectedMinimumFrameNumber =>
        transformers.Min(x => x.SelectedMinimumFrameNumber);

    public long? SelectedMaximumFrameNumber =>
        transformers.Max(x => x.SelectedMaximumFrameNumber);

    public bool HasKeyFrames =>
        transformers.Any(x => x.HasKeyFrames);

    public bool HasMotion =>
        transformers.Any(x => x.HasMotion);
    
    protected CompositeTransformer(IReadOnlyCollection<ITransformer> transformers) => 
        this.transformers = transformers;

    public static ITransformer? Create(IReadOnlyCollection<ITransformer> transformers) =>
        transformers.Any() ? new CompositeTransformer(transformers) : null;

    public virtual bool WriteTo(VmdDocument vmdDocument, long fromFrame, long toFrame) => 
        transformers.Aggregate(false, (current, transformer) => 
            current | transformer.WriteTo(vmdDocument, fromFrame, toFrame));

    public virtual bool WriteTo(VpdDocument vpdDocument, bool changedOnly) =>
        transformers.Aggregate(false, (current, transformer) => 
            current | transformer.WriteTo(vpdDocument, changedOnly));
}