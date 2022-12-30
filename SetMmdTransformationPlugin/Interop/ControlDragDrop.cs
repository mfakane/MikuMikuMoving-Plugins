using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Interop;

/// <summary>
/// Windows Forms の組み込みの機能を利用し、マウス カーソル座標を操作してドラッグ アンド ドロップを実行します。
/// </summary>
class ControlDragDrop : IDragDropHelper
{
    [DllImport("user32")]
    static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    
    [DllImport("user32")]
    static extern bool SetForegroundWindow(IntPtr hWnd);
    
    public void DoDragDrop(IntPtr targetWindowHandle, params string[] fileNames)
    {
        using var control = new Control();
        
        control.QueryContinueDrag += (sender, e) =>
        {
            if (e.EscapePressed)
                e.Action = DragAction.Cancel;
        };
        control.GiveFeedback += (sender, e) => e.UseDefaultCursors = true;

        var savedCursorPosition = Cursor.Position;
        var dataObject = new DataObject();
        var collection = new StringCollection();
        collection.AddRange(fileNames);
        dataObject.SetFileDropList(collection);

        GetWindowRect(targetWindowHandle, out var rect);
        SetForegroundWindow(targetWindowHandle);
        Cursor.Hide();
        Cursor.Position = new Point((rect.Left + rect.Right) / 2, rect.Top + 32);
        
        try
        {
            control.DoDragDrop(dataObject, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);

            for (var i = 0; i < 50; i++)
            {
                DoEvents();
            }
        }
        finally
        {
            Cursor.Position = savedCursorPosition;
            Cursor.Show();
        }
    }

    void DoEvents()
    {
        Application.DoEvents();
        Thread.Sleep(10);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public readonly int Left;
        public readonly int Top;
        public readonly int Right;
        public readonly int Bottom;
    }
}