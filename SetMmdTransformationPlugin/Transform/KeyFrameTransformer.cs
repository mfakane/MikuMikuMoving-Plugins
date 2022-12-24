using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO.MikuMikuDance;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public abstract class KeyFrameTransformer<TFrame, TMotion> : ITransformer
    where TFrame : FrameData
{
    readonly IReadOnlyCollection<TFrame> selectedFrames;
    readonly TMotion? motion;

    public long? SelectedMinimumFrameNumber =>
        selectedFrames.DefaultIfEmpty(null).Min(x => x?.FrameNumber);

    public long? SelectedMaximumFrameNumber =>
        selectedFrames.DefaultIfEmpty(null).Max(x => x?.FrameNumber);

    public bool HasKeyFrames => selectedFrames.Any();

    public bool HasMotion => motion != null;
    
    protected KeyFrameTransformer(IReadOnlyCollection<TFrame> selectedFrames, TMotion? motion)
    {
        this.selectedFrames = selectedFrames;
        this.motion = motion;
    }

    protected abstract void WriteKeyFrames(VmdDocument vmdDocument, IReadOnlyCollection<TFrame> keyFrames, long fromFrame, long toFrame);

    protected virtual bool WriteMotion(VpdDocument vpdDocument, TMotion motion, bool changedOnly) =>
        false;

    public virtual bool WriteTo(VmdDocument vmdDocument, long fromFrame, long toFrame)
    {
        if (!selectedFrames.Any()) return false;
        
        WriteKeyFrames(vmdDocument,
            selectedFrames.Where(x => x.FrameNumber >= fromFrame && x.FrameNumber <= toFrame).ToArray(), fromFrame,
            toFrame);

        return true;
    }

    public virtual bool WriteTo(VpdDocument vpdDocument, bool changedOnly) =>
        motion != null && WriteMotion(vpdDocument, motion, changedOnly);
}