using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Properties;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	partial class MmdImport : IDisposable
	{
		const string namedPipePrefix = "LS_MMM_GMTP_MI_";
		const string remoteEntryPoint = "RemoteEntryPoint";

		readonly NamedPipeServerStream pipe;
		readonly MmdPipeProtocol protocol;

		public Process Process
		{
			get;
			private set;
		}

		public IntPtr Handle
		{
			get;
			private set;
		}

		public bool IsPlatform64
		{
			get
			{
				return !IsWow64Process(this.Process.Handle);
			}
		}

		public MmdImport(Process process)
		{
			var r = new Random();

			this.Process = process;
			this.Handle = new IntPtr(process.Id + r.Next());

			pipe = new NamedPipeServerStream(namedPipePrefix + this.Handle.ToString());

			ThreadPool.QueueUserWorkItem(_ =>
			{
				if (this.IsPlatform64)
					InvokeRemote64(process, this.Handle);
				else
					InvokeRemote(process, this.Handle);
			});

			pipe.WaitForConnection();
			protocol = new MmdPipeProtocol(pipe);
		}

		public IList<MmdModel> Models
		{
			get
			{
				return Enumerable.Range(0, this.ExpGetPmdNum())
								 .Select(_ => new MmdModel(this, _))
								 .ToArray();
			}
		}

		#region Interop

		static T GetDelegate<T>(IntPtr hModule, string entryPoint)
		{
			var proc = GetProcAddress(hModule, entryPoint);

			if (proc == IntPtr.Zero)
				throw new EntryPointNotFoundException(entryPoint);

			return (T)(object)Marshal.GetDelegateForFunctionPointer(proc, typeof(T));
		}

		bool IsWow64Process(IntPtr hProcess)
		{
			var proc = GetProcAddress(GetModuleHandle("kernel32"), "IsWow64Process");

			if (proc == IntPtr.Zero)
				return false;

			bool rt;

			return IsWow64Process(hProcess, out rt) && rt;
		}

		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool IsWow64Process(IntPtr process, out bool wow64Process);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr GetModuleHandle(string lpModuleName);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern uint CloseHandle(IntPtr hHandle);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern bool GetExitCodeThread(IntPtr hThread, out IntPtr lpExitCode);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern IntPtr LoadLibrary(string lpFileName);
		[SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
		static extern bool FreeLibrary(IntPtr hModule);

		const uint MEM_COMMIT = 0x1000;
		const uint MEM_RELEASE = 0x8000;
		const uint PAGE_READWRITE = 0x4;

		static void InvokeRemote64(Process process, IntPtr parameter)
		{
			var remoteProxy = Path.GetTempFileName();
			var remoteContainer = Path.GetTempFileName();

			File.WriteAllBytes(remoteContainer, Resources.GetMmdTransformationContainer64);
			File.WriteAllBytes(remoteProxy, Resources.GetMmdTransformationProxy64);

			using (var p = Process.Start(new ProcessStartInfo
			{
				FileName = remoteContainer,
				Arguments = string.Join(" ", new[]
				{
					remoteProxy,
					process.Id.ToString(),
					parameter.ToString(),
				}.Select(_ => "\"" + _ + "\"")),
				CreateNoWindow = true,
				WindowStyle = ProcessWindowStyle.Hidden,
				UseShellExecute = false,
			}))
				p.WaitForExit();

			try
			{
				File.Delete(remoteProxy);
			}
			catch
			{
			}

			try
			{
				File.Delete(remoteContainer);
			}
			catch
			{
			}
		}

		static void InvokeRemote(Process process, IntPtr parameter)
		{
			var remoteProxy = Path.GetTempFileName();

			File.WriteAllBytes(remoteProxy, Resources.GetMmdTransformationProxy);

			var localModule = LoadLibrary(remoteProxy);
			var hProcess = process.Handle;
			var kernelModule = GetModuleHandle("kernel32");
			IntPtr hModule;

			// check if the assembly is already loaded.
			{
				var moduleName = Encoding.Unicode.GetBytes(Path.GetFileName(remoteProxy)).Concat(new[] { (byte)0 }).ToArray();
				var alloc = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)moduleName.Length, MEM_COMMIT, PAGE_READWRITE);
				uint writtenBytes;

				WriteProcessMemory(hProcess, alloc, moduleName, (uint)moduleName.Length, out writtenBytes);

				{
					uint lpThreadId;
					var getModuleHandle = GetProcAddress(kernelModule, "GetModuleHandleW");
					var hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, getModuleHandle, alloc, 0, out lpThreadId);

					WaitForSingleObject(hThread, unchecked((uint)-1));
					GetExitCodeThread(hThread, out hModule);
					CloseHandle(hThread);
				}

				VirtualFreeEx(hProcess, alloc, writtenBytes, MEM_RELEASE);
			}

			// if not loaded, use LoadLibraryW.
			if (hModule == IntPtr.Zero)
				hModule = LoadLibraryRemote(remoteProxy, hProcess, kernelModule);

			// the main action.
			{
				uint lpThreadId;
				var localEntryPointAddr = GetProcAddress(localModule, remoteEntryPoint);

				// while hModule is just an address of the module, you can get the remote proc address by doing this :)
				var remoteEntryPointAddr = hModule + (localEntryPointAddr.ToInt32() - localModule.ToInt32());
				var hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, remoteEntryPointAddr, parameter, 0, out lpThreadId);

				WaitForSingleObject(hThread, unchecked((uint)-1));
				CloseHandle(hThread);
			}

			// if finished, unload the library.
			FreeLibraryRemote(hProcess, kernelModule, hModule);
			FreeLibrary(localModule);

			try
			{
				File.Delete(remoteProxy);
			}
			catch
			{
			}
		}

		static IntPtr LoadLibraryRemote(string location, IntPtr hProcess, IntPtr kernelModule)
		{
			IntPtr hModule;
			var path = Encoding.Unicode.GetBytes(location).Concat(new[] { (byte)0 }).ToArray();
			var alloc = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)path.Length, MEM_COMMIT, PAGE_READWRITE);
			uint writtenBytes;

			WriteProcessMemory(hProcess, alloc, path, (uint)path.Length, out writtenBytes);

			{
				uint lpThreadId;
				var loadLibrary = GetProcAddress(kernelModule, "LoadLibraryW");
				var hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibrary, alloc, 0, out lpThreadId);

				WaitForSingleObject(hThread, unchecked((uint)-1));
				GetExitCodeThread(hThread, out hModule);
				CloseHandle(hThread);
			}

			VirtualFreeEx(hProcess, alloc, writtenBytes, MEM_RELEASE);

			return hModule;
		}

		static void FreeLibraryRemote(IntPtr hProcess, IntPtr kernelModule, IntPtr hModule)
		{
			uint lpThreadId;
			var freeLibrary = GetProcAddress(kernelModule, "FreeLibrary");
			var hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, freeLibrary, hModule, 0, out lpThreadId);

			WaitForSingleObject(hThread, unchecked((uint)-1));
			CloseHandle(hThread);
		}

		T InvokeRemote<T>(string entryPoint, int[] parameters)
		{
			return protocol.InvokeRemote<T>(entryPoint, parameters);
		}

		#endregion

		public void Dispose()
		{
			protocol.Dispose();
			pipe.Dispose();
			this.Process.Dispose();
			GC.SuppressFinalize(this);
		}

		~MmdImport()
		{
			Dispose();
		}
	}
}
