using System.Collections.Generic;
using System.Linq;
using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin.Transform;

public abstract class MotionTransformer : ITransformer
{
    readonly Bone? bone;
    readonly MotionData initialPreviewMotion;
    
    protected MotionTransformer(Bone? bone, MotionData initialPreviewMotion)
    {
        this.bone = bone;
        this.initialPreviewMotion = initialPreviewMotion;
    }
    
    protected abstract void SetCurrentPreviewMotion(MotionData motion);
    protected abstract IReadOnlyCollection<MotionFrameData> GetKeyFrames();
    protected abstract void ReplaceAllKeyFrames(IEnumerable<MotionFrameData> keyFrames);
  
    protected virtual Vector3 ConvertToWorld(Vector3 local) => local;
    
    public void PreviewTransform(Vector3 origin, Vector3 rotation, bool translateOnly) =>
        SetCurrentPreviewMotion(TransformCore(initialPreviewMotion, origin, rotation, translateOnly));

    public void SaveTransform(Vector3 origin, Vector3 rotation, bool translateOnly)
    {
        var keyFrames = GetKeyFrames();
            
        foreach (var frame in keyFrames.Where(frame => frame.Selected))
        {
            var currentMotion = new MotionData(frame.Position, frame.Quaternion);
            var newMotion = TransformCore(currentMotion, origin, rotation, translateOnly);

            frame.Quaternion = newMotion.Rotation;
            frame.Position = newMotion.Move;
            frame.Selected = true;
        } 
        
        ReplaceAllKeyFrames(keyFrames);
    }

    MotionData TransformCore(MotionData motion, Vector3 origin, Vector3 rotation, bool translateOnly)
    {
        var rotationQuaternion = Quaternion.RotationYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
        var newMotion = new MotionData(motion.Move, motion.Rotation);

        if (bone?.BoneFlags.HasFlag(BoneType.Rotate) != false && !translateOnly)
            newMotion.Rotation = rotationQuaternion * motion.Rotation;

        if (bone?.BoneFlags.HasFlag(BoneType.XYZ) == false) return newMotion;
        
        // origin はワールド座標系なのでボーンローカル座標に変換してから適用する
        var initialWorldPosition = Matrix.Translation(ConvertToWorld(Vector3.Zero));
        var boneLocalOrigin = Vector3.TransformCoordinate(origin, Matrix.Invert(initialWorldPosition));
        var rotationByOriginMatrix = Matrix.Transformation(Vector3.Zero, Quaternion.Identity, new(1), boneLocalOrigin, rotationQuaternion, Vector3.Zero);
            
        newMotion.Move = Vector3.TransformCoordinate(motion.Move, rotationByOriginMatrix);

        return newMotion;
    }

    public void ResetPreview() =>
        SetCurrentPreviewMotion(initialPreviewMotion);
}