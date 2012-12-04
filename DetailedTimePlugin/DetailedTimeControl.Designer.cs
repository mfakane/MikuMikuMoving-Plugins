namespace Linearstar.MikuMikuMoving.DetailedTimePlugin
{
	partial class DetailedTimeControl
	{
		/// <summary> 
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region コンポーネント デザイナーで生成されたコード

		/// <summary> 
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.beatsPerMinuteNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.beginFrameNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.beatsPerMeasureNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.resolutionComboBox = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.beatsPerMinuteNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beginFrameNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beatsPerMeasureNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.beatsPerMinuteNumericUpDown, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.beginFrameNumericUpDown, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.beatsPerMeasureNumericUpDown, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.resolutionComboBox, 1, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(8);
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(169, 117);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(11, 92);
			this.label4.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(63, 12);
			this.label4.TabIndex = 6;
			this.label4.Text = "分解能 (&R):";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label3.Location = new System.Drawing.Point(11, 65);
			this.label3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "拍子 (&M):";
			// 
			// beatsPerMinuteNumericUpDown
			// 
			this.beatsPerMinuteNumericUpDown.DecimalPlaces = 1;
			this.beatsPerMinuteNumericUpDown.Location = new System.Drawing.Point(97, 35);
			this.beatsPerMinuteNumericUpDown.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.beatsPerMinuteNumericUpDown.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            0});
			this.beatsPerMinuteNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.beatsPerMinuteNumericUpDown.Name = "beatsPerMinuteNumericUpDown";
			this.beatsPerMinuteNumericUpDown.Size = new System.Drawing.Size(64, 19);
			this.beatsPerMinuteNumericUpDown.TabIndex = 3;
			this.beatsPerMinuteNumericUpDown.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(11, 38);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(31, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "&BPM:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(11, 11);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "開始フレーム(&F):";
			// 
			// beginFrameNumericUpDown
			// 
			this.beginFrameNumericUpDown.Location = new System.Drawing.Point(97, 8);
			this.beginFrameNumericUpDown.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.beginFrameNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.beginFrameNumericUpDown.Name = "beginFrameNumericUpDown";
			this.beginFrameNumericUpDown.Size = new System.Drawing.Size(64, 19);
			this.beginFrameNumericUpDown.TabIndex = 1;
			// 
			// beatsPerMeasureNumericUpDown
			// 
			this.beatsPerMeasureNumericUpDown.Location = new System.Drawing.Point(97, 62);
			this.beatsPerMeasureNumericUpDown.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.beatsPerMeasureNumericUpDown.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
			this.beatsPerMeasureNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.beatsPerMeasureNumericUpDown.Name = "beatsPerMeasureNumericUpDown";
			this.beatsPerMeasureNumericUpDown.Size = new System.Drawing.Size(64, 19);
			this.beatsPerMeasureNumericUpDown.TabIndex = 5;
			this.beatsPerMeasureNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			// 
			// resolutionComboBox
			// 
			this.resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.resolutionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.resolutionComboBox.FormattingEnabled = true;
			this.resolutionComboBox.Items.AddRange(new object[] {
            "24",
            "48",
            "96",
            "120",
            "192",
            "240",
            "384",
            "480",
            "960",
            "1920"});
			this.resolutionComboBox.Location = new System.Drawing.Point(97, 89);
			this.resolutionComboBox.Margin = new System.Windows.Forms.Padding(0);
			this.resolutionComboBox.MaxDropDownItems = 10;
			this.resolutionComboBox.Name = "resolutionComboBox";
			this.resolutionComboBox.Size = new System.Drawing.Size(64, 20);
			this.resolutionComboBox.TabIndex = 7;
			// 
			// DetailedTimeControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "DetailedTimeControl";
			this.Size = new System.Drawing.Size(169, 117);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.beatsPerMinuteNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beginFrameNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beatsPerMeasureNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown beginFrameNumericUpDown;
		private System.Windows.Forms.NumericUpDown beatsPerMinuteNumericUpDown;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown beatsPerMeasureNumericUpDown;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox resolutionComboBox;
	}
}
