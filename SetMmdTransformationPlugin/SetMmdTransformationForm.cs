using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin;

public partial class SetMmdTransformationForm : Form
{
	readonly IList<Process> mmdInstances;

	public Process SelectedMmdInstance => mmdInstances[mmdComboBox.SelectedIndex];

	public bool ChangedBonesOnly => changedBonesOnlyCheckBox.Checked;

	public SetMmdTransformationForm(string language, IList<Process> mmdInstances)
	{
		InitializeComponent();
		Font = SystemFonts.MessageBoxFont;
		this.mmdInstances = mmdInstances;

		mmdComboBox.Items.AddRange(mmdInstances.Select(x => "[PID: " + x.Id + "] " + GetProjectName(x)).Cast<object>().ToArray());
		mmdComboBox.SelectedIndex = 0;

		if (language != "ja")
		{
			Text = "Set Transformation to MMD";
			optionsLabel.Text = "Options";
			changedBonesOnlyCheckBox.Text = "Send changed bones only";
			okButton.Text = "Apply";
			cancelButton.Text = "Cancel";
		}
	}

	static string GetProjectName(Process process)
	{
		var title = process.MainWindowTitle;
		var beginIndex = title.IndexOf(" [", StringComparison.Ordinal);
		var endIndex = title.LastIndexOf("]", StringComparison.Ordinal);

		if (beginIndex == -1 || endIndex == -1) return "(無題のプロジェクト)";
		
		beginIndex += 2;
		
		var fileName = title.Substring(beginIndex, endIndex - beginIndex);

		if (fileName.Contains(Path.DirectorySeparatorChar))
			fileName = Path.GetFileName(fileName);
			
		return fileName;
	}

	void mmdComboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		instanceLabel.Text = SelectedMmdInstance.ProcessName;
	}
}