using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Linearstar.MikuMikuMoving.Framework.Interop;

class RemoteAlloc : IDisposable
{
    const uint MEM_COMMIT = 0x1000;
    const uint MEM_RELEASE = 0x8000;
    const uint PAGE_READWRITE = 0x4;

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType,
        uint flProtect);

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern IntPtr VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
        out uint lpNumberOfBytesWritten);

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize,
        out uint lpNumberOfBytesWritten);

    readonly IntPtr processHandle;
    readonly uint writtenBytes;

    public IntPtr RemoteAddress { get; }

    public RemoteAlloc(Process process, byte[] buffer)
    {
        processHandle = process.Handle;

        var size = (uint)buffer.Length;

        RemoteAddress = VirtualAllocEx(
            processHandle,
            IntPtr.Zero,
            size,
            MEM_COMMIT,
            PAGE_READWRITE);
        WriteProcessMemory(
            processHandle,
            RemoteAddress,
            buffer,
            size,
            out writtenBytes);
    }

    public RemoteAlloc(Process process, IntPtr ptr, uint size)
    {
        processHandle = process.Handle;

        RemoteAddress = VirtualAllocEx(
            processHandle,
            IntPtr.Zero,
            size,
            MEM_COMMIT,
            PAGE_READWRITE);
        WriteProcessMemory(
            processHandle,
            RemoteAddress,
            ptr,
            size,
            out writtenBytes);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        VirtualFreeEx(processHandle, RemoteAddress, writtenBytes, MEM_RELEASE);
    }

    ~RemoteAlloc() => Dispose();
}