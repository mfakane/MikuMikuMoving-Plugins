using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin
{
	partial class GetMmdTransformationForm : Form
	{
		readonly IList<MmdImport> mmds;

		public event EventHandler SelectedModelChanged;

		public MmdImport SelectedMmd
		{
			get
			{
				return mmds[mmdComboBox.SelectedIndex];
			}
		}

		public MmdModel SelectedModel
		{
			get
			{
				return this.SelectedMmd.Models.Any() ? this.SelectedMmd.Models[modelListBox.SelectedIndex] : null;
			}
		}

		public GetMmdTransformationForm(string language, IList<MmdImport> mmds)
		{
			InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;
			this.mmds = mmds;

			mmdComboBox.Items.AddRange(mmds.Select(_ => "[PID: " + _.Process.Id + "] " + GetProjectName(_.Process)).Cast<object>().ToArray());
			mmdComboBox.SelectedIndex = 0;

			if (Util.IsEnglish(language))
			{
				this.Text = "Get Transformation from MMD";
				modelLabel.Text = "Model";
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

		void mmdComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			modelListBox.Items.Clear();
			modelListBox.Items.AddRange(this.SelectedMmd.Models.Select(_ => Path.GetFileNameWithoutExtension(_.FileName)).Cast<object>().ToArray());
			modelListBox.SelectedIndex = 0;
			OnSelectedModelChanged(e);
		}

		void modelListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSelectedModelChanged(e);
		}

		void MmdPoseRecieveForm_Load(object sender, EventArgs e)
		{
			OnSelectedModelChanged(e);
		}

		protected void OnSelectedModelChanged(EventArgs e)
		{
			if (this.SelectedModelChanged != null)
				this.SelectedModelChanged(this, e);
		}

		void recieveTimer_Tick(object sender, EventArgs e)
		{
			OnSelectedModelChanged(e);
		}
	}
}
