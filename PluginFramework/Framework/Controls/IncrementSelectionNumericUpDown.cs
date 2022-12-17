using System;
using System.Reflection;
using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.Framework;

class IncrementSelectionNumericUpDown : NumericUpDown
{
	readonly TextBox textBox;

	public IncrementSelectionNumericUpDown()
	{
		textBox = (TextBox)typeof(NumericUpDown).GetField("upDownEdit", BindingFlags.NonPublic | BindingFlags.Instance)!
			.GetValue(this);
		textBox.HideSelection = false;
	}

	decimal GetUpDownValue()
	{
		var isNegative = Value < 0;
		var selection = textBox.SelectionStart;
		var point = textBox.Text.IndexOf('.');

		if (point == -1)
			point = textBox.TextLength;

		return textBox.SelectionLength == 0
			? Increment // 選択なしなら設定されているインクリメント値
			: (decimal)(selection > point
				? Math.Pow(0.1, selection - point) // 小数点より右側なら 0.1 ^ 小数点からの桁数
				: Math.Pow(10, point - selection - 1)); // 小数点より左側なら 10 ^ 小数点からの桁数
	}

	public override void UpButton()
	{
		var oldIncrement = Increment;
		var selection = new
		{
			textBox.SelectionStart,
			textBox.SelectionLength,
		};
		var textLength = textBox.TextLength;

		Increment = GetUpDownValue();
		base.UpButton();
		Increment = oldIncrement;

		var targetIndex = Math.Max(0, selection.SelectionStart - (textLength - textBox.TextLength));

		Select(targetIndex, Math.Min(targetIndex + selection.SelectionLength, textBox.TextLength) - targetIndex);
	}

	public override void DownButton()
	{
		var oldIncrement = Increment;
		var selection = new
		{
			textBox.SelectionStart,
			textBox.SelectionLength,
		};
		var textLength = textBox.TextLength;

		Increment = GetUpDownValue();
		base.DownButton();
		Increment = oldIncrement;

		var targetIndex = Math.Max(0, selection.SelectionStart - (textLength - textBox.TextLength));

		Select(targetIndex, Math.Min(targetIndex + selection.SelectionLength, textBox.TextLength) - targetIndex);
	}
}