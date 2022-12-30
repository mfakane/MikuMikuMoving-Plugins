using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Linearstar.MikuMikuMoving.Framework.Interop;

class RemoteThread : IDisposable
{
    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);
    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern uint CloseHandle(IntPtr hHandle);
    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern bool GetExitCodeThread(IntPtr hThread, out IntPtr lpExitCode);
    
    readonly IntPtr processHandle;
    readonly IntPtr remoteProcAddress;
    readonly IntPtr parameter;
    IntPtr? remoteThreadHandle;
    
    public RemoteThread(Process process, IntPtr remoteProcAddress, IntPtr parameter)
    {
        processHandle = process.Handle;
        this.remoteProcAddress = remoteProcAddress;
        this.parameter = parameter;
    }
    
    public static void Run(Process process, IntPtr remoteProcAddress, IntPtr remoteParameter)
    {
        using var rt = new RemoteThread(process, remoteProcAddress, remoteParameter);
        
        rt.Run();
        rt.WaitForExit();
    }

    public void Run()
    {
        remoteThreadHandle = CreateRemoteThread(
            processHandle,
            IntPtr.Zero,
            0,
            remoteProcAddress,
            parameter,
            0,
            out _);
        WaitForSingleObject(remoteThreadHandle.Value, uint.MaxValue);
    }

    public void WaitForExit()
    {
        if (!remoteThreadHandle.HasValue)
            throw new InvalidOperationException();

        GetExitCodeThread(remoteThreadHandle.Value, out _);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        if (!remoteThreadHandle.HasValue) return;
        
        CloseHandle(remoteThreadHandle.Value);
    }
    
    ~RemoteThread() => Dispose();
}
