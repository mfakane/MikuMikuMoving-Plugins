using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationContainer
{
	class Program
	{
		const string remoteEntryPoint = "RemoteEntryPoint";

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

		static void Main(string[] args)
		{
			var remoteProxy = args[0];
			var pid = int.Parse(args[1]);
			var parameter = (IntPtr)int.Parse(args[2]);
			var moduleFileName = Path.GetFileName(remoteProxy);
			IntPtr hProcess;
			IntPtr hModule;

			using (var process = Process.GetProcessById(pid))
			{
				hProcess = process.Handle;
				hModule = process.Modules.Cast<ProcessModule>().Where(_ => _.ModuleName == moduleFileName).Select(_ => _.BaseAddress).FirstOrDefault();

				var localModule = LoadLibrary(remoteProxy);
				var kernelModule = GetModuleHandle("kernel32");

				// if not loaded, use LoadLibraryW.
				if (hModule == IntPtr.Zero)
				{
					LoadLibraryRemote(remoteProxy, process, kernelModule);

					using (var process2 = Process.GetProcessById(pid))
						hModule = process2.Modules.Cast<ProcessModule>().Where(_ => _.ModuleName == moduleFileName).Select(_ => _.BaseAddress).FirstOrDefault();
				}

				// the main action.
				{
					uint lpThreadId;
					var localEntryPointAddr = GetProcAddress(localModule, remoteEntryPoint);

					// while hModule is just an address of the module, you can get the remote proc address by doing this :)
					var remoteEntryPointAddr = (IntPtr)(hModule.ToInt64() + (localEntryPointAddr.ToInt64() - localModule.ToInt64()));
					var hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, remoteEntryPointAddr, parameter, 0, out lpThreadId);

					WaitForSingleObject(hThread, unchecked((uint)-1));
					CloseHandle(hThread);
				}

				// if finished, unload the library.
				FreeLibraryRemote(hProcess, kernelModule, hModule);
				FreeLibrary(localModule);
			}
		}

		static void LoadLibraryRemote(string location, Process process, IntPtr kernelModule)
		{
			var hProcess = process.Handle;
			var path = Encoding.Unicode.GetBytes(location).Concat(new[] { (byte)0 }).ToArray();
			var alloc = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)path.Length, MEM_COMMIT, PAGE_READWRITE);
			uint writtenBytes;

			WriteProcessMemory(hProcess, alloc, path, (uint)path.Length, out writtenBytes);
			alloc.ToString();

			{
				uint lpThreadId;
				var loadLibrary = GetProcAddress(kernelModule, "LoadLibraryW");
				var hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibrary, alloc, 0, out lpThreadId);
				IntPtr returnValue;

				WaitForSingleObject(hThread, unchecked((uint)-1));
				GetExitCodeThread(hThread, out returnValue);
				CloseHandle(hThread);
			}

			VirtualFreeEx(hProcess, alloc, writtenBytes, MEM_RELEASE);
		}

		static void FreeLibraryRemote(IntPtr hProcess, IntPtr kernelModule, IntPtr hModule)
		{
			uint lpThreadId;
			var freeLibrary = GetProcAddress(kernelModule, "FreeLibrary");
			var hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, freeLibrary, hModule, 0, out lpThreadId);

			WaitForSingleObject(hThread, unchecked((uint)-1));
			CloseHandle(hThread);
		}
	}
}
