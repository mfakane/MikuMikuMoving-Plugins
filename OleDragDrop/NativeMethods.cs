using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Linearstar.MikuMikuMoving.OleDragDrop;

static class NativeMethods
{
    [SuppressUnmanagedCodeSecurity, DllImport("user32")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
}