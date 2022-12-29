using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Mmd;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin;

public partial class SetMmdTransformationForm : Form
{
	readonly IReadOnlyList<MmdDropTarget> mmdInstances;

	public MmdDropTarget SelectedMmdInstance => mmdInstances[mmdComboBox.SelectedIndex];

	public bool ChangedBonesOnly => changedBonesOnlyCheckBox.Checked;

	public SetMmdTransformationForm(string language, IReadOnlyList<MmdDropTarget> mmdInstances)
	{
		InitializeComponent();
		Font = SystemFonts.MessageBoxFont;
		this.mmdInstances = mmdInstances;

		mmdComboBox.Items.AddRange(mmdInstances.Select(x => "[PID: " + x.Id + "] " + x.ProjectName).Cast<object>().ToArray());
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

	void mmdComboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		instanceLabel.Text = SelectedMmdInstance.ProcessName;
	}
}