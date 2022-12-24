using System;
using System.Collections.Generic;
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
	[SuppressUnmanagedCodeSecurity, DllImport("user32")]
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
	/// <param name="file">ドロップするファイル。</param>
	public static void DropFile(IntPtr hWnd, MmdDropFile file)
	{
		DropFile(hWnd, new[] { file });
	}

	static void DropFile(IntPtr hWnd, IList<MmdDropFile> files)
	{
		var names = Encoding.Unicode.GetBytes(string.Join("\0", files.Select(x => x.FullName).ToArray()) + "\0\0");
		var pipes = files.Where(x => x.IsPipe).Select(x => new
		{
			Pipe = new NamedPipeServerStream(x.FileName, PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous),
			File = x,
		}).ToArray();
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
		Marshal.Copy(names, 0, new(hGlobal.ToInt32() + dropFiles.pFiles), names.Length);

		PostMessage(hWnd, WM_DROPFILES, hGlobal, IntPtr.Zero);

		// NOTE: the drop target will automatically release it, so doing this would cause a Win32 exception.
		// Marshal.FreeHGlobal(hGlobal);

		foreach (var i in pipes)
			using (var handle = new ManualResetEvent(false))
			{
				var success = false;

				i.Pipe.BeginWaitForConnection(ar =>
				{
					try
					{
						i.Pipe.EndWaitForConnection(ar);
						success = true;

						try
						{
							i.File.Stream.CopyTo(i.Pipe, (int)i.File.Stream.Length);
							i.Pipe.WaitForPipeDrain();
						}
						catch (IOException)
						{
						}

						i.Pipe.Dispose();
						i.File.Stream.Dispose();
						handle.Set();
					}
					catch (ObjectDisposedException)
					{
					}
				}, null);

				if (i.File.Timeout != -1)
					ThreadPool.QueueUserWorkItem(_ =>
					{
						Thread.Sleep(i.File.Timeout);

						if (!success && !i.Pipe.IsConnected)
						{
							i.Pipe.Dispose();
							i.File.Stream.Dispose();
							handle.Set();
						}
					});

				handle.WaitOne();
			}
	}
}

/// <summary>
/// 対象のアプリケーションにドロップするファイルのデータを表します。
/// </summary>
class MmdDropFile
{
	/// <summary>
	/// ファイル名を取得します。
	/// </summary>
	public string FileName { get; }

	/// <summary>
	/// ファイル名を取得します。これはファイルのフルパスまたは名前付きパイプの名前です。
	/// </summary>
	public string FullName => IsPipe ? @"\\.\pipe\" + FileName : FileName;

	/// <summary>
	/// 基になるストリームを取得します。
	/// </summary>
	public Stream? Stream { get; }

	/// <summary>
	/// このファイルが名前付きパイプを使用して転送されるかどうかを取得します。
	/// </summary>
	public bool IsPipe => Stream != null;

	/// <summary>
	/// 転送のタイムアウトを取得または設定します。
	/// </summary>
	public int Timeout
	{
		get;
		set;
	}

	/// <summary>
	/// ファイル名および基になるストリームを指定して MmdDropFile の新しいインスタンスを初期化します。
	/// </summary>
	/// <param name="fileName">ドロップするファイルの任意の名前。</param>
	/// <param name="stream">ドロップするデータを提供するストリーム。</param>
	public MmdDropFile(string fileName, Stream? stream = null)
	{
		Timeout = -1;
		FileName = stream == null ? fileName : Path.GetFileName(fileName);
		Stream = stream;
	}
}