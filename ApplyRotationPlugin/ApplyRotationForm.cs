using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin;

public partial class ApplyRotationForm : Form
{
	public event EventHandler<ValueChangedEventArgs>? ValueChanged;

	public Vector3 Position =>
		new(
			(float)positionXNumericUpDown.Value,
			(float)positionYNumericUpDown.Value,
			(float)positionZNumericUpDown.Value
		);

	public Vector3 Rotation =>
		MathHelper.ToRadians(new Vector3(
			(float)rotationXNumericUpDown.Value,
			(float)rotationYNumericUpDown.Value,
			(float)rotationZNumericUpDown.Value
		));

	public bool IsMoveOnly => moveOnlyCheckBox.Checked;

	public ApplyRotationForm(string language)
	{
		InitializeComponent();
		Font = SystemFonts.MessageBoxFont;

		if (language != "ja")
		{
			Text = "Apply Rotation";
			positionLabel.Text = "Origin";
			rotationLabel.Text = "Angle";
			okButton.Text = "Apply";
			cancelButton.Text = "Cancel";
		}

		var numericUpDowns = GetControls().OfType<NumericUpDown>().ToDictionary(x => x.Name);
		var controls = GetControls().OfType<TrackBar>()
			.Select(x => new
			{
				TrackBar = x,
				NumericUpDown = x.Name.Replace("TrackBar", "NumericUpDown"),
			})
			.Where(x => numericUpDowns.ContainsKey(x.NumericUpDown))
			.Select(x => new
			{
				x.TrackBar,
				NumericUpDown = numericUpDowns[x.NumericUpDown],
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

	IEnumerable<Control> GetControls() =>
		GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
			.Where(x => typeof(Control).IsAssignableFrom(x.FieldType))
			.Select(x => x.GetValue(this))
			.Cast<Control>();

	void positionXNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		ValueChanged?.Invoke(this, new ValueChangedEventArgs
		{
			Position = Position,
			Rotation = Rotation,
			IsMoveOnly = IsMoveOnly,
		});
	}

	void ApplyRotationForm_Load(object sender, EventArgs e)
	{
		ValueChanged?.Invoke(this, new ValueChangedEventArgs
		{
			Position = Position,
			Rotation = Rotation,
			IsMoveOnly = IsMoveOnly,
		});
	}
}