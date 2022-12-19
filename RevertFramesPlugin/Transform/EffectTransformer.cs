using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

public class EffectTransformer : MotionTransformer
{
    readonly Effect effect;

    public EffectTransformer(Effect effect)
        : base(effect.Frames.GetKeyFrames()) =>
        this.effect = effect;

    protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) =>
        effect.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
}