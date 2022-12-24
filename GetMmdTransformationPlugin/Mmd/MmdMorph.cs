namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

public class MmdMorph
{
	readonly MmdImport import;

	public MmdModel Model { get; }

	public int Index { get; }

	public string Name => import.ExpGetPmdMorphName(Model.Index, Index);

	public float Weight => import.ExpGetPmdMorphValue(Model.Index, Index);

	public MmdMorph(MmdImport import, MmdModel model, int index)
	{
		this.import = import;
		Model = model;
		Index = index;
	}

	public override int GetHashCode() =>
		Model.GetHashCode() ^ typeof(MmdMorph).GetHashCode() ^ Index;
}