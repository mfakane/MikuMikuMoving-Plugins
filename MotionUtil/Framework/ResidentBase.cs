using System.Drawing;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	public abstract class ResidentBase : PluginBase, IResidentPlugin
	{
		public virtual void Initialize()
		{
		}

		public virtual void Enabled()
		{
		}

		public virtual void Disabled()
		{
		}

		public virtual void Update(float frame, float elapsedTime)
		{
		}

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
