using System.Drawing;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	public abstract class CommandBase : PluginBase, ICommandPlugin
	{
		public abstract void Run(CommandArgs e);

		public abstract string EnglishText
		{
			get;
		}

		public virtual Image Image
		{
			get
			{
				return null;
			}
		}

		public virtual Image SmallImage
		{
			get
			{
				return null;
			}
		}

		public abstract string Text
		{
			get;
		}
	}
}
