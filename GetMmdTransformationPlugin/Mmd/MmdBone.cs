using DxMath;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	class MmdBone
	{
		readonly MmdImport import;

		public MmdModel Model
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public string Name
		{
			get
			{
				return import.ExpGetPmdBoneName(this.Model.Index, this.Index);
			}
		}

		public Matrix Transform
		{
			get
			{
				return import.ExpGetPmdBoneWorldMat(this.Model.Index, this.Index);
			}
		}

		public MmdBone(MmdImport import, MmdModel model, int index)
		{
			this.import = import;
			this.Model = model;
			this.Index = index;
		}
	}
}
