using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Interop;

public class MmdDropTarget : IDisposable
{
    const string MikuMikuDanceProcessName = "MikuMikuDance";
    const string NexGiMaProcessName = "NexGiMa";
    const string PmxEditorProcessName = "PmxEditor";
    const string PmxEditor64ProcessName = "PmxEditor_x64";
    const string UntitledProject = "無題のプロジェクト";

    [SuppressUnmanagedCodeSecurity, DllImport("user32", SetLastError = true)]
    static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
    [SuppressUnmanagedCodeSecurity, DllImport("user32", SetLastError = true)]
    static extern int GetWindowText(IntPtr hWnd, [Out] char[] lpString, int nMaxCount);
    [SuppressUnmanagedCodeSecurity, DllImport("user32", SetLastError = true)]
    static extern int GetWindowTextLength(IntPtr hWnd);
    
    delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

    public Process BaseProcess { get; }
    public int Id { get; }
    public string ProcessName { get; }
    public MmdDropTargetKind Kind { get; }
    public string ProjectName { get; }

    public MmdDropTarget(Process process)
    {
        BaseProcess = process;
        Id = process.Id;
        ProcessName = process.ProcessName;
        Kind = GetKindFromProcessName(ProcessName);
        ProjectName = GetProjectName(process, Kind);
    }

    public static IReadOnlyList<MmdDropTarget> GetTargetProcesses() =>
        Process.GetProcessesByName(MikuMikuDanceProcessName)
            .Concat(Process.GetProcessesByName(NexGiMaProcessName))
            // TODO: PmxEditor はそのうちサポートしたい
            .Concat(Process.GetProcessesByName(PmxEditorProcessName))
            .Concat(Process.GetProcessesByName(PmxEditor64ProcessName))
            .Select(x => new MmdDropTarget(x))
            .ToArray();

    static MmdDropTargetKind GetKindFromProcessName(string processName) =>
        processName switch
        {
            MikuMikuDanceProcessName => MmdDropTargetKind.MikuMikuDance,
            NexGiMaProcessName => MmdDropTargetKind.NexGiMa,
            PmxEditorProcessName => MmdDropTargetKind.PmxEditor,
            PmxEditor64ProcessName => MmdDropTargetKind.PmxEditor64,
            _ => MmdDropTargetKind.Unknown,
        };
    
    static IReadOnlyList<IntPtr> GetThreadWindows(int threadId)
    {
        var ret = new List<IntPtr>();

        EnumThreadWindows(threadId, (hWnd, lParam) =>
        {
            ret.Add(hWnd);

            return true;
        }, IntPtr.Zero);

        return ret;
    }

    static string? GetWindowTitle(IntPtr hWnd)
    {
        var length = GetWindowTextLength(hWnd);
        if (length == 0) return null;

        var buffer = new char[length + 1];

        GetWindowText(hWnd, buffer, buffer.Length);

        return new string(buffer).TrimEnd('\0');
    }

    static string GetProjectName(Process process, MmdDropTargetKind kind)
    {
        var mainWindowTitle = process.MainWindowTitle;
        if (GetProjectNameFromTitle(mainWindowTitle) is { } mainWindowProjectName)
            return mainWindowProjectName;

        foreach (ProcessThread thread in process.Threads)
        foreach (var hWnd in GetThreadWindows(thread.Id))
        {
            var title = GetWindowTitle(hWnd);
            if (GetProjectNameFromTitle(title) is { } projectName)
                return projectName;
        }

        return UntitledProject;

        string? GetProjectNameFromTitle(string? title)
        {
            if (title == null) return null;

            switch (kind)
            {
                case MmdDropTargetKind.PmxEditor:
                case MmdDropTargetKind.PmxEditor64:
                {
                    if (string.IsNullOrEmpty(title) || !title!.StartsWith("Pmx編集 - "))
                        return null;

                    var separatorIndex = title.IndexOf(" - ", StringComparison.Ordinal);

                    return title.Substring(separatorIndex + 3);
                }
                default:
                {
                    if (string.IsNullOrEmpty(title) || !title!.Contains(" - [") || !title.Contains("]"))
                        return null;

                    var beginIndex = title.IndexOf(" [", StringComparison.Ordinal) + 2;
                    var endIndex = title.LastIndexOf(']');
                    var fileName = title.Substring(beginIndex, endIndex - beginIndex);

                    if (fileName.Contains(Path.DirectorySeparatorChar))
                        fileName = Path.GetFileName(fileName);

                    return fileName;
                }
            }
        }
    }

    IntPtr GetTargetWindowHandle()
    {
        if (IsTargetWindow(BaseProcess.MainWindowTitle))
            return BaseProcess.MainWindowHandle;
        
        foreach (ProcessThread thread in BaseProcess.Threads)
        foreach (var hWnd in GetThreadWindows(thread.Id))
        {
            var title = GetWindowTitle(hWnd);

            if (IsTargetWindow(title))
                return hWnd;
        }

        return BaseProcess.MainWindowHandle;
        
        bool IsTargetWindow(string? title) =>
            Kind switch
            {
                MmdDropTargetKind.PmxEditor => title == "TransformView",
                MmdDropTargetKind.PmxEditor64 => title == "TransformView",
                MmdDropTargetKind.MikuMikuDance => title?.StartsWith("MikuMikuDance") ?? false,
                MmdDropTargetKind.NexGiMa => title?.StartsWith("NexGiMa") ?? false,
                _ => false,
            };
    }

    public void DoDragDrop(string? vpdFileName, string? vmdFileName)
    {
        if (BaseProcess.HasExited || vmdFileName == null && vpdFileName == null) return;
       
        var targetWindowHandle = GetTargetWindowHandle();
        
        switch (Kind)
        {
            case MmdDropTargetKind.PmxEditor:
            case MmdDropTargetKind.PmxEditor64:
            {
                var dragDropHelper = new OleDragDrop();
                
                if (vmdFileName != null)
                    dragDropHelper.DoDragDrop(targetWindowHandle, vmdFileName);
                
                if (vpdFileName != null)
                    dragDropHelper.DoDragDrop(targetWindowHandle, vpdFileName);

                break;
            }
            default:
            {
                var dragDropHelper = new WmDropFiles();
                
                if (vmdFileName != null)
                    dragDropHelper.DoDragDrop(targetWindowHandle, vmdFileName);
                
                if (vpdFileName != null)
                    dragDropHelper.DoDragDrop(targetWindowHandle, vpdFileName);
         
                break;
            }
        }
    }

    public void Dispose() => 
        BaseProcess.Dispose();
}

public enum MmdDropTargetKind
{
    Unknown,
    MikuMikuDance,
    NexGiMa,
    PmxEditor,
    PmxEditor64,
}