using System;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	static class CaptionEx
	{
		public static object GetRealCaption(this ICaption caption)
		{
			if (caption.GetType().FullName != "MikuMikuMoving.Plugin.CCaption")
				throw new ArgumentException();

			return caption.Member("Controller").Member("CaptionManager").Member("CaptionList").Member("Item", caption.Member("Index"));
		}
	}
}
