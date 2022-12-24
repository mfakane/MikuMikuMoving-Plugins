using System.Collections.Generic;
using System.Linq;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

public class MmdModel
{
	readonly MmdImport import;
	IReadOnlyList<MmdBone>? bones;
	IReadOnlyList<MmdMorph>? morphs;

	public int Index { get; }

	public int Id => import.ExpGetPmdID(Index);

	public int Order => import.ExpGetPmdOrder(Index);

	public string FileName => import.ExpGetPmdFilename(Index);

	public IReadOnlyList<MmdBone> Bones =>
		bones ??= Enumerable.Range(0, import.ExpGetPmdBoneNum(Index))
			.Select(x => new MmdBone(import, this, x))
			.ToArray();

	public IReadOnlyList<MmdMorph> Morphs =>
		morphs ??= Enumerable.Range(0, import.ExpGetPmdMorphNum(Index))
			.Select(x => new MmdMorph(import, this, x))
			.ToArray();

	public MmdModel(MmdImport import, int index)
	{
		this.import = import;
		Index = index;
	}

	public override int GetHashCode() =>
		import.GetHashCode() ^ typeof(MmdModel).GetHashCode() ^ Index;
}