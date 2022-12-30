using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;

namespace Linearstar.MikuMikuMoving.Framework;

public static class ProcessExtensions
{
    [SuppressUnmanagedCodeSecurity, DllImport("kernel32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsWow64Process(IntPtr process, out bool wow64Process);
    
    public static bool IsWow64Process(this Process process)
    {
        IsWow64Process(process.Handle, out var result);

        return result;
    }
}