namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	class MmdMorph
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
				return import.ExpGetPmdMorphName(this.Model.Index, this.Index);
			}
		}

		public float Weight
		{
			get
			{
				return import.ExpGetPmdMorphValue(this.Model.Index, this.Index);
			}
		}

		public MmdMorph(MmdImport import, MmdModel model, int index)
		{
			this.import = import;
			this.Model = model;
			this.Index = index;
		}
	}
}
