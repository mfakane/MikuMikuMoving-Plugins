using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DxMath;
using Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin;

public partial class ApplyNoiseForm : Form
{
	public bool IsPositionEnabled
	{
		get => positionPanel.Visible;
		set => positionPanel.Visible = value;
	}

	public bool IsRotationEnabled
	{
		get => rotationPanel.Visible;
		set => rotationPanel.Visible = value;
	}

	public bool IsEnvironmentEnabled
	{
		get => environmentPanel.Visible;
		set => environmentPanel.Visible = value;
	}

	public int KeyFrameInterval
	{
		get => (int)keyFrameIntervalNumericUpDown.Value;
		set => keyFrameIntervalNumericUpDown.Value = value;
	}

	public int NoiseValueInterval
	{
		get => (int)noiseValueIntervalNumericUpDown.Value;
		set => noiseValueIntervalNumericUpDown.Value = value;
	}

	public int KeyShiftWidth
	{
		get => (int)keyShiftNoiseNumericUpDown.Value;
		set => keyShiftNoiseNumericUpDown.Value = value;
	}

	public NoiseValue NoiseValue
	{
		get =>
			new(
				new((float)positionXNumericUpDown.Value, (float)positionYNumericUpDown.Value, (float)positionZNumericUpDown.Value),
				MathHelper.ToRadians(new Vector3((float)rotationXNumericUpDown.Value, (float)rotationYNumericUpDown.Value, (float)rotationZNumericUpDown.Value)),
				(float)gravityNumericUpDown.Value,
				new((float)gravityXNumericUpDown.Value, (float)gravityYNumericUpDown.Value, (float)gravityZNumericUpDown.Value)
			);
		set
		{
			positionXNumericUpDown.Value = (decimal)value.Position.X;
			positionYNumericUpDown.Value = (decimal)value.Position.Y;
			positionZNumericUpDown.Value = (decimal)value.Position.Z;
			var degrees = MathHelper.ToDegrees(value.Rotation);
			rotationXNumericUpDown.Value = (decimal)degrees.X;
			rotationYNumericUpDown.Value = (decimal)degrees.Y;
			rotationZNumericUpDown.Value = (decimal)degrees.Z;
			gravityNumericUpDown.Value = (decimal)value.Gravity;
			gravityXNumericUpDown.Value = (decimal)value.GravityDirection.X;
			gravityYNumericUpDown.Value = (decimal)value.GravityDirection.Y;
			gravityZNumericUpDown.Value = (decimal)value.GravityDirection.Z;
		}
	}

	public bool IsPositionLocal
	{
		get => positionLocalCheckBox.Checked;
		set => positionLocalCheckBox.Checked = value;
	}

	public bool IsPositionLocalVisible
	{
		get => positionLocalCheckBox.Visible;
		set => positionLocalCheckBox.Visible = value;
	}

	public bool IsRotationLocal
	{
		get => rotationLocalCheckBox.Checked;
		set => rotationLocalCheckBox.Checked = value;
	}

	public bool IsRotationLocalVisible
	{
		get => rotationLocalCheckBox.Visible;
		set => rotationLocalCheckBox.Visible = value;
	}

	public ApplyNoiseForm()
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
		return GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
			.Where(x => typeof(Control).IsAssignableFrom(x.FieldType))
			.Select(x => x.GetValue(this))
			.Cast<Control>();
	}
}