using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin
{
	public partial class BeatBasedCopyForm : Form
	{
		public int BeginFrame
		{
			get
			{
				return (int)beginNumericUpDown.Value;
			}
			set
			{
				beginNumericUpDown.Value = value;
			}
		}

		public float BeatsPerMinute
		{
			get
			{
				return (float)bpmNumericUpDown.Value;
			}
			set
			{
				bpmNumericUpDown.Value = (decimal)value;
			}
		}

		public float StartupBeats
		{
			get
			{
				return (float)startupBeatsNumericUpDown.Value;
			}
			set
			{
				startupBeatsNumericUpDown.Value = (decimal)value;
			}
		}

		public float IntervalBeats
		{
			get
			{
				return (float)intervalNumericUpDown.Value;
			}
			set
			{
				intervalNumericUpDown.Value = (decimal)value;
			}
		}

		public int Times
		{
			get
			{
				return (int)timesNumericUpDown.Value;
			}
			set
			{
				timesNumericUpDown.Value = value;
			}
		}

		public BeatBasedCopyForm()
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
	}
}
