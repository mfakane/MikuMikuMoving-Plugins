using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.DetailedTimePlugin;

public partial class DetailedTimeControl : UserControl
{
	public long BeginFrame
	{
		get => (long)beginFrameNumericUpDown.Value;
		set => beginFrameNumericUpDown.Value = value;
	}

	public float BeatsPerMinute
	{
		get => (float)beatsPerMinuteNumericUpDown.Value;
		set => beatsPerMinuteNumericUpDown.Value = (decimal)value;
	}

	public int BeatsPerMeasure
	{
		get => (int)beatsPerMeasureNumericUpDown.Value;
		set => beatsPerMeasureNumericUpDown.Value = value;
	}

	public int Resolution
	{
		get => int.Parse(resolutionComboBox.SelectedItem.ToString());
		set => resolutionComboBox.SelectedItem = value.ToString();
	}

	public DetailedTimeControl()
	{
		InitializeComponent();
		resolutionComboBox.SelectedIndex = 7;
	}
}