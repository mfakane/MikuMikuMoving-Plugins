using System;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	class EnableScreenObjectBlock : IDisposable
	{
		readonly object pluginProxy;

		public EnableScreenObjectBlock(Scene scene)
		{
			pluginProxy = scene.Member("Controller").GetType().Assembly.GetType("MikuMikuMoving.PluginProxy").GetProperty("ProcessingProxy").GetValue(null, null);
			pluginProxy.SetMember("Enable", true);
		}

		public void Dispose()
		{
			pluginProxy.SetMember("Enable", false);
		}
	}
}
