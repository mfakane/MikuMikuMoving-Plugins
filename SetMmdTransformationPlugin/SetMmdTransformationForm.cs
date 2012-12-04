using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin
{
	public partial class SetMmdTransformationForm : Form
	{
		readonly IList<Process> mmds;

		public Process SelectedMmd
		{
			get
			{
				return mmds[mmdComboBox.SelectedIndex];
			}
		}

		public bool ChangedBonesOnly
		{
			get
			{
				return changedBonesOnlyCheckBox.Checked;
			}
		}

		public SetMmdTransformationForm(string language, IList<Process> mmds)
		{
			InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;
			this.mmds = mmds;

			mmdComboBox.Items.AddRange(mmds.Select(_ => "[PID: " + _.Id + "] " + GetProjectName(_)).Cast<object>().ToArray());
			mmdComboBox.SelectedIndex = 0;

			if (Util.IsEnglish(language))
			{
				this.Text = "Set Transformation to MMD";
				optionsLabel.Text = "Options";
				changedBonesOnlyCheckBox.Text = "Send changed bones only";
				okButton.Text = "Apply";
				cancelButton.Text = "Cancel";
			}
		}

		string GetProjectName(Process process)
		{
			var rt = process.MainWindowTitle;

			if (rt.Contains(" ["))
				return rt.Split(new[] { " [" }, 2, StringSplitOptions.None).Last().TrimEnd(']');
			else
				return "(無題のプロジェクト)";
		}
	}
}
