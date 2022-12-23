using System.Collections.Generic;
using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public abstract class MotionTransformer : KeyFrameTransformer<MotionFrameData>
{
    readonly Bone? bone;
    
    public MotionTransformer(Bone? bone, IReadOnlyCollection<MotionFrameData> keyFrames) 
        : base(keyFrames) =>
        this.bone = bone;

    protected abstract override void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames);

    protected override void ApplyNoiseToKeyFrame(MotionFrameData keyFrame, NoiseValue value, bool translateByLocal, bool rotateByLocal)
    {
        if (bone?.BoneFlags.HasFlag(BoneType.Rotate) != false)
            keyFrame.Quaternion = ApplyRotation(bone, keyFrame.Quaternion, value.Rotation, rotateByLocal);

        if (bone?.BoneFlags.HasFlag(BoneType.XYZ) != false)
            keyFrame.Position = ApplyTranslation(bone,  keyFrame.Position, value.Position, keyFrame.Quaternion, translateByLocal);
    }
    
    static Quaternion ApplyRotation(Bone? bone, Quaternion currentRotation, Vector3 rotationOffsetRadians, bool transformByLocal)
    {
        if (!transformByLocal ||
            bone == null ||
            !bone.BoneFlags.HasFlag(BoneType.LocalAxis) &&
            bone.LocalAxisX == Vector3.UnitX &&
            bone.LocalAxisY == Vector3.UnitY &&
            bone.LocalAxisZ == Vector3.UnitZ)
            return currentRotation *
                   Quaternion.RotationYawPitchRoll(
                       rotationOffsetRadians.Y,
                       rotationOffsetRadians.X,
                       rotationOffsetRadians.Z
                   );

        return Quaternion.RotationAxis(bone.LocalAxisY, rotationOffsetRadians.Y) *
               Quaternion.RotationAxis(bone.LocalAxisX, rotationOffsetRadians.X) *
               Quaternion.RotationAxis(bone.LocalAxisZ, rotationOffsetRadians.Z) *
               currentRotation;
    }

    static Vector3 ApplyTranslation(Bone? bone, Vector3 currentTranslation, Vector3 translationOffset, Quaternion currentRotation, bool transformByLocal)
    {
        if (!transformByLocal)
            return currentTranslation + translationOffset;

        var localMatrix = bone?.BoneFlags.HasFlag(BoneType.LocalAxis) == true
            ? Matrix.LookAtLH(Vector3.Zero, bone.LocalAxisZ, bone.LocalAxisY)
            : Matrix.Identity;
            
        return currentTranslation +
               Vector3.TransformCoordinate(translationOffset, localMatrix * Matrix.RotationQuaternion(currentRotation));
    }
}