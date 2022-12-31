using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public class MotionLayerTransformer : MotionTransformer
{
    readonly MotionLayer motionLayer;
    readonly Bone? bone;
    
    public override bool CanTranslateByLocal => true;

    public override bool CanRotateByLocal => bone?.BoneFlags.HasFlag(BoneType.Rotate) ?? false;

    public override bool CanApplyWeight => false;

    MotionLayerTransformer(Bone? bone, MotionLayer motionLayer)
        : base(bone, motionLayer.Frames.GetKeyFrames())
    {
        this.bone = bone;
        this.motionLayer = motionLayer;
    }

    public static IEnumerable<MotionLayerTransformer> FromBone(Bone bone) =>
        bone.Layers
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(bone, layer));

    public static IEnumerable<MotionLayerTransformer> FromAccessory(Accessory accessory) =>
        accessory.Layers
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(null, layer));

    protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) =>
        motionLayer.Frames.ReplaceAllKeyFrames(keyFrames.ToList());

    protected override MotionFrameData GetFrame(long frameNumber) =>
        motionLayer.Frames.GetFrame(frameNumber);
}