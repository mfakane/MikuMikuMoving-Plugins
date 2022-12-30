using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Interop;

/// <summary>
/// DLL インジェクションを利用して対象プロセス内で OLE ドラッグ アンド ドロップをエミュレートします。
/// </summary>
class OleDragDrop : IDragDropHelper
{
    readonly Assembly assembly;

    [SuppressUnmanagedCodeSecurity, DllImport("user32")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWow64Process(IntPtr process, out bool wow64Process);
    
    public OleDragDrop()
    {
        assembly = Assembly.GetExecutingAssembly();
    }
    
    public void DoDragDrop(IntPtr targetWindowHandle, params string[] fileNames)
    {
        GetWindowThreadProcessId(targetWindowHandle, out var processId);
        
        var remoteProcess = Process.GetProcessById((int)processId);
        IsWow64Process(remoteProcess.Handle, out var isWow64Process);
        
        var is64Bit = !isWow64Process && Environment.Is64BitOperatingSystem;
        var resourceSuffix = is64Bit ? "64" : "";
        using var oleDragDropExe = new TempFile($"OleDragDrop{resourceSuffix}.exe");
    
        using (var rs = assembly.GetManifestResourceStream($"OleDragDrop{resourceSuffix}")!)
        using (var fs = File.OpenWrite(oleDragDropExe.FileName))
            rs.CopyTo(fs);

        using var oleDragDropProcess = Process.Start(new ProcessStartInfo
        {
            FileName = oleDragDropExe.FileName,
            Arguments = $"\"{targetWindowHandle.ToInt32()}\" \"{fileNames[0]}\"",
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
            UseShellExecute = false,
        })!;
        
        oleDragDropProcess.WaitForExit();
    }
}