using System.Collections.Generic;
using System.Linq;
using DxMath;
using Linearstar.Keystone.IO.MikuMikuDance;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public class BoneTransformer : KeyFrameTransformer<MotionFrameData, MotionData>
{
    readonly Bone bone;

    BoneTransformer(Bone bone)
        : base(bone.Layers.First().Frames.GetSelectedKeyFrames(), bone.CurrentLocalMotion) =>
        this.bone = bone;

    public static IEnumerable<BoneTransformer> FromModel(Model model) =>
        model.Bones
            .Where(x => x.BoneFlags.HasFlag(BoneType.Operatable) && x.BoneFlags.HasFlag(BoneType.Draw))
            .Select(x => new BoneTransformer(x));
    
    protected override void WriteKeyFrames(VmdDocument vmdDocument, IReadOnlyCollection<MotionFrameData> keyFrames, long fromFrame, long toFrame)
    {
        foreach (var keyFrame in keyFrames)
            vmdDocument.BoneFrames.Add(new()
            {
                FrameTime = (uint)(keyFrame.FrameNumber - fromFrame),
                Name = bone.Name,
                Position = keyFrame.Position.ToArray(),
                Quaternion = keyFrame.Quaternion.ToArray(),
                RotationInterpolation = new[] { keyFrame.InterpolRA.ToVmd(), keyFrame.InterpolRB.ToVmd() },
                XInterpolation = new[] { keyFrame.InterpolXA.ToVmd(), keyFrame.InterpolXB.ToVmd() },
                YInterpolation = new[] { keyFrame.InterpolYA.ToVmd(), keyFrame.InterpolYB.ToVmd() },
                ZInterpolation = new[] { keyFrame.InterpolZA.ToVmd(), keyFrame.InterpolZB.ToVmd() },
            });
    }

    protected override bool WriteMotion(VpdDocument vpdDocument, MotionData motion, bool changedOnly)
    {
        const float precision = 0.000001f;

        if (changedOnly &&
            motion.Move.Length() <= precision &&
            1 - Quaternion.Dot(Quaternion.Identity, motion.Rotation) <= precision)
            return false;
        
        vpdDocument.Bones.Add(new()
        {
            BoneName = bone.Name,
            Position = motion.Move.ToArray(),
            Quaternion = motion.Rotation.ToArray(),
        });

        return true;
    }
}