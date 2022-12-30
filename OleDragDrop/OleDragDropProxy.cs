using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Linearstar.MikuMikuMoving.Framework.Interop;

namespace Linearstar.MikuMikuMoving.OleDragDrop;

class OleDragDropProxy : IDisposable
{
    readonly string proxyDllPath;
    
    public OleDragDropProxy()
    {
        proxyDllPath = Path.Combine(Path.GetTempPath(), $"OleDragDropProxy{Process.GetCurrentProcess().Id}.dll");
        
        var proxyDllStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OleDragDropProxy")!;
        using var proxyDllFile = File.Create(proxyDllPath);
        
        proxyDllStream.CopyTo(proxyDllFile);
    }   
    
    public unsafe void DoDragDrop(IntPtr targetWindowHandle, string fileName)
    {
        NativeMethods.GetWindowThreadProcessId(targetWindowHandle, out var targetProcessId);

        using var targetProcess = Process.GetProcessById((int)targetProcessId);
        using var remoteFileName = new RemoteAlloc(targetProcess, Encoding.Unicode.GetBytes(fileName + "\0"));
        var args = stackalloc byte[sizeof(PerformOleDragDropArgs)];

        *(PerformOleDragDropArgs*)args = new PerformOleDragDropArgs
        {
            hWnd = targetWindowHandle,
            lpFileName = remoteFileName.RemoteAddress,
        };
        
        using var remoteArgs = new RemoteAlloc(targetProcess, new IntPtr(args), (uint)sizeof(PerformOleDragDropArgs));
        using var library = new RemoteLibrary(targetProcess, proxyDllPath);

        library.InvokeEntryPoint("PerformOleDragDrop", remoteArgs.RemoteAddress);
    }

    public void Dispose() => Dispose(true);
    
    void Dispose(bool disposing)
    {
        if (disposing) GC.SuppressFinalize(this);

        try
        {
            File.Delete(proxyDllPath);
        }
        catch
        {
            // 万が一削除できなくても気にしない
        }
    }

    ~OleDragDropProxy() => Dispose(false);
    
    [StructLayout(LayoutKind.Sequential)]
    struct PerformOleDragDropArgs
    {
        public IntPtr hWnd;
        public IntPtr lpFileName;
    }
}