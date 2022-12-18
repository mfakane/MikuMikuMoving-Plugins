using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public class MotionLayerTransformer : MotionTransformer
{
    readonly Bone? bone;
    readonly MotionLayer layer;

    MotionLayerTransformer(Bone? bone, MotionLayer layer)
    {
        this.bone = bone;
        this.layer = layer;
    }
    
    public static IEnumerable<MotionLayerTransformer> FromBone(Bone bone) =>
        bone.Layers
            .Where(layer => layer.Selected || layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(bone, layer));
    
    public static IEnumerable<MotionLayerTransformer> FromAccessory(Accessory accessory) =>
        accessory.Layers
            .Where(layer => layer.Selected || layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(null, layer));
    
    public override IValueTransformer6 GetPositionRotationTransformer() =>
        new MotionLayerValueTransformer(bone, layer);

    class MotionLayerValueTransformer : PositionRotationTransformer
    {
        readonly MotionLayer layer;

        public MotionLayerValueTransformer(Bone? bone, MotionLayer layer)
            : base(bone, layer.CurrentLocalMotion)
        {
            this.layer = layer;
        }

        protected override void SetCurrentPreviewMotion(MotionData motion) =>
            layer.CurrentLocalMotion = motion;

        protected override IReadOnlyCollection<MotionFrameData> GetKeyFrames() =>
            layer.Frames.GetKeyFrames();

        protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) =>
            layer.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
    }
}