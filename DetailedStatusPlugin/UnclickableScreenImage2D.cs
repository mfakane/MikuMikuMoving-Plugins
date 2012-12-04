using System.Drawing;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.DetailedStatusPlugin
{
	class UnclickableScreenImage2D : ScreenImage_2D
	{
		public override bool Contains(Point point)
		{
			return false;
		}
	}
}
