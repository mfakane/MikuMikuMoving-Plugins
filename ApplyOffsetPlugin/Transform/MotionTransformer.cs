using System.Collections.Generic;
using System.Linq;
using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public abstract class MotionTransformer : ITransformer
{
    public abstract IValueTransformer6 GetPositionRotationTransformer();

    IValueTransformer3? ITransformer.GetPositionOnlyTransformer() => null;
    IValueTransformer3? ITransformer.GetRotationOnlyTransformer() => null;
    IValueTransformer? ITransformer.GetWeightTransformer() => null;
    IValueTransformer? ITransformer.GetDistanceTransformer() => null;
    IValueTransformer3? ITransformer.GetColorTransformer() => null;
    
    protected abstract class PositionRotationTransformer : IValueTransformer6
    {
        readonly Bone? bone;
        readonly MotionData initialPreviewMotion;

        public bool CanTranslateByLocal => true;
        public bool CanRotateByLocal => true;

        protected PositionRotationTransformer(Bone? bone, MotionData initialPreviewMotion)
        {
            this.bone = bone;
            this.initialPreviewMotion = initialPreviewMotion;
        }

        protected abstract void SetCurrentPreviewMotion(MotionData motion);
        protected abstract IReadOnlyCollection<MotionFrameData> GetKeyFrames();
        protected abstract void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames);

        public void PreviewTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal) =>
            SetCurrentPreviewMotion(TransformCore(initialPreviewMotion, bone, translation, rotation, translateByLocal, rotateByLocal));

        public void SaveTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal)
        {
            var keyFrames = GetKeyFrames();
            
            foreach (var frame in keyFrames.Where(frame => frame.Selected))
            {
                var currentMotion = new MotionData(frame.Position, frame.Quaternion);
                var newMotion = TransformCore(currentMotion, bone, translation, rotation, translateByLocal, rotateByLocal);

                frame.Quaternion = newMotion.Rotation;
                frame.Position = newMotion.Move;
                frame.Selected = true;
            }

            ReplaceAllKeyFrames(keyFrames);
        }

        static MotionData TransformCore(MotionData motion, Bone? bone, Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal)
        {
            var newMotion = new MotionData(motion.Move, motion.Rotation);

            if (bone?.BoneFlags.HasFlag(BoneType.Rotate) != false)
                newMotion.Rotation = ApplyRotation(bone, motion.Rotation, rotation, rotateByLocal);

            if (bone?.BoneFlags.HasFlag(BoneType.XYZ) != false)
                newMotion.Move = ApplyTranslation(bone, motion.Move, translation, newMotion.Rotation, translateByLocal);

            return newMotion;
        }

        public void ResetPreview() =>
            SetCurrentPreviewMotion(initialPreviewMotion);

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
}