using System.Drawing;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

/// <summary>
/// クリックに反応しない画像を表示します。
/// </summary>
class NonInteractiveScreenImage2D : ScreenImage_2D
{
	public override bool Contains(Point point) => false;
}