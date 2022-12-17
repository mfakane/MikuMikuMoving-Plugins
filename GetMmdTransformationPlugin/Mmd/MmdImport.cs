using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

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

			ThreadPool.QueueUserWorkItem(_ => InvokeRemote(process, this.Handle));

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

		static bool IsWow64Process(IntPtr hProcess)
		{
			var proc = GetProcAddress(GetModuleHandle("kernel32"), "IsWow64Process");

			if (proc == IntPtr.Zero)
				return true;

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

		static void InvokeRemote(Process process, IntPtr parameter)
		{
			var remoteProxy = Path.GetTempFileName();
			var remoteContainer = Path.GetTempFileName();
			var isPlatform64 = !IsWow64Process(process.Handle);
			var assembly = Assembly.GetExecutingAssembly();
			var containerStream =
				assembly.GetManifestResourceStream(isPlatform64
					? "GetMmdTransformationContainer64"
					: "GetMmdTransformationContainer")!;
			var proxyStream =
				assembly.GetManifestResourceStream(isPlatform64
					? "GetMmdTransformationProxy64"
					: "GetMmdTransformationProxy")!;

			using (var fs = File.OpenWrite(remoteContainer))
				containerStream.CopyTo(fs);
			
			using (var fs = File.OpenWrite(remoteProxy))
				proxyStream.CopyTo(fs);

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
