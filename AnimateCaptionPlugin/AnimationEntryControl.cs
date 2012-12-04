using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public partial class AnimationEntryControl : UserControl
	{
		public event EventHandler BeginValueChanged;
		public event EventHandler EndValueChanged;
		public event EventHandler ModeChanged;

		bool beginChanging;
		bool endChanging;

		public float TrackMinimum
		{
			get
			{
				return beginTrackBar.Minimum / 100f;
			}
			set
			{
				beginTrackBar.Minimum = endTrackBar.Minimum = (int)(value * 100);
			}
		}

		public float TrackMaximum
		{
			get
			{
				return beginTrackBar.Maximum / 100f;
			}
			set
			{
				beginTrackBar.Maximum = endTrackBar.Maximum = (int)(value * 100);
			}
		}

		public float NumericMinimum
		{
			get
			{
				return (float)beginNumericUpDown.Minimum;
			}
			set
			{
				beginNumericUpDown.Minimum =
					endNumericUpDown.Minimum = (decimal)value;
			}
		}

		public float NumericMaximum
		{
			get
			{
				return (float)beginNumericUpDown.Maximum;
			}
			set
			{
				beginNumericUpDown.Maximum =
					endNumericUpDown.Maximum = (decimal)value;
			}
		}

		public float BeginValue
		{
			get
			{
				return (float)beginNumericUpDown.Value;
			}
			set
			{
				using (FinallyBlock.Create(beginChanging = true, _ => beginChanging = false))
				{
					beginNumericUpDown.Value = float.IsNaN(value) ? 0 : Math.Max(beginNumericUpDown.Minimum, Math.Min(beginNumericUpDown.Maximum, (decimal)value));
					beginTrackBar.Value = float.IsNaN(value) ? 0 : (int)Math.Max(beginTrackBar.Minimum, Math.Min(beginTrackBar.Maximum, value * 100));
				}
			}
		}

		public float EndValue
		{
			get
			{
				return (float)endNumericUpDown.Value;
			}
			set
			{
				using (FinallyBlock.Create(endChanging = true, _ => endChanging = false))
				{
					endNumericUpDown.Value = float.IsNaN(value) ? 0 : Math.Max(endNumericUpDown.Minimum, Math.Min(endNumericUpDown.Maximum, (decimal)value));
					endTrackBar.Value = float.IsNaN(value) ? 0 : (int)Math.Max(endTrackBar.Minimum, Math.Min(endTrackBar.Maximum, value * 100));
				}
			}
		}

		public AnimationMode Mode
		{
			get
			{
				var items = modeContextMenuStrip.Items;

				return (AnimationMode)items.IndexOf(items.OfType<ToolStripMenuItem>().Where(_ => _.Checked).DefaultIfEmpty(noneMenuItem).First());
			}
			set
			{
				var was = this.Mode;
				var items = modeContextMenuStrip.Items;

				foreach (var i in items.OfType<ToolStripMenuItem>())
					i.Checked = false;

				((ToolStripMenuItem)items[(int)value]).Checked = true;
				endTrackBar.Enabled = endNumericUpDown.Enabled = value != AnimationMode.None;

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

				if (was != value && ModeChanged != null)
					ModeChanged(this, EventArgs.Empty);
			}
		}

		public string ParameterName
		{
			get
			{
				return switchButton.Text;
			}
			set
			{
				switchButton.Text = value;
			}
		}

		public int IterationDuration
		{
			get
			{
				int rt;

				int.TryParse(intervalTextBox.Text, out rt);

				return rt;
			}
			set
			{
				intervalTextBox.Text = value.ToString();
			}
		}

		public bool EaseIn
		{
			get
			{
				return easeInMenuItem.Checked;
			}
			set
			{
				easeInMenuItem.Checked = value;
			}
		}

		public bool EaseOut
		{
			get
			{
				return easeOutMenuItem.Checked;
			}
			set
			{
				easeOutMenuItem.Checked = value;
			}
		}

		public AnimationEntryControl()
		{
			InitializeComponent();
			this.Mode = AnimationMode.None;
		}

		void beginNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (!beginChanging)
				using (FinallyBlock.Create(beginChanging = true, _ => beginChanging = false))
				{
					this.BeginValue = sender == beginNumericUpDown ? (float)beginNumericUpDown.Value : beginTrackBar.Value / 100f;

					if (BeginValueChanged != null)
						BeginValueChanged(this, EventArgs.Empty);
				}
		}

		void endNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (!endChanging)
				using (FinallyBlock.Create(endChanging = true, _ => endChanging = false))
				{
					this.EndValue = sender == endNumericUpDown ? (float)endNumericUpDown.Value : endTrackBar.Value / 100f;

					if (EndValueChanged != null)
						EndValueChanged(this, EventArgs.Empty);
				}
		}

		void noneMenuItem_Click(object sender, EventArgs e)
		{
			var items = modeContextMenuStrip.Items;

			this.Mode = (AnimationMode)items.IndexOf((ToolStripItem)sender);
		}

		void switchButton_Click(object sender, EventArgs e)
		{
			modeContextMenuStrip.Show(switchButton, new Point(0, switchButton.Height + 1));
		}

		void intervalTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar);
		}
	}
}
