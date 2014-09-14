using System;
using System.Reflection;
using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.Framework
{
	class IncrementSelectionNumericUpDown : NumericUpDown
	{
		TextBox textBox;

		public IncrementSelectionNumericUpDown()
			: base()
		{
			textBox = (TextBox)typeof(NumericUpDown).GetField("upDownEdit", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
			textBox.HideSelection = false;
		}

		decimal GetUpDownValue(TextBox textBox)
		{
			var isNegative = this.Value < 0;
			var selection = textBox.SelectionStart;
			var point = textBox.Text.IndexOf('.');

			if (point == -1)
				point = textBox.TextLength;

			return textBox.SelectionLength == 0 ? this.Increment : (decimal)(selection > point
				? Math.Pow(0.1, selection - point)
				: Math.Pow(10, point - selection - (isNegative ? 1 : 1)));
		}

		public override void UpButton()
		{
			var oldIncrement = this.Increment;
			var selection = new
			{
				textBox.SelectionStart,
				textBox.SelectionLength,
			};
			var textLength = textBox.TextLength;

			this.Increment = GetUpDownValue(textBox);
			base.UpButton();
			this.Increment = oldIncrement;

			var targetIndex = Math.Max(0, selection.SelectionStart - (textLength - textBox.TextLength));

			this.Select(targetIndex, Math.Min(targetIndex + selection.SelectionLength, textBox.TextLength) - targetIndex);
		}

		public override void DownButton()
		{
			var oldIncrement = this.Increment;
			var selection = new
			{
				textBox.SelectionStart,
				textBox.SelectionLength,
			};
			var textLength = textBox.TextLength;

			this.Increment = GetUpDownValue(textBox);
			base.DownButton();
			this.Increment = oldIncrement;

			var targetIndex = Math.Max(0, selection.SelectionStart - (textLength - textBox.TextLength));

			this.Select(targetIndex, Math.Min(targetIndex + selection.SelectionLength, textBox.TextLength) - targetIndex);
		}
	}
}
