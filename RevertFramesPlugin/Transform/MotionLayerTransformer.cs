using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

public class MotionLayerTransformer : MotionTransformer
{
    readonly MotionLayer motionLayer;

    MotionLayerTransformer(MotionLayer motionLayer)
        : base(motionLayer.Frames.GetKeyFrames()) => 
        this.motionLayer = motionLayer;
    
    public static IEnumerable<MotionLayerTransformer> FromBone(Bone bone) =>
        bone.Layers
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(layer));
    
    public static IEnumerable<MotionLayerTransformer> FromAccessory(Accessory accessory) =>
        accessory.Layers
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(layer));

    protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) =>
        motionLayer.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
}