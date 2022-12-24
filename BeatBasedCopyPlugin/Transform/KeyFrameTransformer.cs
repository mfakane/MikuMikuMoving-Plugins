using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public abstract class KeyFrameTransformer<T> : ITransformer
    where T : FrameData
{
    readonly IReadOnlyCollection<T> keyFrames;
    readonly IReadOnlyCollection<T> selectedKeyFrames;

    public long? SelectedMinimumFrameNumber =>
        selectedKeyFrames.Any() ? selectedKeyFrames.Min(x => x.FrameNumber) : null;

    public long? SelectedMaximumFrameNumber =>
        selectedKeyFrames.Any() ? selectedKeyFrames.Max(x => x.FrameNumber) : null;
    
    protected KeyFrameTransformer(IReadOnlyCollection<T> keyFrames)
    {
        this.keyFrames = keyFrames;
        selectedKeyFrames = this.keyFrames.Where(x => x.Selected).ToArray();
    }
    
    protected abstract void ReplaceAllKeyFrames(IEnumerable<T> keyFrames);
    
    public void Copy(BeatContext context, long markerPosition)
    {
        var newKeyFrames = keyFrames.ToDictionary(x => x.FrameNumber);
        var sourceOrigin = SelectedMinimumFrameNumber!.Value;
        
        foreach (var destinationFrame in context.GetDestinationFrames(markerPosition))
        {
            foreach (var sourceFrame in selectedKeyFrames)
            {
                var newFrame = (T)sourceFrame.Clone();

                newFrame.FrameNumber = newFrame.FrameNumber - sourceOrigin + destinationFrame;
                newKeyFrames[newFrame.FrameNumber] = newFrame;
            }
        }
        
        ReplaceAllKeyFrames(newKeyFrames.Values.ToArray());
    }
}