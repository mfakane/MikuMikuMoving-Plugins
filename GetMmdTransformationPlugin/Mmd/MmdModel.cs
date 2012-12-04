using System.Collections.Generic;
using System.Linq;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	class MmdModel
	{
		readonly MmdImport import;

		public int Index
		{
			get;
			private set;
		}

		public int Id
		{
			get
			{
				return import.ExpGetPmdID(this.Index);
			}
		}

		public int Order
		{
			get
			{
				return import.ExpGetPmdOrder(this.Index);
			}
		}

		public string FileName
		{
			get
			{
				return import.ExpGetPmdFilename(this.Index);
			}
		}

		public IEnumerable<MmdBone> Bones
		{
			get
			{
				return Enumerable.Range(0, import.ExpGetPmdBoneNum(this.Index))
								 .Select(_ => new MmdBone(import, this, _));
			}
		}

		public IEnumerable<MmdMorph> Morphs
		{
			get
			{
				return Enumerable.Range(0, import.ExpGetPmdMorphNum(this.Index))
								 .Select(_ => new MmdMorph(import, this, _));
			}
		}

		public MmdModel(MmdImport import, int index)
		{
			this.import = import;
			this.Index = index;
		}
	}
}
