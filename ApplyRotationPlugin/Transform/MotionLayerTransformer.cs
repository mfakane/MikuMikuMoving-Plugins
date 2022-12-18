using System.Collections.Generic;
using System.Linq;
using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin.Transform;

public class MotionLayerTransformer : MotionTransformer
{
    readonly Model? model;
    readonly Bone? bone;
    readonly MotionLayer layer;

    MotionLayerTransformer(Model? model, Bone? bone, MotionLayer layer)
        : base(bone, layer.CurrentLocalMotion)
    {
        this.model = model;
        this.bone = bone;
        this.layer = layer;
    }
    
    public static IEnumerable<MotionLayerTransformer> FromBone(Model model, Bone bone) =>
        bone.Layers
            .Where(layer => layer.Selected || layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(model, bone, layer));
    
    public static IEnumerable<MotionLayerTransformer> FromAccessory(Accessory accessory) =>
        accessory.Layers
            .Where(layer => layer.Selected || layer.SelectedFrames.Any())
            .Select(layer => new MotionLayerTransformer(null, null, layer));

    protected override void SetCurrentPreviewMotion(MotionData motion) =>
        layer.CurrentLocalMotion = motion;

    protected override IReadOnlyCollection<MotionFrameData> GetKeyFrames() => 
        layer.Frames.GetKeyFrames();

    protected override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames) => 
        layer.Frames.ReplaceAllKeyFrames(keyFrames.ToList());

    protected override Vector3 ConvertToWorld(Vector3 local)
    {
        if (model == null || bone == null || bone.ParentBoneID == -1) return local;

        for (var current = bone; current.ParentBoneID != -1; current = model.Bones[current.ParentBoneID])
        {
            local += current.InitialPosition;
        }

        return local;
    }
}