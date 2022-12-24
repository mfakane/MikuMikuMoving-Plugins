using System.Runtime.InteropServices;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

[StructLayout(LayoutKind.Sequential)]
struct MmdMatrix
{
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public readonly float[] Value;
}