using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin
{
	public partial class ApplyOffsetForm : Form
	{
		public event EventHandler ValueChanged;

		public bool IsPositionVisible
		{
			get
			{
				return positionPanel.Visible;
			}
			set
			{
				positionPanel.Visible = value;
			}
		}

		public Vector3 Position
		{
			get
			{
				return new Vector3
				(
					(float)positionXNumericUpDown.Value,
					(float)positionYNumericUpDown.Value,
					(float)positionZNumericUpDown.Value
				);
			}
		}

		public bool IsPositionLocalVisible
		{
			get
			{
				return positionLocalCheckBox.Visible;
			}
			set
			{
				positionLocalCheckBox.Visible = value;
			}
		}

		public bool IsPositionLocal
		{
			get
			{
				return positionLocalCheckBox.Checked;
			}
		}

		public bool IsRotationVisible
		{
			get
			{
				return rotationPanel.Visible;
			}
			set
			{
				rotationPanel.Visible = value;
			}
		}

		public Vector3 Rotation
		{
			get
			{
				return new Vector3
				(
					(float)rotationXNumericUpDown.Value,
					(float)rotationYNumericUpDown.Value,
					(float)rotationZNumericUpDown.Value
				);
			}
		}

		public bool IsRotationLocal
		{
			get
			{
				return rotationLocalCheckBox.Checked;
			}
		}

		public bool IsRotationLocalVisible
		{
			get
			{
				return rotationLocalCheckBox.Visible;
			}
			set
			{
				rotationLocalCheckBox.Visible = value;
			}
		}

		public bool IsWeightVisible
		{
			get
			{
				return weightPanel.Visible;
			}
			set
			{
				weightPanel.Visible = value;
			}
		}

		public float Weight
		{
			get
			{
				return (float)weightNumericUpDown.Value;
			}
		}

		public bool IsDistanceVisible
		{
			get
			{
				return distancePanel.Visible;
			}
			set
			{
				distancePanel.Visible = value;
			}
		}

		public float Distance
		{
			get
			{
				return (float)distanceNumericUpDown.Value;
			}
		}

		public bool IsColorVisible
		{
			get
			{
				return colorPanel.Visible;
			}
			set
			{
				colorPanel.Visible = value;
			}
		}

		public Vector3 Color
		{
			get
			{
				return new Vector3
				(
					(float)rNumericUpDown.Value,
					(float)gNumericUpDown.Value,
					(float)bNumericUpDown.Value
				);
			}
		}

		public ApplyOffsetForm()
		{
			InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;

			var numericUpDowns = GetControls().OfType<NumericUpDown>().ToDictionary(_ => _.Name);
			var controls = GetControls().OfType<TrackBar>()
										.Select(_ => new
										{
											TrackBar = _,
											NumericUpDown = _.Name.Replace("TrackBar", "NumericUpDown"),
										})
										.Where(_ => numericUpDowns.ContainsKey(_.NumericUpDown))
										.Select(_ => new
										{
											_.TrackBar,
											NumericUpDown = numericUpDowns[_.NumericUpDown],
										});

			foreach (var i in controls)
			{
				var changing = false;
				var isPercentage = new[] { "weightTrackBar", "rTrackBar", "gTrackBar", "bTrackBar" }.Contains(i.TrackBar.Name);

				i.TrackBar.ValueChanged += (sender, e) =>
				{
					if (changing)
						return;

					changing = true;
					i.NumericUpDown.Value = isPercentage ? (decimal)i.TrackBar.Value / 100 : i.TrackBar.Value;
					changing = false;
				};
				i.NumericUpDown.ValueChanged += (sender, e) =>
				{
					if (changing)
						return;

					changing = true;
					i.TrackBar.Value = Math.Min(Math.Max(i.TrackBar.Minimum, (int)(isPercentage ? i.NumericUpDown.Value * 100 : i.NumericUpDown.Value)), i.TrackBar.Maximum);
					changing = false;
				};
			}
		}

		IEnumerable<Control> GetControls()
		{
			return this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
								 .Where(_ => typeof(Control).IsAssignableFrom(_.FieldType))
								 .Select(_ => _.GetValue(this))
								 .Cast<Control>();
		}

		void positionXNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, EventArgs.Empty);
		}
	}
}
