using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin;

public partial class BeatBasedCopyForm : Form
{
	public int BeginFrame
	{
		get => (int)beginNumericUpDown.Value;
		set => beginNumericUpDown.Value = value;
	}

	public float BeatsPerMinute
	{
		get => (float)bpmNumericUpDown.Value;
		set => bpmNumericUpDown.Value = (decimal)value;
	}

	public float StartupBeats
	{
		get => (float)startupBeatsNumericUpDown.Value;
		set => startupBeatsNumericUpDown.Value = (decimal)value;
	}

	public float IntervalBeats
	{
		get => (float)intervalNumericUpDown.Value;
		set => intervalNumericUpDown.Value = (decimal)value;
	}

	public int Times
	{
		get => (int)timesNumericUpDown.Value;
		set => timesNumericUpDown.Value = value;
	}

	public BeatBasedCopyForm()
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
}