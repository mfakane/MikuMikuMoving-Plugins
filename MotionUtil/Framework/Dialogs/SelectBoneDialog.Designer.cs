namespace Linearstar.MikuMikuMoving.Framework
{
	partial class SelectBoneDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectBoneDialog));
			this.innerContainerPanel = new System.Windows.Forms.TableLayoutPanel();
			this.treeRadioButton = new System.Windows.Forms.RadioButton();
			this.panel2 = new System.Windows.Forms.Panel();
			this.optionsLabel = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.displayFrameRadioButton = new System.Windows.Forms.RadioButton();
			this.boneTreeView = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.outerContainerPanel = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.innerContainerPanel.SuspendLayout();
			this.panel2.SuspendLayout();
			this.outerContainerPanel.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// innerContainerPanel
			// 
			this.innerContainerPanel.AutoSize = true;
			this.innerContainerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.innerContainerPanel.BackColor = System.Drawing.SystemColors.Window;
			this.innerContainerPanel.ColumnCount = 1;
			this.innerContainerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.innerContainerPanel.Controls.Add(this.treeRadioButton, 0, 2);
			this.innerContainerPanel.Controls.Add(this.panel2, 0, 0);
			this.innerContainerPanel.Controls.Add(this.displayFrameRadioButton, 0, 1);
			this.innerContainerPanel.Controls.Add(this.boneTreeView, 0, 3);
			this.innerContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.innerContainerPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.innerContainerPanel.Location = new System.Drawing.Point(0, 0);
			this.innerContainerPanel.Margin = new System.Windows.Forms.Padding(0);
			this.innerContainerPanel.Name = "innerContainerPanel";
			this.innerContainerPanel.Padding = new System.Windows.Forms.Padding(8);
			this.innerContainerPanel.RowCount = 4;
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.innerContainerPanel.Size = new System.Drawing.Size(208, 322);
			this.innerContainerPanel.TabIndex = 0;
			// 
			// treeRadioButton
			// 
			this.treeRadioButton.AutoSize = true;
			this.treeRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.treeRadioButton.Location = new System.Drawing.Point(8, 48);
			this.treeRadioButton.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.treeRadioButton.Name = "treeRadioButton";
			this.treeRadioButton.Size = new System.Drawing.Size(117, 17);
			this.treeRadioButton.TabIndex = 5;
			this.treeRadioButton.Text = "親子関係から選ぶ";
			this.treeRadioButton.UseVisualStyleBackColor = true;
			this.treeRadioButton.CheckedChanged += new System.EventHandler(this.displayFrameRadioButton_CheckedChanged);
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.Controls.Add(this.optionsLabel);
			this.panel2.Controls.Add(this.groupBox2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(8, 8);
			this.panel2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(192, 15);
			this.panel2.TabIndex = 2;
			// 
			// optionsLabel
			// 
			this.optionsLabel.AutoSize = true;
			this.optionsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.optionsLabel.Location = new System.Drawing.Point(0, 3);
			this.optionsLabel.Name = "optionsLabel";
			this.optionsLabel.Size = new System.Drawing.Size(58, 12);
			this.optionsLabel.TabIndex = 0;
			this.optionsLabel.Text = "ボーンリスト";
			// 
			// groupBox2
			// 
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox2.Location = new System.Drawing.Point(0, 11);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(192, 4);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			// 
			// displayFrameRadioButton
			// 
			this.displayFrameRadioButton.AutoSize = true;
			this.displayFrameRadioButton.Checked = true;
			this.displayFrameRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.displayFrameRadioButton.Location = new System.Drawing.Point(8, 31);
			this.displayFrameRadioButton.Margin = new System.Windows.Forms.Padding(0);
			this.displayFrameRadioButton.Name = "displayFrameRadioButton";
			this.displayFrameRadioButton.Size = new System.Drawing.Size(105, 17);
			this.displayFrameRadioButton.TabIndex = 4;
			this.displayFrameRadioButton.TabStop = true;
			this.displayFrameRadioButton.Text = "表示枠から選ぶ";
			this.displayFrameRadioButton.UseVisualStyleBackColor = true;
			this.displayFrameRadioButton.CheckedChanged += new System.EventHandler(this.displayFrameRadioButton_CheckedChanged);
			// 
			// boneTreeView
			// 
			this.boneTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.boneTreeView.ImageIndex = 0;
			this.boneTreeView.ImageList = this.imageList1;
			this.boneTreeView.Location = new System.Drawing.Point(8, 73);
			this.boneTreeView.Margin = new System.Windows.Forms.Padding(0);
			this.boneTreeView.Name = "boneTreeView";
			this.boneTreeView.SelectedImageIndex = 0;
			this.boneTreeView.Size = new System.Drawing.Size(192, 241);
			this.boneTreeView.TabIndex = 6;
			this.boneTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.boneTreeView_AfterSelect);
			this.boneTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.boneTreeView_NodeMouseDoubleClick);
			// 
			// imageList1
			// 
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "DisplayList");
			this.imageList1.Images.SetKeyName(1, "Bone");
			this.imageList1.Images.SetKeyName(2, "Layer");
			// 
			// outerContainerPanel
			// 
			this.outerContainerPanel.AutoSize = true;
			this.outerContainerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.outerContainerPanel.ColumnCount = 1;
			this.outerContainerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.outerContainerPanel.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.outerContainerPanel.Controls.Add(this.innerContainerPanel, 0, 0);
			this.outerContainerPanel.Controls.Add(this.label5, 0, 1);
			this.outerContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outerContainerPanel.Location = new System.Drawing.Point(0, 0);
			this.outerContainerPanel.Name = "outerContainerPanel";
			this.outerContainerPanel.RowCount = 3;
			this.outerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.outerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.outerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.outerContainerPanel.Size = new System.Drawing.Size(208, 362);
			this.outerContainerPanel.TabIndex = 4;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.okButton);
			this.flowLayoutPanel1.Controls.Add(this.cancelButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(42, 331);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(8);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(158, 23);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// okButton
			// 
			this.okButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(0, 0);
			this.okButton.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
			this.okButton.MinimumSize = new System.Drawing.Size(75, 23);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "選択";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(83, 0);
			this.cancelButton.Margin = new System.Windows.Forms.Padding(0);
			this.cancelButton.MinimumSize = new System.Drawing.Size(75, 23);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "キャンセル";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(3, 322);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(202, 1);
			this.label5.TabIndex = 1;
			// 
			// SelectBoneDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(208, 362);
			this.Controls.Add(this.outerContainerPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectBoneDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ボーンを選択";
			this.Load += new System.EventHandler(this.SelectBoneDialog_Load);
			this.innerContainerPanel.ResumeLayout(false);
			this.innerContainerPanel.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.outerContainerPanel.ResumeLayout(false);
			this.outerContainerPanel.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel innerContainerPanel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label optionsLabel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel outerContainerPanel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton treeRadioButton;
		private System.Windows.Forms.RadioButton displayFrameRadioButton;
		private System.Windows.Forms.TreeView boneTreeView;
		private System.Windows.Forms.ImageList imageList1;

	}
}