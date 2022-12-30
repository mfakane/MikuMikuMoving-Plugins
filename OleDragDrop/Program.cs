using System;
using System.IO;
using Linearstar.MikuMikuMoving.OleDragDrop;

if (args.Length < 2)
{
    Console.WriteLine(
        $"""
        Usage: {Path.GetFileName(Environment.GetCommandLineArgs()[0])} <target window handle> <file to drop>
        """);
    return;
}

var targetWindowHandleString = args[0];
var targetWindowHandle = new IntPtr(targetWindowHandleString.StartsWith("0x")
    ? long.Parse(targetWindowHandleString.Substring(2), System.Globalization.NumberStyles.HexNumber)
    : long.Parse(targetWindowHandleString));
var fileToDrop = args[1];

using var oleDragDropProxy = new OleDragDropProxy();

oleDragDropProxy.DoDragDrop(targetWindowHandle, fileToDrop);
