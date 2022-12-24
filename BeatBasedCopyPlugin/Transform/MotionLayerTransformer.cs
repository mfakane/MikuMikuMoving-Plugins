using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public class MotionLayerTransformer : KeyFrameTransformer<MotionFrameData>
{
    readonly MotionLayer layer;
    
    MotionLayerTransformer(MotionLayer layer)
        : base(layer.Frames.GetKeyFrames()) =>
        this.layer = layer;
    
    public static IEnumerable<MotionLayerTransformer> FromBone(Bone bone) =>
        bone.Layers
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(layer));

    public static IEnumerable<MotionLayerTransformer> FromAccessory(Accessory accessory) =>
        accessory.Layers
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(layer));

    protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) =>
        layer.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
}