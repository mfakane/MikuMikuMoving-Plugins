using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin;

partial class GetMmdTransformationForm : Form
{
	readonly IList<MmdImport> mmdInstances;

	public event EventHandler? SelectedModelChanged;

	public MmdImport SelectedMmdInstance => mmdInstances[mmdComboBox.SelectedIndex];

	public MmdModel? SelectedModel => SelectedMmdInstance.Models.ElementAtOrDefault(modelListBox.SelectedIndex);

	public GetMmdTransformationForm(string language, IList<MmdImport> mmdInstances)
	{
		InitializeComponent();
		Font = SystemFonts.MessageBoxFont;
		this.mmdInstances = mmdInstances;

		mmdComboBox.Items.AddRange(mmdInstances.Select(x => "[PID: " + x.Process.Id + "] " + GetProjectName(x.Process)).Cast<object>().ToArray());
		mmdComboBox.SelectedIndex = 0;

		if (language != "ja")
		{
			Text = "Get Transformation from MMD";
			modelLabel.Text = "Model";
			okButton.Text = "Apply";
			cancelButton.Text = "Cancel";
		}
	}

	static string GetProjectName(Process process)
	{
		var rt = process.MainWindowTitle;

		if (rt.Contains(" ["))
			return rt.Split(new[] { " [" }, 2, StringSplitOptions.None).Last().TrimEnd(']');
		else
			return "(無題のプロジェクト)";
	}

	void mmdComboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		modelListBox.Items.Clear();
		modelListBox.Items.AddRange(SelectedMmdInstance.Models.Select(_ => Path.GetFileNameWithoutExtension(_.FileName)).Cast<object>().ToArray());
		modelListBox.SelectedIndex = 0;
		OnSelectedModelChanged(e);
	}

	void modelListBox_SelectedIndexChanged(object sender, EventArgs e) =>
		OnSelectedModelChanged(e);

	void GetMmdTransformationForm_Load(object sender, EventArgs e) =>
		OnSelectedModelChanged(e);

	void OnSelectedModelChanged(EventArgs e) =>
		SelectedModelChanged?.Invoke(this, e);
}