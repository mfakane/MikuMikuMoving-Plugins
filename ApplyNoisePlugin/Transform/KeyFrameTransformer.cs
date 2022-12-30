using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public abstract class KeyFrameTransformer<T> : ITransformer
    where T : FrameData
{
    readonly IReadOnlyCollection<T> keyFrames;
    readonly IReadOnlyCollection<T> selectedKeyFrames;

    public long? SelectedMinimumFrameNumber =>
        selectedKeyFrames.Any() ? selectedKeyFrames.Min(x => x.FrameNumber) : null;

    public long? SelectedMaximumFrameNumber =>
        selectedKeyFrames.Any() ? selectedKeyFrames.Max(x => x.FrameNumber) : null;

    public bool CanApplyTranslation => true;

    public abstract bool CanTranslateByLocal { get; }

    public bool CanApplyRotation => true;

    public abstract bool CanRotateByLocal { get; }

    protected KeyFrameTransformer(IReadOnlyCollection<T> keyFrames)
    {
        this.keyFrames = keyFrames;
        selectedKeyFrames = this.keyFrames.Where(x => x.Selected).ToArray();
    }
    
    protected abstract void ReplaceAllKeyFrames(IEnumerable<T> keyFrames);
    protected abstract T GetFrame(long frameNumber);
    protected abstract void ApplyNoiseToKeyFrame(T keyFrame, NoiseValue value, bool translateByLocal, bool rotateByLocal);

    IEnumerable<T> GetAppliedKeyFrames(NoiseContext context, bool translateByLocal, bool rotateByLocal)
    {
        // 時折フレーム番号が重複していることがあるので ToLookup してから最初に出現したもののみ取り扱う
        var existingKeyFrames = keyFrames.ToLookup(x => x.FrameNumber).ToDictionary(x => x.Key, x => x.First());
        var noiseValues = context.GetNoiseValues(new HashSet<long>(existingKeyFrames.Keys));
        var newKeyFrames = new Dictionary<long, T>();

        foreach (var value in noiseValues)
        {
            var newKeyFrame = existingKeyFrames.TryGetValue(value.FrameNumber, out var existingKeyFrame)
                ? existingKeyFrame
                : GetFrame(value.FrameNumber);

            ApplyNoiseToKeyFrame(newKeyFrame, value.Value, translateByLocal, rotateByLocal);
            newKeyFrame.FrameNumber = value.ShiftedFrameNumber;
            newKeyFrame.Selected = true;
            
            newKeyFrames[newKeyFrame.FrameNumber] = newKeyFrame;
            existingKeyFrames.Remove(value.FrameNumber);
        }

        // 適用範囲外のものはそのままコピーする
        foreach (var keyFrame in existingKeyFrames.Where(x => !x.Value.Selected))
            newKeyFrames[keyFrame.Key] = keyFrame.Value;

        return newKeyFrames.Values;
    }

    public void ApplyNoise(NoiseContext context, bool translateByLocal, bool rotateByLocal) =>
        ReplaceAllKeyFrames(GetAppliedKeyFrames(context, translateByLocal, rotateByLocal));
}