using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;

public partial class AnimationEntryControl : UserControl
{
	public event EventHandler<EntryChangedEventArgs>? ValueChanged;

	public float TrackMinimum
	{
		get => beginTrackBar.Minimum / 100f;
		set => beginTrackBar.Minimum = endTrackBar.Minimum = (int)(value * 100);
	}

	public float TrackMaximum
	{
		get => beginTrackBar.Maximum / 100f;
		set => beginTrackBar.Maximum = endTrackBar.Maximum = (int)(value * 100);
	}

	public float NumericMinimum
	{
		get => (float)beginNumericUpDown.Minimum;
		set =>
			beginNumericUpDown.Minimum =
				endNumericUpDown.Minimum = (decimal)value;
	}

	public float NumericMaximum
	{
		get => (float)beginNumericUpDown.Maximum;
		set =>
			beginNumericUpDown.Maximum =
				endNumericUpDown.Maximum = (decimal)value;
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public float BeginValue
	{
		get => (float)beginNumericUpDown.Value;
		set
		{
			beginNumericUpDown.Value = float.IsNaN(value)
				? 0
				: Math.Max(beginNumericUpDown.Minimum, Math.Min(beginNumericUpDown.Maximum, (decimal)value));
			beginTrackBar.Value = float.IsNaN(value)
				? 0
				: (int)Math.Max(beginTrackBar.Minimum, Math.Min(beginTrackBar.Maximum, value * 100));
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public float EndValue
	{
		get => (float)endNumericUpDown.Value;
		set
		{
			endNumericUpDown.Value = float.IsNaN(value)
				? 0
				: Math.Max(endNumericUpDown.Minimum, Math.Min(endNumericUpDown.Maximum, (decimal)value));
			endTrackBar.Value = float.IsNaN(value)
				? 0
				: (int)Math.Max(endTrackBar.Minimum, Math.Min(endTrackBar.Maximum, value * 100));
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public AnimationMode Mode
	{
		get
		{
			var items = modeContextMenuStrip.Items
				.Cast<ToolStripItem>()
				.TakeWhile(x => x is not ToolStripSeparator)
				.OfType<ToolStripMenuItem>()
				.ToArray();
			var checkedItem = items.FirstOrDefault(x => x.Checked);

			return checkedItem == null ? AnimationMode.None : (AnimationMode)Array.IndexOf(items, checkedItem);
		}
		set
		{
			if (Mode == value) return;
			
			var items = modeContextMenuStrip.Items
				.Cast<ToolStripItem>()
				.TakeWhile(x => x is not ToolStripSeparator)
				.OfType<ToolStripMenuItem>()
				.ToArray();

			foreach (var i in items)
				i.Checked = false;

			items[(int)value].Checked = true;
			UpdateView(value);
		}
	}

	public string ParameterName
	{
		get => switchButton.Text;
		set => switchButton.Text = value;
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int IterationDuration
	{
		get
		{
			int.TryParse(intervalTextBox.Text, out var rt);

			return rt;
		}
		set => intervalTextBox.Text = value.ToString();
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool EaseIn
	{
		get => easeInMenuItem.Checked;
		set => easeInMenuItem.Checked = value;
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool EaseOut
	{
		get => easeOutMenuItem.Checked;
		set => easeOutMenuItem.Checked = value;
	}

	public AnimationEntryControl()
	{
		InitializeComponent();
		Mode = AnimationMode.None;
	}

	void beginNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		var newValue = sender == beginNumericUpDown ? (float)beginNumericUpDown.Value : beginTrackBar.Value / 100f;

		if (sender == beginTrackBar &&
		    (newValue < TrackMinimum || newValue > TrackMaximum))
			return;
		
		BeginValue = newValue;
		OnValueChanged();
	}

	void endNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		var newValue = sender == endNumericUpDown ? (float)endNumericUpDown.Value : endTrackBar.Value / 100f;

		EndValue = newValue;
		OnValueChanged();
	}

	void noneMenuItem_Click(object sender, EventArgs e)
	{
		var items = modeContextMenuStrip.Items;
		var mode = (AnimationMode)items.IndexOf((ToolStripItem)sender);
		
		UpdateView(mode);
		Mode = mode;
	}

	void switchButton_Click(object sender, EventArgs e)
	{
		modeContextMenuStrip.Show(switchButton, new(0, switchButton.Height + 1));
	}

	void intervalTextBox_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar);
	}

    void AnimationEntryControl_Resize(object sender, EventArgs e)
    {
		var leftAndRightWidth = Width / 2 - middlePanel.Width / 2;
		leftPanel.Width = leftAndRightWidth;
		rightPanel.Width = leftAndRightWidth;
    }

    void UpdateView(AnimationMode value)
    {
	    rightPanel.Enabled = value != AnimationMode.None;

	    switch (value)
	    {
		    case AnimationMode.None:
		    case AnimationMode.NoInterpolation:
		    case AnimationMode.ByAcceleration:
			    easeInMenuItem.Enabled =
				    easeOutMenuItem.Enabled =
					    intervalMenuItem.Enabled = false;

			    break;
		    case AnimationMode.LinearInterpolation:
		    case AnimationMode.LinearInterpolationFirstAndLastOnly:
			    easeInMenuItem.Enabled =
				    easeOutMenuItem.Enabled = true;
			    intervalMenuItem.Enabled = false;

			    break;
		    case AnimationMode.RandomFirstAndLast:
		    case AnimationMode.RepeatFirstAndLast:
			    easeInMenuItem.Enabled =
				    easeOutMenuItem.Enabled =
					    intervalMenuItem.Enabled = true;

			    break;
	    }

	    OnValueChanged();
    }

    void OnValueChanged() =>
	    ValueChanged?.Invoke(this, new EntryChangedEventArgs(
		    BeginValue,
		    EndValue,
		    Mode,
		    IterationDuration,
		    EaseIn,
		    EaseOut
	    ));

    void easeInMenuItem_CheckedChanged(object sender, EventArgs e)
    {
		OnValueChanged();
    }

    void intervalTextBox_TextChanged(object sender, EventArgs e)
    {
        OnValueChanged();
    }
}