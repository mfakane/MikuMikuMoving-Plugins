using System;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	public abstract class PluginBase : IBasePlugin, IHaveScenePlugin
	{
		Scene scene;

		public IWin32Window ApplicationForm
		{
			get;
			set;
		}

		public Scene Scene
		{
			get
			{
				return scene;
			}
			set
			{
				if (scene != (scene = value))
					OnSceneInitialized();
			}
		}

		public abstract string Description
		{
			get;
		}

		public abstract Guid GUID
		{
			get;
		}

		protected virtual void OnSceneInitialized()
		{
		}

		public virtual void Dispose()
		{
		}
	}
}
