using System;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Interop;

interface IDragDropHelper
{
    void DoDragDrop(IntPtr targetWindowHandle, params string[] fileNames);
}