using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Interop;

/// <summary>
/// WM_DROPFILES ウィンドウ メッセージを利用してドラッグ アンド ドロップをエミュレートします。
/// </summary>
class WmDropFiles : IDragDropHelper
{
	[SuppressUnmanagedCodeSecurity, DllImport("user32", SetLastError = true)]
	static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
	
	const uint WM_DROPFILES = 0x233;

	public void DoDragDrop(IntPtr targetWindowHandle, params string[] fileNames)
	{
		var names = Encoding.Unicode.GetBytes(string.Join("\0", fileNames.ToArray()) + "\0\0");
		var dropFilesSize = Marshal.SizeOf(typeof(DropFiles));
		var hGlobal = Marshal.AllocHGlobal(dropFilesSize + names.Length);

		var dropFiles = new DropFiles
		{
			pFiles = (uint)dropFilesSize,
			x = 0,
			y = 0,
			fNC = false,
			fWide = true,
		};

		Marshal.StructureToPtr(dropFiles, hGlobal, true);
		Marshal.Copy(names, 0, hGlobal + dropFilesSize, names.Length);

		if (!PostMessage(targetWindowHandle, WM_DROPFILES, hGlobal, IntPtr.Zero))
		{
			// 成功時はドロップ先が自動的に解放するはずなので失敗時のみ自前で解放する
			Marshal.FreeHGlobal(hGlobal);
		}
	}
	
	[StructLayout(LayoutKind.Sequential)]
	struct DropFiles
	{
		public uint pFiles;
		public int x;
		public int y;
		[MarshalAs(UnmanagedType.Bool)]
		public bool fNC;
		[MarshalAs(UnmanagedType.Bool)]
		public bool fWide;
	}
}
