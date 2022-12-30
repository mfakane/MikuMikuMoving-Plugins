using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Linearstar.MikuMikuMoving.Framework.Interop;

class RemoteLibrary : IDisposable
{
    readonly Process process;
    readonly string fileName;
    readonly IntPtr remoteModuleHandle;
    readonly Dictionary<string, IntPtr> remoteProcAddressCache;

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32", SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32", SetLastError = true)]
    static extern IntPtr LoadLibrary(string lpFileName);

    [SuppressUnmanagedCodeSecurity, DllImport("kernel32", SetLastError = true)]
    static extern bool FreeLibrary(IntPtr hModule);
    
    public RemoteLibrary(Process process, string fileName)
    {
        this.process = process;
        this.fileName = fileName;

        // kernel32 は相手でも同じアドレスにロードされるのでそのまま使ってよい
        remoteProcAddressCache = LoadAndGetProcAddress("kernel32", new[]
            {
                "LoadLibraryW",
                "FreeLibrary",
            })
            .ToDictionary(x => x.Key, x => x.Value.procAddress);

        var fileNameInBytes = Encoding.Unicode.GetBytes(fileName + "\0");
        using var fileNameInRemote = new RemoteAlloc(process, fileNameInBytes);

        RemoteThread.Run(process, remoteProcAddressCache["LoadLibraryW"], fileNameInRemote.RemoteAddress);
        
        process.Refresh();
        remoteModuleHandle = process.Modules
            .Cast<ProcessModule>()
            .First(x => x.FileName == fileName)
            .BaseAddress;
    }

    public void InvokeEntryPoint(string procName, IntPtr remoteParameter)
    {
        var entryPoint = LoadAndGetProcAddress(fileName, procName);
        
        RemoteThread.Run(process, entryPoint.GetProcAddressForModule(remoteModuleHandle), remoteParameter);
    }
    
    /// <summary>
    /// 指定されたモジュールをロードし、そのモジュール内の指定された関数のアドレスを取得します。
    /// </summary>
    /// <param name="moduleName">ロードするモジュール名またはファイル名。</param>
    /// <param name="procNames">アドレスを取得する関数名。</param>
    /// <returns>取得したモジュールのベース アドレスおよび関数のアドレス。</returns>
    static IReadOnlyDictionary<string, ModuleProcAddress> LoadAndGetProcAddress(string moduleName, IEnumerable<string> procNames)
    {
        var hModule = LoadLibrary(moduleName);
        if (hModule == IntPtr.Zero) throw new Win32Exception();

        try
        {
            var addressesByName = new Dictionary<string, ModuleProcAddress>();

            foreach (var procName in procNames)
            {
                var procAddress = GetProcAddress(hModule, procName);
                if (procAddress == IntPtr.Zero) continue;

                addressesByName[procName] = new ModuleProcAddress(hModule, procAddress);
            }

            return addressesByName;
        }
        finally
        {
            FreeLibrary(hModule);
        }
    }
    
    static ModuleProcAddress LoadAndGetProcAddress(string moduleName, string procName)
    {
        var addressesByName = LoadAndGetProcAddress(moduleName, new[] { procName });
        return addressesByName[procName];
    }
    
    public void Dispose()
    {
        RemoteThread.Run(process, remoteProcAddressCache["FreeLibrary"], remoteModuleHandle);
    }

    readonly struct ModuleProcAddress
    {
        public readonly IntPtr hModule;
        public readonly IntPtr procAddress;

        public ModuleProcAddress(IntPtr hModule, IntPtr procAddress)
        {
            this.hModule = hModule;
            this.procAddress = procAddress;
        }
        
        public IntPtr GetProcAddressForModule(IntPtr moduleBaseAddress) =>
            // HMODULE は PE イメージの配置されているアドレスなので、
            // Proc のアドレスから引いてリモートプロセス上のモジュールアドレスを足せばリモートでの Proc のアドレスが求まる
            new(procAddress.ToInt64() - hModule.ToInt64() + moduleBaseAddress.ToInt64());
    }
}
