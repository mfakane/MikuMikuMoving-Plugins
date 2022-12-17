using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public abstract class PluginBase : IBasePlugin, IHaveScenePlugin
{
	public IWin32Window ApplicationForm { get; set; } = null!;

	public Scene Scene { get; set; } = null!;

	public abstract string Description { get; }

	public abstract Guid GUID { get; }

	protected string Localize(string ja, string en) =>
		Scene.Localize(ja, en);
	
	public virtual void Dispose()
	{
	}
}