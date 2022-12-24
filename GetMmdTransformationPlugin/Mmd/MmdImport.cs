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

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

public partial class MmdImport : IDisposable
{
    const string NamedPipePrefix = "LS_MMM_GMTP_MI_";

    readonly NamedPipeServerStream pipe;
    readonly MmdPipeProtocol protocol;

    public Process Process { get; }

    public IntPtr Handle { get; }

    public bool IsPlatform64 => !IsWow64Process(Process.Handle);

    public MmdImport(Process process)
    {
        var r = new Random();

        Process = process;
        Handle = new(process.Id + r.Next());

        pipe = new(NamedPipePrefix + Handle.ToInt64());

        ThreadPool.QueueUserWorkItem(_ => InvokeRemote(process, Handle));

        pipe.WaitForConnection();
        protocol = new(pipe);
    }

    public IList<MmdModel> Models =>
        Enumerable.Range(0, ExpGetPmdNum())
            .Select(x => new MmdModel(this, x))
            .ToArray();

    #region Interop

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
        Process.Dispose();
        GC.SuppressFinalize(this);
    }

    ~MmdImport()
    {
        Dispose();
    }

    public override int GetHashCode() =>
        typeof(MmdImport).GetHashCode() ^ Handle.GetHashCode();
}