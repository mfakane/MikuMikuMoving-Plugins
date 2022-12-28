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
		modelListBox.Items.Clear();
		modelListBox.Items.AddRange(SelectedMmdInstance.Models.Select(x => Path.GetFileNameWithoutExtension(x.FileName)).Cast<object>().ToArray());
		modelListBox.SelectedIndex = 0;
		instanceLabel.Text = SelectedMmdInstance.Process.ProcessName;
		OnSelectedModelChanged(e);
	}

	void modelListBox_SelectedIndexChanged(object sender, EventArgs e) =>
		OnSelectedModelChanged(e);

	void GetMmdTransformationForm_Load(object sender, EventArgs e) =>
		OnSelectedModelChanged(e);

	void OnSelectedModelChanged(EventArgs e) =>
		SelectedModelChanged?.Invoke(this, e);
}