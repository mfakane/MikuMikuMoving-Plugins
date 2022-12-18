using System;

namespace Linearstar.MikuMikuMoving.Framework;

public class Disposable : IDisposable
{
    readonly Action disposeAction;

    public static IDisposable DoNothing { get; } = Create(() => { });

    Disposable(Action disposeAction) =>
        this.disposeAction = disposeAction;

    public static IDisposable Create(Action disposeAction) =>
        new Disposable(disposeAction);
    
    public void Dispose() =>
        disposeAction();
}