using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin
{
	public partial class ApplyNoiseForm : Form
	{
		public bool IsPositionEnabled
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

		public bool IsRotationEnabled
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

		public bool IsEnvironmentEnabled
		{
			get
			{
				return environmentPanel.Visible;
			}
			set
			{
				environmentPanel.Visible = value;
			}
		}

		public int KeyFrameInterval
		{
			get
			{
				return (int)keyFrameIntervalNumericUpDown.Value;
			}
			set
			{
				keyFrameIntervalNumericUpDown.Value = value;
			}
		}

		public int NoiseValueInterval
		{
			get
			{
				return (int)noiseValueIntervalNumericUpDown.Value;
			}
			set
			{
				noiseValueIntervalNumericUpDown.Value = value;
			}
		}

		public int KeyShiftWidth
		{
			get
			{
				return (int)keyShiftNoiseNumericUpDown.Value;
			}
			set
			{
				keyShiftNoiseNumericUpDown.Value = value;
			}
		}

		public NoiseValue NoiseValue
		{
			get
			{
				return new NoiseValue
				{
					PositionWidth = new Vector3((float)positionXNumericUpDown.Value, (float)positionYNumericUpDown.Value, (float)positionZNumericUpDown.Value),
					RotationWidth = new Vector3((float)rotationXNumericUpDown.Value, (float)rotationYNumericUpDown.Value, (float)rotationZNumericUpDown.Value),
					GravityWidth = (float)gravityNumericUpDown.Value,
					GravityDirectionWidth = new Vector3((float)gravityXNumericUpDown.Value, (float)gravityYNumericUpDown.Value, (float)gravityZNumericUpDown.Value),
				};
			}
			set
			{
				positionXNumericUpDown.Value = (decimal)value.PositionWidth.X;
				positionYNumericUpDown.Value = (decimal)value.PositionWidth.Y;
				positionZNumericUpDown.Value = (decimal)value.PositionWidth.Z;
				rotationXNumericUpDown.Value = (decimal)value.RotationWidth.X;
				rotationYNumericUpDown.Value = (decimal)value.RotationWidth.Y;
				rotationZNumericUpDown.Value = (decimal)value.RotationWidth.Z;
				gravityNumericUpDown.Value = (decimal)value.GravityWidth;
				gravityXNumericUpDown.Value = (decimal)value.GravityDirectionWidth.X;
				gravityYNumericUpDown.Value = (decimal)value.GravityDirectionWidth.Y;
				gravityZNumericUpDown.Value = (decimal)value.GravityDirectionWidth.Z;
			}
		}

		public bool IsPositionLocal
		{
			get
			{
				return positionLocalCheckBox.Checked;
			}
			set
			{
				positionLocalCheckBox.Checked = value;
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

		public bool IsRotationLocal
		{
			get
			{
				return rotationLocalCheckBox.Checked;
			}
			set
			{
				rotationLocalCheckBox.Checked = value;
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

		public ApplyNoiseForm()
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
				var isPercentage = new[] { "gravityXTrackBar", "gravityYTrackBar", "gravityZTrackBar" }.Contains(i.TrackBar.Name);

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
	}
}
