using DxMath;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

public class MmdBone
{
	readonly MmdImport import;

	public MmdModel Model { get; }

	public int Index { get; }

	public string Name => import.ExpGetPmdBoneName(Model.Index, Index);

	public Matrix Transform => import.ExpGetPmdBoneWorldMat(Model.Index, Index);

	public MmdBone(MmdImport import, MmdModel model, int index)
	{
		this.import = import;
		Model = model;
		Index = index;
	}

	public override int GetHashCode() =>
		Model.GetHashCode() ^ typeof(MmdBone).GetHashCode() ^ Index;
}