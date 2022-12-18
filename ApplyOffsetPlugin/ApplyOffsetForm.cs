using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin;

public partial class ApplyOffsetForm : Form
{
	public event EventHandler<ValueChangedEventArgs>? ValueChanged;

	public bool IsPositionVisible
	{
		get => positionPanel.Visible;
		set => positionPanel.Visible = value;
	}

	public Vector3 Position =>
		new(
			(float)positionXNumericUpDown.Value,
			(float)positionYNumericUpDown.Value,
			(float)positionZNumericUpDown.Value
		);

	public bool IsPositionLocalVisible
	{
		get => positionLocalCheckBox.Visible;
		set => positionLocalCheckBox.Visible = value;
	}

	public bool IsPositionLocal => positionLocalCheckBox.Checked;

	public bool IsRotationVisible
	{
		get => rotationPanel.Visible;
		set => rotationPanel.Visible = value;
	}

	public Vector3 Rotation =>
		MathHelper.ToRadians(new Vector3(
			(float)rotationXNumericUpDown.Value,
			(float)rotationYNumericUpDown.Value,
			(float)rotationZNumericUpDown.Value
		));

	public bool IsRotationLocal => rotationLocalCheckBox.Checked;

	public bool IsRotationLocalVisible
	{
		get => rotationLocalCheckBox.Visible;
		set => rotationLocalCheckBox.Visible = value;
	}

	public bool IsWeightVisible
	{
		get => weightPanel.Visible;
		set => weightPanel.Visible = value;
	}

	public float Weight => (float)weightNumericUpDown.Value;

	public bool IsDistanceVisible
	{
		get => distancePanel.Visible;
		set => distancePanel.Visible = value;
	}

	public float Distance => (float)distanceNumericUpDown.Value;

	public bool IsColorVisible
	{
		get => colorPanel.Visible;
		set => colorPanel.Visible = value;
	}

	public Vector3 Color =>
		new(
			(float)rNumericUpDown.Value,
			(float)gNumericUpDown.Value,
			(float)bNumericUpDown.Value
		);

	public ApplyOffsetForm()
	{
		InitializeComponent();
		Font = SystemFonts.MessageBoxFont;

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

	IEnumerable<Control> GetControls() =>
		GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
			.Where(x => typeof(Control).IsAssignableFrom(x.FieldType))
			.Select(x => x.GetValue(this))
			.Cast<Control>();

	void positionAndRotationNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		ValueChanged?.Invoke(this, new ValueChangedEventArgs
		{
			PositionAndRotation = new(
				Position,
				IsPositionLocal,
				Rotation,
				IsRotationLocal
			),
		});
	}

	void weightNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		ValueChanged?.Invoke(this, new ValueChangedEventArgs
		{
			Weight = Weight,
		});
	}

	void distanceNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		ValueChanged?.Invoke(this, new ValueChangedEventArgs
		{
			Distance = Distance,
		});
	}

	void colorNumericUpDown_ValueChanged(object sender, EventArgs e)
	{
		ValueChanged?.Invoke(this, new ValueChangedEventArgs
		{
			Color = Color,
		});
	}
}