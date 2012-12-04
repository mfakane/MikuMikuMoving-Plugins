using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin
{
	public partial class ApplyRotationForm : Form
	{
		public event EventHandler ValueChanged;

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
			set
			{
				positionXNumericUpDown.Value = (decimal)value.X;
				positionYNumericUpDown.Value = (decimal)value.Y;
				positionZNumericUpDown.Value = (decimal)value.Z;
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
			set
			{
				rotationXNumericUpDown.Value = (decimal)value.X;
				rotationYNumericUpDown.Value = (decimal)value.Y;
				rotationZNumericUpDown.Value = (decimal)value.Z;
			}
		}

		public bool IsMoveOnly
		{
			get
			{
				return moveOnlyCheckBox.Checked;
			}
			set
			{
				moveOnlyCheckBox.Checked = value;
			}
		}

		public ApplyRotationForm(string language)
		{
			InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;

			if (Util.IsEnglish(language))
			{
				this.Text = "Apply Rotation";
				positionLabel.Text = "Origin";
				rotationLabel.Text = "Angle";
				okButton.Text = "Apply";
				cancelButton.Text = "Cancel";
			}

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

				i.TrackBar.ValueChanged += (sender, e) =>
				{
					if (changing)
						return;

					changing = true;
					i.NumericUpDown.Value = i.TrackBar.Value;
					changing = false;
				};
				i.NumericUpDown.ValueChanged += (sender, e) =>
				{
					if (changing)
						return;

					changing = true;
					i.TrackBar.Value = Math.Min(Math.Max(i.TrackBar.Minimum, (int)i.NumericUpDown.Value), i.TrackBar.Maximum);
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

		void ApplyRotationForm_Load(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, EventArgs.Empty);
		}
	}
}
