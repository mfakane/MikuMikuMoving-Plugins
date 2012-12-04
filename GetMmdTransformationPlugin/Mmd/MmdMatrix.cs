using System.Runtime.InteropServices;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	struct MmdMatrix
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		float[] value;

		public float[] Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}
	}
}
