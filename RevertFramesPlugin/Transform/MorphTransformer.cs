using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

public class MorphTransformer : ITransformer
{
    readonly Morph morph;
    readonly IReadOnlyCollection<MorphFrameData> keyFrames;
    readonly IReadOnlyCollection<MorphFrameData> selectedKeyFrames;
    
    public long? SelectedMinimumFrameNumber =>
        selectedKeyFrames.Any() ? selectedKeyFrames.Max(x => x.FrameNumber) : null;

    public long? SelectedMaximumFrameNumber =>
        selectedKeyFrames.Any() ? selectedKeyFrames.Max(x => x.FrameNumber) : null;
    
    MorphTransformer(Morph morph)
    {
        this.morph = morph;
        keyFrames = morph.Frames.GetKeyFrames();
        selectedKeyFrames = keyFrames.Where(x => x.Selected).ToArray();
    }
    
    public static IEnumerable<MorphTransformer> FromModel(Model model) =>
        model.Morphs
            .Where(morph => morph.SelectedFrames.Any())
            .Select(morph => new MorphTransformer(morph));

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
        var revertedInterpolationsByRevertedFrames = framesToRevert
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
                InterpolA = x.FrameA.InterpolB.Invert(),
                InterpolB = x.FrameA.InterpolA.Invert(),
            })
            .Concat(new[] { framesToRevert.Last() }
                .Select(x => new
                {
                    x.FrameNumber,
                    x.InterpolA,
                    x.InterpolB,
                }))
            .ToDictionary(x => x.FrameNumber);

        var newFrames = framesToRevert
            .Select(frame =>
            {
                var newFrameNumber = fromFrame + toFrame - frame.FrameNumber;
                var newFrame = new MorphFrameData(newFrameNumber, frame.Weight)
                {
                    Selected = true,
                };

                if (revertedInterpolationsByRevertedFrames.TryGetValue(newFrameNumber, out var newInterpolation))
                {
                    newFrame.InterpolA = newInterpolation.InterpolA;
                    newFrame.InterpolB = newInterpolation.InterpolB;
                }

                return newFrame;
            })
            .ToArray();

        var frameNumbersToRemove = new HashSet<long>(framesToRevert.Select(x => x.FrameNumber));
        var newKeyFrames = keyFrames
            .Where(x => !frameNumbersToRemove.Contains(x.FrameNumber))
            .Concat(newFrames)
            .ToList();
        
        morph.Frames.ReplaceAllKeyFrames(newKeyFrames);
    }
}