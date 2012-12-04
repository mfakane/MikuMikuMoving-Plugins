using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.DetailedTimePlugin
{
	public partial class DetailedTimeControl : UserControl
	{
		public long BeginFrame
		{
			get
			{
				return (long)beginFrameNumericUpDown.Value;
			}
			set
			{
				beginFrameNumericUpDown.Value = value;
			}
		}

		public float BeatsPerMinute
		{
			get
			{
				return (float)beatsPerMinuteNumericUpDown.Value;
			}
			set
			{
				beatsPerMinuteNumericUpDown.Value = (decimal)value;
			}
		}

		public int BeatsPerMeasure
		{
			get
			{
				return (int)beatsPerMeasureNumericUpDown.Value;
			}
			set
			{
				beatsPerMeasureNumericUpDown.Value = value;
			}
		}

		public int Resolution
		{
			get
			{
				return int.Parse(resolutionComboBox.SelectedItem.ToString());
			}
			set
			{
				resolutionComboBox.SelectedItem = value.ToString();
			}
		}

		public DetailedTimeControl()
		{
			InitializeComponent();
			this.resolutionComboBox.SelectedIndex = 7;
		}
	}
}
