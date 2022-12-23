using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public class EffectTransformer : MotionTransformer
{
    readonly Effect effect;

    public override bool CanTranslateByLocal => true;

    public override bool CanRotateByLocal => false;

    public EffectTransformer(Effect effect)
        : base(null, effect.Frames.GetKeyFrames()) =>
        this.effect = effect;

    protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) =>
        effect.Frames.ReplaceAllKeyFrames(keyFrames.ToList());

    protected override MotionFrameData GetFrame(long frameNumber) =>
        effect.Frames.GetFrame(frameNumber);
}