namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	partial class AnimationEntryControl
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
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.endNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.endTrackBar = new System.Windows.Forms.TrackBar();
			this.beginTrackBar = new System.Windows.Forms.TrackBar();
			this.beginNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.switchButton = new System.Windows.Forms.Button();
			this.modeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.noneMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.linearInterpolationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.noInterpolationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.linearInterpolationFirstAndLastOnlyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.byAccelerationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.randomFirstAndLastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.repeatFirstAndLastMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.easeInMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.easeOutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.intervalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.intervalTextBox = new System.Windows.Forms.ToolStripTextBox();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.endNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.endTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beginTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.beginNumericUpDown)).BeginInit();
			this.modeContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.endNumericUpDown, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.endTrackBar, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.beginTrackBar, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.beginNumericUpDown, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.switchButton, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(521, 27);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// endNumericUpDown
			// 
			this.endNumericUpDown.DecimalPlaces = 2;
			this.endNumericUpDown.Location = new System.Drawing.Point(457, 2);
			this.endNumericUpDown.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.endNumericUpDown.Name = "endNumericUpDown";
			this.endNumericUpDown.Size = new System.Drawing.Size(64, 19);
			this.endNumericUpDown.TabIndex = 4;
			this.endNumericUpDown.ValueChanged += new System.EventHandler(this.endNumericUpDown_ValueChanged);
			// 
			// endTrackBar
			// 
			this.endTrackBar.AutoSize = false;
			this.endTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.endTrackBar.Location = new System.Drawing.Point(306, 0);
			this.endTrackBar.Margin = new System.Windows.Forms.Padding(0);
			this.endTrackBar.Maximum = 100;
			this.endTrackBar.Name = "endTrackBar";
			this.endTrackBar.Size = new System.Drawing.Size(151, 27);
			this.endTrackBar.SmallChange = 10;
			this.endTrackBar.TabIndex = 3;
			this.endTrackBar.TickFrequency = 0;
			this.endTrackBar.Value = 5;
			this.endTrackBar.ValueChanged += new System.EventHandler(this.endNumericUpDown_ValueChanged);
			// 
			// beginTrackBar
			// 
			this.beginTrackBar.AutoSize = false;
			this.beginTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.beginTrackBar.Location = new System.Drawing.Point(0, 0);
			this.beginTrackBar.Margin = new System.Windows.Forms.Padding(0);
			this.beginTrackBar.Maximum = 100;
			this.beginTrackBar.Name = "beginTrackBar";
			this.beginTrackBar.Size = new System.Drawing.Size(151, 27);
			this.beginTrackBar.SmallChange = 10;
			this.beginTrackBar.TabIndex = 0;
			this.beginTrackBar.TickFrequency = 0;
			this.beginTrackBar.Value = 5;
			this.beginTrackBar.ValueChanged += new System.EventHandler(this.beginNumericUpDown_ValueChanged);
			// 
			// beginNumericUpDown
			// 
			this.beginNumericUpDown.DecimalPlaces = 2;
			this.beginNumericUpDown.Location = new System.Drawing.Point(151, 2);
			this.beginNumericUpDown.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.beginNumericUpDown.Name = "beginNumericUpDown";
			this.beginNumericUpDown.Size = new System.Drawing.Size(64, 19);
			this.beginNumericUpDown.TabIndex = 1;
			this.beginNumericUpDown.ValueChanged += new System.EventHandler(this.beginNumericUpDown_ValueChanged);
			// 
			// switchButton
			// 
			this.switchButton.AutoSize = true;
			this.switchButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.switchButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.switchButton.Location = new System.Drawing.Point(223, 0);
			this.switchButton.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.switchButton.MinimumSize = new System.Drawing.Size(75, 23);
			this.switchButton.Name = "switchButton";
			this.switchButton.Size = new System.Drawing.Size(75, 23);
			this.switchButton.TabIndex = 2;
			this.switchButton.Text = "button1";
			this.switchButton.UseVisualStyleBackColor = true;
			this.switchButton.Click += new System.EventHandler(this.switchButton_Click);
			// 
			// modeContextMenuStrip
			// 
			this.modeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneMenuItem,
            this.linearInterpolationMenuItem,
            this.noInterpolationMenuItem,
            this.linearInterpolationFirstAndLastOnlyMenuItem,
            this.byAccelerationMenuItem,
            this.randomFirstAndLastMenuItem,
            this.repeatFirstAndLastMenuItem,
            this.toolStripMenuItem1,
            this.easeInMenuItem,
            this.easeOutMenuItem,
            this.toolStripMenuItem2,
            this.intervalMenuItem});
			this.modeContextMenuStrip.Name = "contextMenuStrip1";
			this.modeContextMenuStrip.Size = new System.Drawing.Size(153, 258);
			// 
			// noneMenuItem
			// 
			this.noneMenuItem.Checked = true;
			this.noneMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.noneMenuItem.Name = "noneMenuItem";
			this.noneMenuItem.Size = new System.Drawing.Size(152, 22);
			this.noneMenuItem.Text = "なし";
			this.noneMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
			// 
			// linearInterpolationMenuItem
			// 
			this.linearInterpolationMenuItem.Name = "linearInterpolationMenuItem";
			this.linearInterpolationMenuItem.Size = new System.Drawing.Size(152, 22);
			this.linearInterpolationMenuItem.Text = "直線移動";
			this.linearInterpolationMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
			// 
			// noInterpolationMenuItem
			// 
			this.noInterpolationMenuItem.Name = "noInterpolationMenuItem";
			this.noInterpolationMenuItem.Size = new System.Drawing.Size(152, 22);
			this.noInterpolationMenuItem.Text = "瞬間移動";
			this.noInterpolationMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
			// 
			// linearInterpolationFirstAndLastOnlyMenuItem
			// 
			this.linearInterpolationFirstAndLastOnlyMenuItem.Name = "linearInterpolationFirstAndLastOnlyMenuItem";
			this.linearInterpolationFirstAndLastOnlyMenuItem.Size = new System.Drawing.Size(152, 22);
			this.linearInterpolationFirstAndLastOnlyMenuItem.Text = "中間点無視";
			this.linearInterpolationFirstAndLastOnlyMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
			// 
			// byAccelerationMenuItem
			// 
			this.byAccelerationMenuItem.Name = "byAccelerationMenuItem";
			this.byAccelerationMenuItem.Size = new System.Drawing.Size(152, 22);
			this.byAccelerationMenuItem.Text = "移動量指定";
			this.byAccelerationMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
			// 
			// randomFirstAndLastMenuItem
			// 
			this.randomFirstAndLastMenuItem.Name = "randomFirstAndLastMenuItem";
			this.randomFirstAndLastMenuItem.Size = new System.Drawing.Size(152, 22);
			this.randomFirstAndLastMenuItem.Text = "ランダム移動";
			this.randomFirstAndLastMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
			// 
			// repeatFirstAndLastMenuItem
			// 
			this.repeatFirstAndLastMenuItem.Name = "repeatFirstAndLastMenuItem";
			this.repeatFirstAndLastMenuItem.Size = new System.Drawing.Size(152, 22);
			this.repeatFirstAndLastMenuItem.Text = "反復移動";
			this.repeatFirstAndLastMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// easeInMenuItem
			// 
			this.easeInMenuItem.CheckOnClick = true;
			this.easeInMenuItem.Enabled = false;
			this.easeInMenuItem.Name = "easeInMenuItem";
			this.easeInMenuItem.Size = new System.Drawing.Size(152, 22);
			this.easeInMenuItem.Text = "加速";
			// 
			// easeOutMenuItem
			// 
			this.easeOutMenuItem.CheckOnClick = true;
			this.easeOutMenuItem.Enabled = false;
			this.easeOutMenuItem.Name = "easeOutMenuItem";
			this.easeOutMenuItem.Size = new System.Drawing.Size(152, 22);
			this.easeOutMenuItem.Text = "減速";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
			// 
			// intervalMenuItem
			// 
			this.intervalMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intervalTextBox});
			this.intervalMenuItem.Enabled = false;
			this.intervalMenuItem.Name = "intervalMenuItem";
			this.intervalMenuItem.Size = new System.Drawing.Size(152, 22);
			this.intervalMenuItem.Text = "間隔";
			// 
			// intervalTextBox
			// 
			this.intervalTextBox.Name = "intervalTextBox";
			this.intervalTextBox.Size = new System.Drawing.Size(100, 23);
			this.intervalTextBox.Text = "0";
			this.intervalTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.intervalTextBox_KeyPress);
			// 
			// AnimationEntryControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "AnimationEntryControl";
			this.Size = new System.Drawing.Size(521, 27);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.endNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.endTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beginTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.beginNumericUpDown)).EndInit();
			this.modeContextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.NumericUpDown endNumericUpDown;
		private System.Windows.Forms.TrackBar endTrackBar;
		private System.Windows.Forms.TrackBar beginTrackBar;
		private System.Windows.Forms.NumericUpDown beginNumericUpDown;
		private System.Windows.Forms.Button switchButton;
		private System.Windows.Forms.ContextMenuStrip modeContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem linearInterpolationMenuItem;
		private System.Windows.Forms.ToolStripMenuItem noInterpolationMenuItem;
		private System.Windows.Forms.ToolStripMenuItem linearInterpolationFirstAndLastOnlyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem byAccelerationMenuItem;
		private System.Windows.Forms.ToolStripMenuItem randomFirstAndLastMenuItem;
		private System.Windows.Forms.ToolStripMenuItem repeatFirstAndLastMenuItem;
		private System.Windows.Forms.ToolStripMenuItem noneMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem easeInMenuItem;
		private System.Windows.Forms.ToolStripMenuItem easeOutMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem intervalMenuItem;
		private System.Windows.Forms.ToolStripTextBox intervalTextBox;
	}
}
