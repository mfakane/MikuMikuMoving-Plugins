namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin
{
	partial class SetMmdTransformationForm
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
			this.outerContainerPanel = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.innerContainerPanel = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.optionsLabel = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.mmdComboBox = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.changedBonesOnlyCheckBox = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.outerContainerPanel.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.innerContainerPanel.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
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
			this.outerContainerPanel.Size = new System.Drawing.Size(304, 171);
			this.outerContainerPanel.TabIndex = 3;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.okButton);
			this.flowLayoutPanel1.Controls.Add(this.cancelButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(138, 140);
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
			this.okButton.Text = "設定";
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
			// innerContainerPanel
			// 
			this.innerContainerPanel.AutoSize = true;
			this.innerContainerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.innerContainerPanel.BackColor = System.Drawing.SystemColors.Window;
			this.innerContainerPanel.ColumnCount = 1;
			this.innerContainerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 228F));
			this.innerContainerPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.innerContainerPanel.Controls.Add(this.panel2, 0, 2);
			this.innerContainerPanel.Controls.Add(this.mmdComboBox, 0, 1);
			this.innerContainerPanel.Controls.Add(this.panel1, 0, 0);
			this.innerContainerPanel.Controls.Add(this.changedBonesOnlyCheckBox, 0, 5);
			this.innerContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.innerContainerPanel.ForeColor = System.Drawing.SystemColors.WindowText;
			this.innerContainerPanel.Location = new System.Drawing.Point(0, 0);
			this.innerContainerPanel.Margin = new System.Windows.Forms.Padding(0);
			this.innerContainerPanel.Name = "innerContainerPanel";
			this.innerContainerPanel.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
			this.innerContainerPanel.RowCount = 6;
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.innerContainerPanel.Size = new System.Drawing.Size(304, 131);
			this.innerContainerPanel.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.AutoSize = true;
			this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel2.Controls.Add(this.optionsLabel);
			this.panel2.Controls.Add(this.groupBox2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(8, 59);
			this.panel2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(288, 15);
			this.panel2.TabIndex = 2;
			// 
			// optionsLabel
			// 
			this.optionsLabel.AutoSize = true;
			this.optionsLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.optionsLabel.Location = new System.Drawing.Point(0, 3);
			this.optionsLabel.Name = "optionsLabel";
			this.optionsLabel.Size = new System.Drawing.Size(48, 12);
			this.optionsLabel.TabIndex = 0;
			this.optionsLabel.Text = "オプション";
			// 
			// groupBox2
			// 
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox2.Location = new System.Drawing.Point(0, 11);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(288, 4);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			// 
			// mmdComboBox
			// 
			this.mmdComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mmdComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mmdComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.mmdComboBox.FormattingEnabled = true;
			this.mmdComboBox.Location = new System.Drawing.Point(8, 31);
			this.mmdComboBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.mmdComboBox.Name = "mmdComboBox";
			this.mmdComboBox.Size = new System.Drawing.Size(288, 20);
			this.mmdComboBox.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(8, 8);
			this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(288, 15);
			this.panel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(0, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "MikuMikuDance";
			// 
			// groupBox1
			// 
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox1.Location = new System.Drawing.Point(0, 11);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 4);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// changedBonesOnlyCheckBox
			// 
			this.changedBonesOnlyCheckBox.AutoSize = true;
			this.changedBonesOnlyCheckBox.Checked = true;
			this.changedBonesOnlyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.changedBonesOnlyCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.changedBonesOnlyCheckBox.Location = new System.Drawing.Point(8, 82);
			this.changedBonesOnlyCheckBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.changedBonesOnlyCheckBox.Name = "changedBonesOnlyCheckBox";
			this.changedBonesOnlyCheckBox.Size = new System.Drawing.Size(194, 17);
			this.changedBonesOnlyCheckBox.TabIndex = 3;
			this.changedBonesOnlyCheckBox.Text = "変形されているボーンのみ設定する";
			this.changedBonesOnlyCheckBox.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(222)))), ((int)(((byte)(222)))));
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(3, 131);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(298, 1);
			this.label5.TabIndex = 1;
			// 
			// SetMmdTransformationForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(304, 171);
			this.Controls.Add(this.outerContainerPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetMmdTransformationForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MMD へポーズを設定";
			this.outerContainerPanel.ResumeLayout(false);
			this.outerContainerPanel.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.innerContainerPanel.ResumeLayout(false);
			this.innerContainerPanel.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel outerContainerPanel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TableLayoutPanel innerContainerPanel;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label optionsLabel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox mmdComboBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox changedBonesOnlyCheckBox;
	}
}