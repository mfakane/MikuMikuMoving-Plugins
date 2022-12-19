using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

public abstract class KeyFrameTransformer<T, TInterpolation> : ITransformer
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
    protected abstract TInterpolation GetInterpolation(T keyFrame);
    protected abstract TInterpolation GetInvertedInterpolation(T keyFrame);
    protected abstract void SetInterpolation(T keyFrame, TInterpolation interpolation);
    
    public void RevertFrames(long fromFrame, long toFrame)
    {
        // 何も選択してなかったり単体のキーフレームに対しては反転するものがない
        if (selectedKeyFrames.Count < 2)
            return;
        
        var framesToRevert = selectedKeyFrames
            .Where(x => fromFrame <= x.FrameNumber && x.FrameNumber <= toFrame)
            .ToArray();
        
        // 補完曲線はそのキーフレームと次のキーフレームの間に対して適用されるため、逆順にするには一つ先のキーフレームに付け替える必要がある
        // また、最後の補完曲線は適用範囲外のため手をつけない
        var revertedInterpolationsByRevertedFrames = selectedKeyFrames
            .Take(framesToRevert.Length - 1)
            .Zip(framesToRevert.Skip(1),
                (a, b) => new
                {
                    FrameA = a,
                    FrameB = b,
                })
            .Select(x => new
            {
                // フレーム番号も曲線も前後を入れ替える
                FrameNumber = x.FrameB.FrameNumber - toFrame + fromFrame,
                Interpolation = GetInvertedInterpolation(x.FrameA),
            })
            .Concat(new[] { framesToRevert.Last() }
                .Select(x => new
                {
                    x.FrameNumber,
                    Interpolation = GetInterpolation(x),
                }))
            .ToDictionary(x => x.FrameNumber);

        var newFrames = framesToRevert
            .Select(frame =>
            {
                var newFrameNumber = fromFrame + toFrame - frame.FrameNumber;
                var newFrame = (T)frame.Clone();
                
                newFrame.FrameNumber = newFrameNumber;

                if (revertedInterpolationsByRevertedFrames.TryGetValue(newFrameNumber, out var newInterpolation))
                {
                    SetInterpolation(newFrame, newInterpolation.Interpolation);
                }

                return newFrame;
            })
            .ToArray();

        var frameNumbersToRemove = new HashSet<long>(framesToRevert.Select(x => x.FrameNumber));

        ReplaceAllKeyFrames(keyFrames
            .Where(x => !frameNumbersToRemove.Contains(x.FrameNumber))
            .Concat(newFrames));
    }
}