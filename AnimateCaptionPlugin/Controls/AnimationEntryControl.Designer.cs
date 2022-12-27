namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls
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
            this.endNumericUpDown = new Linearstar.MikuMikuMoving.Framework.IncrementSelectionNumericUpDown();
            this.endTrackBar = new System.Windows.Forms.TrackBar();
            this.beginTrackBar = new System.Windows.Forms.TrackBar();
            this.beginNumericUpDown = new Linearstar.MikuMikuMoving.Framework.IncrementSelectionNumericUpDown();
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
            this.leftPanel = new System.Windows.Forms.Panel();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.middlePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.endNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beginTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beginNumericUpDown)).BeginInit();
            this.modeContextMenuStrip.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.rightPanel.SuspendLayout();
            this.middlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // endNumericUpDown
            // 
            this.endNumericUpDown.DecimalPlaces = 2;
            this.endNumericUpDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.endNumericUpDown.Location = new System.Drawing.Point(36, 0);
            this.endNumericUpDown.Name = "endNumericUpDown";
            this.endNumericUpDown.Size = new System.Drawing.Size(64, 19);
            this.endNumericUpDown.TabIndex = 4;
            this.endNumericUpDown.ValueChanged += new System.EventHandler(this.endNumericUpDown_ValueChanged);
            // 
            // endTrackBar
            // 
            this.endTrackBar.AutoSize = false;
            this.endTrackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endTrackBar.Location = new System.Drawing.Point(0, 0);
            this.endTrackBar.Maximum = 100;
            this.endTrackBar.Name = "endTrackBar";
            this.endTrackBar.Size = new System.Drawing.Size(36, 24);
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
            this.beginTrackBar.Maximum = 100;
            this.beginTrackBar.Name = "beginTrackBar";
            this.beginTrackBar.Size = new System.Drawing.Size(36, 24);
            this.beginTrackBar.SmallChange = 10;
            this.beginTrackBar.TabIndex = 0;
            this.beginTrackBar.TickFrequency = 0;
            this.beginTrackBar.Value = 5;
            this.beginTrackBar.ValueChanged += new System.EventHandler(this.beginNumericUpDown_ValueChanged);
            // 
            // beginNumericUpDown
            // 
            this.beginNumericUpDown.DecimalPlaces = 2;
            this.beginNumericUpDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.beginNumericUpDown.Location = new System.Drawing.Point(36, 0);
            this.beginNumericUpDown.Name = "beginNumericUpDown";
            this.beginNumericUpDown.Size = new System.Drawing.Size(64, 19);
            this.beginNumericUpDown.TabIndex = 1;
            this.beginNumericUpDown.ValueChanged += new System.EventHandler(this.beginNumericUpDown_ValueChanged);
            // 
            // switchButton
            // 
            this.switchButton.AutoSize = true;
            this.switchButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.switchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.switchButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.switchButton.Location = new System.Drawing.Point(8, 0);
            this.switchButton.MinimumSize = new System.Drawing.Size(75, 0);
            this.switchButton.Name = "switchButton";
            this.switchButton.Size = new System.Drawing.Size(75, 23);
            this.switchButton.TabIndex = 2;
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
            this.modeContextMenuStrip.Size = new System.Drawing.Size(181, 258);
            // 
            // noneMenuItem
            // 
            this.noneMenuItem.Checked = true;
            this.noneMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noneMenuItem.Name = "noneMenuItem";
            this.noneMenuItem.Size = new System.Drawing.Size(180, 22);
            this.noneMenuItem.Text = "なし";
            this.noneMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
            // 
            // linearInterpolationMenuItem
            // 
            this.linearInterpolationMenuItem.Name = "linearInterpolationMenuItem";
            this.linearInterpolationMenuItem.Size = new System.Drawing.Size(180, 22);
            this.linearInterpolationMenuItem.Text = "直線移動";
            this.linearInterpolationMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
            // 
            // noInterpolationMenuItem
            // 
            this.noInterpolationMenuItem.Name = "noInterpolationMenuItem";
            this.noInterpolationMenuItem.Size = new System.Drawing.Size(180, 22);
            this.noInterpolationMenuItem.Text = "瞬間移動";
            this.noInterpolationMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
            // 
            // linearInterpolationFirstAndLastOnlyMenuItem
            // 
            this.linearInterpolationFirstAndLastOnlyMenuItem.Name = "linearInterpolationFirstAndLastOnlyMenuItem";
            this.linearInterpolationFirstAndLastOnlyMenuItem.Size = new System.Drawing.Size(180, 22);
            this.linearInterpolationFirstAndLastOnlyMenuItem.Text = "中間点無視";
            this.linearInterpolationFirstAndLastOnlyMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
            // 
            // byAccelerationMenuItem
            // 
            this.byAccelerationMenuItem.Name = "byAccelerationMenuItem";
            this.byAccelerationMenuItem.Size = new System.Drawing.Size(180, 22);
            this.byAccelerationMenuItem.Text = "移動量指定";
            this.byAccelerationMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
            // 
            // randomFirstAndLastMenuItem
            // 
            this.randomFirstAndLastMenuItem.Name = "randomFirstAndLastMenuItem";
            this.randomFirstAndLastMenuItem.Size = new System.Drawing.Size(180, 22);
            this.randomFirstAndLastMenuItem.Text = "ランダム移動";
            this.randomFirstAndLastMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
            // 
            // repeatFirstAndLastMenuItem
            // 
            this.repeatFirstAndLastMenuItem.Name = "repeatFirstAndLastMenuItem";
            this.repeatFirstAndLastMenuItem.Size = new System.Drawing.Size(180, 22);
            this.repeatFirstAndLastMenuItem.Text = "反復移動";
            this.repeatFirstAndLastMenuItem.Click += new System.EventHandler(this.noneMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(177, 6);
            // 
            // easeInMenuItem
            // 
            this.easeInMenuItem.CheckOnClick = true;
            this.easeInMenuItem.Enabled = false;
            this.easeInMenuItem.Name = "easeInMenuItem";
            this.easeInMenuItem.Size = new System.Drawing.Size(180, 22);
            this.easeInMenuItem.Text = "加速";
            this.easeInMenuItem.CheckedChanged += new System.EventHandler(this.easeInMenuItem_CheckedChanged);
            // 
            // easeOutMenuItem
            // 
            this.easeOutMenuItem.CheckOnClick = true;
            this.easeOutMenuItem.Enabled = false;
            this.easeOutMenuItem.Name = "easeOutMenuItem";
            this.easeOutMenuItem.Size = new System.Drawing.Size(180, 22);
            this.easeOutMenuItem.Text = "減速";
            this.easeOutMenuItem.CheckedChanged += new System.EventHandler(this.easeInMenuItem_CheckedChanged);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(177, 6);
            // 
            // intervalMenuItem
            // 
            this.intervalMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.intervalTextBox});
            this.intervalMenuItem.Enabled = false;
            this.intervalMenuItem.Name = "intervalMenuItem";
            this.intervalMenuItem.Size = new System.Drawing.Size(180, 22);
            this.intervalMenuItem.Text = "間隔";
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Size = new System.Drawing.Size(100, 23);
            this.intervalTextBox.Text = "0";
            this.intervalTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.intervalTextBox_KeyPress);
            this.intervalTextBox.TextChanged += new System.EventHandler(this.intervalTextBox_TextChanged);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.beginTrackBar);
            this.leftPanel.Controls.Add(this.beginNumericUpDown);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Margin = new System.Windows.Forms.Padding(0);
            this.leftPanel.MinimumSize = new System.Drawing.Size(100, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(100, 24);
            this.leftPanel.TabIndex = 3;
            // 
            // rightPanel
            // 
            this.rightPanel.Controls.Add(this.endTrackBar);
            this.rightPanel.Controls.Add(this.endNumericUpDown);
            this.rightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightPanel.Enabled = false;
            this.rightPanel.Location = new System.Drawing.Point(191, 0);
            this.rightPanel.Margin = new System.Windows.Forms.Padding(0);
            this.rightPanel.MinimumSize = new System.Drawing.Size(100, 0);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(100, 24);
            this.rightPanel.TabIndex = 4;
            // 
            // middlePanel
            // 
            this.middlePanel.AutoSize = true;
            this.middlePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.middlePanel.Controls.Add(this.switchButton);
            this.middlePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.middlePanel.Location = new System.Drawing.Point(100, 0);
            this.middlePanel.MaximumSize = new System.Drawing.Size(91, 23);
            this.middlePanel.Name = "middlePanel";
            this.middlePanel.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.middlePanel.Size = new System.Drawing.Size(91, 23);
            this.middlePanel.TabIndex = 5;
            // 
            // AnimationEntryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.middlePanel);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.leftPanel);
            this.MinimumSize = new System.Drawing.Size(0, 23);
            this.Name = "AnimationEntryControl";
            this.Size = new System.Drawing.Size(291, 24);
            this.Resize += new System.EventHandler(this.AnimationEntryControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.endNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beginTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beginNumericUpDown)).EndInit();
            this.modeContextMenuStrip.ResumeLayout(false);
            this.leftPanel.ResumeLayout(false);
            this.rightPanel.ResumeLayout(false);
            this.middlePanel.ResumeLayout(false);
            this.middlePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private Linearstar.MikuMikuMoving.Framework.IncrementSelectionNumericUpDown endNumericUpDown;
		private System.Windows.Forms.TrackBar endTrackBar;
		private System.Windows.Forms.TrackBar beginTrackBar;
		private Linearstar.MikuMikuMoving.Framework.IncrementSelectionNumericUpDown beginNumericUpDown;
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
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel middlePanel;
    }
}
