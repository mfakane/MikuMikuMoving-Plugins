using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin;

static class MmdDrop
{
	[SuppressUnmanagedCodeSecurity, DllImport("user32", SetLastError = true)]
	static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
	
	const uint WM_DROPFILES = 0x233;

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

	/// <summary>
	/// 指定されたウィンドウにファイルをドロップします。
	/// </summary>
	/// <param name="hWnd">ドロップ先のウィンドウ ハンドル。</param>
	/// <param name="fileName">ドロップするファイル。</param>
	public static void DropFile(IntPtr hWnd, string fileName)
	{
		DropFile(hWnd, new[] { fileName });
	}

	static void DropFile(IntPtr hWnd, IList<string> fileNames)
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

		if (!PostMessage(hWnd, WM_DROPFILES, hGlobal, IntPtr.Zero))
		{
			// 成功時はドロップ先が自動的に解放するはずなので失敗時のみ自前で解放する
			Marshal.FreeHGlobal(hGlobal);
		}
	}
}
