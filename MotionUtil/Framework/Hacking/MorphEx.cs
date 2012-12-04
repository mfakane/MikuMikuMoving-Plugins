using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	static class MorphEx
	{
		public static object GetRealMorph(this Morph morph)
		{
			return morph.Member("Controller").Member("ObjectList").Member("Item", morph.Member("ModelID")).Member("modelfile").Member("Morphs").Member("GetValue", new[] { new[] { morph.MorphID } });
		}
	}
}
