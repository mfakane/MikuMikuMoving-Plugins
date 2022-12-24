using System;
using System.IO;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin;

public class TempFile : IDisposable
{
    string? fileName;

    public string FileName => 
        fileName ?? throw new ObjectDisposedException(nameof(TempFile));

    public TempFile()
        : this(Path.GetTempFileName())
    {
    }
    
    public TempFile(string fileName) => 
        this.fileName = Path.IsPathRooted(fileName) ? fileName : Path.Combine(Path.GetTempPath(), fileName);

    public void Dispose() => Dispose(true);
    
    void Dispose(bool disposing)
    {
        if (disposing) GC.SuppressFinalize(this);

        if (fileName == null) return;

        try
        {
            File.Delete(fileName);
            fileName = null;
        }
        catch
        {
            // 万が一削除できなくても気にしない
        }
    }

    ~TempFile() => Dispose(false);
}