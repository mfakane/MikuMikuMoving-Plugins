using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public class EffectTransformer : MotionTransformer
{
    readonly Effect effect;

    public EffectTransformer(Effect effect)
    {
        this.effect = effect;
    }

    public override IValueTransformer6 GetPositionRotationTransformer() =>
        new EffectMotionTransformer(effect);
    
    class EffectMotionTransformer : PositionRotationTransformer
    {
        readonly Effect effect;

        public EffectMotionTransformer(Effect effect)
            : base(null, effect.CurrentLocalMotion)
        {
            this.effect = effect;
        }

        protected override void SetCurrentPreviewMotion(MotionData motion) => 
            effect.CurrentLocalMotion = motion;

        protected override IReadOnlyCollection<MotionFrameData> GetKeyFrames() =>
            effect.Frames.GetKeyFrames();
        
        protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) =>
            effect.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
    }
}