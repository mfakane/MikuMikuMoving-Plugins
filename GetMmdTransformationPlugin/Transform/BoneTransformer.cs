using System.Collections.Generic;
using System.Linq;
using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Transform;

public class BoneTransformer : ITransformer
{
    readonly Bone bone;
    readonly State initialState;
    Target? currentTarget;

    BoneTransformer(Bone bone)
    {
        this.bone = bone;
        initialState = new State(bone.Layers[0]);
    }

    public static IEnumerable<BoneTransformer> FromModel(Model model) =>
        model.Bones
            .Where(x => (x.BoneFlags.HasFlag(BoneType.Rotate) || x.BoneFlags.HasFlag(BoneType.XYZ)) && x.BoneFlags.HasFlag(BoneType.Operatable) && x.BoneFlags.HasFlag(BoneType.Draw))
            .Select(x => new BoneTransformer(x));

    public void PreviewTransform(ModelBinding binding)
    {
        if (!binding.Bones.TryGetValue(bone.BoneID, out var target))
        {
            currentTarget = null;
            return;
        }
        
        if (currentTarget?.Bone.MmdBone.Model.Index != binding.MmdModel.Index)
        {
            binding.Bones.TryGetValue(bone.ParentBoneID, out var parentBoneBinding);

            BoneBinding? inheritBaseBoneBinding = null;
            BoneBinding? inheritBaseParentBoneBinding = null;
            
            if (bone.InheraRate != 0 && binding.Bones.TryGetValue(bone.InhereBoneID, out inheritBaseBoneBinding))
                binding.Bones.TryGetValue(inheritBaseBoneBinding.Bone.ParentBoneID, out inheritBaseParentBoneBinding);

            currentTarget = new Target(target, parentBoneBinding, inheritBaseBoneBinding, inheritBaseParentBoneBinding);
        }

        if (currentTarget == null) return;

        var newMotion = new MotionData();
        var matrix = Matrix.Translation(-bone.InitialPosition) * currentTarget.Bone.MmdBone.Transform;

        // MMD から取得できるのはワールド座標系での変形だが、セットしたいのはボーンのローカル座標系なので、親の変形を打ち消す
        if (currentTarget.Parent != null)
            matrix = ConvertToLocal(
                matrix,
                currentTarget.Parent.MmdBone.Transform,
                currentTarget.Parent.Bone.InitialPosition
            );
        
        // MMD から取得できる変形には回転連動が含まれているが、セットしたいのは連動を抜いた分のため、打ち消す
        if (currentTarget.InheritBase != null)
        {
            var inheritBaseBone = currentTarget.InheritBase.Bone;
            var inheritedMatrix = currentTarget.InheritBase.MmdBone.Transform;

            // 連動親の親ボーンの変形を打ち消してローカル座標系での変形を得る
            if (currentTarget.InheritBaseParent != null)
                inheritedMatrix = ConvertToLocal(
                    inheritedMatrix, 
                    currentTarget.InheritBaseParent.MmdBone.Transform,
                    currentTarget.InheritBaseParent.Bone.InitialPosition
                );
            
            if (!bone.BoneFlags.HasFlag(BoneType.InhereRotate) || !bone.BoneFlags.HasFlag(BoneType.InhereXYZ))
            {
                inheritedMatrix.Decompose(out _, out var inheritedRotation, out var inheritedTranslation);
                inheritedMatrix = Matrix.Identity;

                if (bone.BoneFlags.HasFlag(BoneType.InhereRotate))
                    inheritedMatrix = Matrix.RotationAxis(inheritedRotation.Axis, inheritedRotation.Angle * bone.InheraRate);

                if (bone.BoneFlags.HasFlag(BoneType.InhereXYZ))
                    inheritedMatrix = Matrix.Translation(inheritedTranslation);
                else
                    inheritedMatrix *= Matrix.Translation(inheritBaseBone.InitialPosition);
            }
            
            matrix = ConvertToLocal(matrix, inheritedMatrix, inheritBaseBone.InitialPosition);
        }

        matrix.Decompose(out _, out var rotation, out var translation);

        if (bone.BoneFlags.HasFlag(BoneType.Rotate))
            newMotion.Rotation = rotation;

        if (bone.BoneFlags.HasFlag(BoneType.XYZ))
            newMotion.Move = translation;

        // 計算誤差で未登録ボーンを増やしたくないので、誤差以内は無視する
        if (State.HasDifference(initialState.Motion, newMotion))
            new State(newMotion, true).Apply(bone.Layers[0]);
        else
            initialState.Apply(bone.Layers[0]);
    }

    static Matrix ConvertToLocal(Matrix matrix, Matrix parentTransform, Vector3 initialParentPosition) =>
        matrix
        * Matrix.Invert(parentTransform)
        * Matrix.Translation(initialParentPosition);

    public void SaveTransform(ModelBinding binding) =>
        PreviewTransform(binding);

    public void ResetPreview() =>
        initialState.Apply(bone.Layers[0]);

    record Target(BoneBinding Bone, BoneBinding? Parent, BoneBinding? InheritBase, BoneBinding? InheritBaseParent);
    
    record State(MotionData Motion, bool Selected)
    {
        public State(MotionLayer layer)
            : this(layer.CurrentLocalMotion, layer.Selected)
        {
        }

        public static bool HasDifference(MotionData a, MotionData b)
        {
            const float precision = 0.000001f;

            return Vector3.Distance(a.Move, b.Move) > precision
                || 1 - Quaternion.Dot(a.Rotation, b.Rotation) > precision;
        }

        public void Apply(MotionLayer layer)
        {
            // 計算誤差で未登録ボーンを増やしたくないので、誤差以内は無視する
            if (HasDifference(layer.CurrentLocalMotion, Motion))
                layer.CurrentLocalMotion = Motion;

            layer.Selected = Selected;
        }
    }
}
