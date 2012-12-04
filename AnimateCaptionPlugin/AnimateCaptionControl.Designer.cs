namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	partial class AnimateCaptionControl
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
			this.activePanel = new System.Windows.Forms.TableLayoutPanel();
			this.parametersPanel = new System.Windows.Forms.Panel();
			this.shadowDistanceEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.letterSpacingEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.lineSpacingEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.fontSizeEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.rotationEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.alphaEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.yEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.xEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationEntryControl();
			this.enabledCheckBox = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.beginLabel = new System.Windows.Forms.Label();
			this.timelineControl = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.TimelineControl();
			this.inactiveLabel = new System.Windows.Forms.Label();
			this.endLabel = new System.Windows.Forms.Label();
			this.activePanel.SuspendLayout();
			this.parametersPanel.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// activePanel
			// 
			this.activePanel.AutoSize = true;
			this.activePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.activePanel.ColumnCount = 1;
			this.activePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.activePanel.Controls.Add(this.parametersPanel, 0, 2);
			this.activePanel.Controls.Add(this.enabledCheckBox, 0, 0);
			this.activePanel.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.activePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.activePanel.Location = new System.Drawing.Point(8, 8);
			this.activePanel.Name = "activePanel";
			this.activePanel.RowCount = 3;
			this.activePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.activePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.activePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.activePanel.Size = new System.Drawing.Size(366, 277);
			this.activePanel.TabIndex = 0;
			// 
			// parametersPanel
			// 
			this.parametersPanel.AutoSize = true;
			this.parametersPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.parametersPanel.Controls.Add(this.shadowDistanceEntry);
			this.parametersPanel.Controls.Add(this.letterSpacingEntry);
			this.parametersPanel.Controls.Add(this.lineSpacingEntry);
			this.parametersPanel.Controls.Add(this.fontSizeEntry);
			this.parametersPanel.Controls.Add(this.rotationEntry);
			this.parametersPanel.Controls.Add(this.alphaEntry);
			this.parametersPanel.Controls.Add(this.yEntry);
			this.parametersPanel.Controls.Add(this.xEntry);
			this.parametersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parametersPanel.Location = new System.Drawing.Point(0, 49);
			this.parametersPanel.Margin = new System.Windows.Forms.Padding(0);
			this.parametersPanel.Name = "parametersPanel";
			this.parametersPanel.Size = new System.Drawing.Size(366, 228);
			this.parametersPanel.TabIndex = 2;
			// 
			// shadowDistanceEntry
			// 
			this.shadowDistanceEntry.AutoSize = true;
			this.shadowDistanceEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.shadowDistanceEntry.BeginValue = 0F;
			this.shadowDistanceEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.shadowDistanceEntry.EaseIn = false;
			this.shadowDistanceEntry.EaseOut = false;
			this.shadowDistanceEntry.EndValue = 0F;
			this.shadowDistanceEntry.IterationDuration = 0;
			this.shadowDistanceEntry.Location = new System.Drawing.Point(0, 189);
			this.shadowDistanceEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.shadowDistanceEntry.Name = "shadowDistanceEntry";
			this.shadowDistanceEntry.NumericMaximum = 100F;
			this.shadowDistanceEntry.NumericMinimum = -100F;
			this.shadowDistanceEntry.ParameterName = "影距離";
			this.shadowDistanceEntry.Size = new System.Drawing.Size(366, 27);
			this.shadowDistanceEntry.TabIndex = 7;
			this.shadowDistanceEntry.TrackMaximum = 100F;
			this.shadowDistanceEntry.TrackMinimum = -100F;
			// 
			// letterSpacingEntry
			// 
			this.letterSpacingEntry.AutoSize = true;
			this.letterSpacingEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.letterSpacingEntry.BeginValue = 0F;
			this.letterSpacingEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.letterSpacingEntry.EaseIn = false;
			this.letterSpacingEntry.EaseOut = false;
			this.letterSpacingEntry.EndValue = 0F;
			this.letterSpacingEntry.IterationDuration = 0;
			this.letterSpacingEntry.Location = new System.Drawing.Point(0, 162);
			this.letterSpacingEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.letterSpacingEntry.Name = "letterSpacingEntry";
			this.letterSpacingEntry.NumericMaximum = 100F;
			this.letterSpacingEntry.NumericMinimum = -100F;
			this.letterSpacingEntry.ParameterName = "字間";
			this.letterSpacingEntry.Size = new System.Drawing.Size(366, 27);
			this.letterSpacingEntry.TabIndex = 6;
			this.letterSpacingEntry.TrackMaximum = 100F;
			this.letterSpacingEntry.TrackMinimum = -100F;
			// 
			// lineSpacingEntry
			// 
			this.lineSpacingEntry.AutoSize = true;
			this.lineSpacingEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.lineSpacingEntry.BeginValue = 0F;
			this.lineSpacingEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.lineSpacingEntry.EaseIn = false;
			this.lineSpacingEntry.EaseOut = false;
			this.lineSpacingEntry.EndValue = 0F;
			this.lineSpacingEntry.IterationDuration = 0;
			this.lineSpacingEntry.Location = new System.Drawing.Point(0, 135);
			this.lineSpacingEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.lineSpacingEntry.Name = "lineSpacingEntry";
			this.lineSpacingEntry.NumericMaximum = 100F;
			this.lineSpacingEntry.NumericMinimum = -100F;
			this.lineSpacingEntry.ParameterName = "行間";
			this.lineSpacingEntry.Size = new System.Drawing.Size(366, 27);
			this.lineSpacingEntry.TabIndex = 5;
			this.lineSpacingEntry.TrackMaximum = 100F;
			this.lineSpacingEntry.TrackMinimum = -100F;
			// 
			// fontSizeEntry
			// 
			this.fontSizeEntry.AutoSize = true;
			this.fontSizeEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.fontSizeEntry.BeginValue = 0F;
			this.fontSizeEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.fontSizeEntry.EaseIn = false;
			this.fontSizeEntry.EaseOut = false;
			this.fontSizeEntry.EndValue = 0F;
			this.fontSizeEntry.IterationDuration = 0;
			this.fontSizeEntry.Location = new System.Drawing.Point(0, 108);
			this.fontSizeEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.fontSizeEntry.Name = "fontSizeEntry";
			this.fontSizeEntry.NumericMaximum = 100F;
			this.fontSizeEntry.NumericMinimum = 0F;
			this.fontSizeEntry.ParameterName = "サイズ";
			this.fontSizeEntry.Size = new System.Drawing.Size(366, 27);
			this.fontSizeEntry.TabIndex = 4;
			this.fontSizeEntry.TrackMaximum = 100F;
			this.fontSizeEntry.TrackMinimum = 0F;
			// 
			// rotationEntry
			// 
			this.rotationEntry.AutoSize = true;
			this.rotationEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.rotationEntry.BeginValue = 0F;
			this.rotationEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.rotationEntry.EaseIn = false;
			this.rotationEntry.EaseOut = false;
			this.rotationEntry.EndValue = 0F;
			this.rotationEntry.IterationDuration = 0;
			this.rotationEntry.Location = new System.Drawing.Point(0, 81);
			this.rotationEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.rotationEntry.Name = "rotationEntry";
			this.rotationEntry.NumericMaximum = 360F;
			this.rotationEntry.NumericMinimum = -360F;
			this.rotationEntry.ParameterName = "角度";
			this.rotationEntry.Size = new System.Drawing.Size(366, 27);
			this.rotationEntry.TabIndex = 3;
			this.rotationEntry.TrackMaximum = 360F;
			this.rotationEntry.TrackMinimum = -360F;
			// 
			// alphaEntry
			// 
			this.alphaEntry.AutoSize = true;
			this.alphaEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.alphaEntry.BeginValue = 0F;
			this.alphaEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.alphaEntry.EaseIn = false;
			this.alphaEntry.EaseOut = false;
			this.alphaEntry.EndValue = 0F;
			this.alphaEntry.IterationDuration = 0;
			this.alphaEntry.Location = new System.Drawing.Point(0, 54);
			this.alphaEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.alphaEntry.Name = "alphaEntry";
			this.alphaEntry.NumericMaximum = 1F;
			this.alphaEntry.NumericMinimum = 0F;
			this.alphaEntry.ParameterName = "不透明度";
			this.alphaEntry.Size = new System.Drawing.Size(366, 27);
			this.alphaEntry.TabIndex = 2;
			this.alphaEntry.TrackMaximum = 1F;
			this.alphaEntry.TrackMinimum = 0F;
			// 
			// yEntry
			// 
			this.yEntry.AutoSize = true;
			this.yEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.yEntry.BeginValue = 0F;
			this.yEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.yEntry.EaseIn = false;
			this.yEntry.EaseOut = false;
			this.yEntry.EndValue = 0F;
			this.yEntry.IterationDuration = 0;
			this.yEntry.Location = new System.Drawing.Point(0, 27);
			this.yEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.yEntry.Name = "yEntry";
			this.yEntry.NumericMaximum = 1000F;
			this.yEntry.NumericMinimum = -1000F;
			this.yEntry.ParameterName = "Y";
			this.yEntry.Size = new System.Drawing.Size(366, 27);
			this.yEntry.TabIndex = 1;
			this.yEntry.TrackMaximum = 1000F;
			this.yEntry.TrackMinimum = 0F;
			// 
			// xEntry
			// 
			this.xEntry.AutoSize = true;
			this.xEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.xEntry.BeginValue = 0F;
			this.xEntry.Dock = System.Windows.Forms.DockStyle.Top;
			this.xEntry.EaseIn = false;
			this.xEntry.EaseOut = false;
			this.xEntry.EndValue = 0F;
			this.xEntry.IterationDuration = 0;
			this.xEntry.Location = new System.Drawing.Point(0, 0);
			this.xEntry.Mode = Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AnimationMode.None;
			this.xEntry.Name = "xEntry";
			this.xEntry.NumericMaximum = 1000F;
			this.xEntry.NumericMinimum = -1000F;
			this.xEntry.ParameterName = "X";
			this.xEntry.Size = new System.Drawing.Size(366, 27);
			this.xEntry.TabIndex = 0;
			this.xEntry.TrackMaximum = 1000F;
			this.xEntry.TrackMinimum = 0F;
			// 
			// enabledCheckBox
			// 
			this.enabledCheckBox.AutoSize = true;
			this.enabledCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.enabledCheckBox.Location = new System.Drawing.Point(0, 0);
			this.enabledCheckBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.enabledCheckBox.Name = "enabledCheckBox";
			this.enabledCheckBox.Size = new System.Drawing.Size(143, 17);
			this.enabledCheckBox.TabIndex = 0;
			this.enabledCheckBox.Text = "アニメーションを使用する";
			this.enabledCheckBox.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Controls.Add(this.beginLabel, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.endLabel, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.timelineControl, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 25);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(366, 16);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// beginLabel
			// 
			this.beginLabel.AutoSize = true;
			this.beginLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.beginLabel.Location = new System.Drawing.Point(6, 3);
			this.beginLabel.Margin = new System.Windows.Forms.Padding(6, 3, 0, 0);
			this.beginLabel.Name = "beginLabel";
			this.beginLabel.Size = new System.Drawing.Size(31, 12);
			this.beginLabel.TabIndex = 0;
			this.beginLabel.Text = "00.00";
			// 
			// timelineControl
			// 
			this.timelineControl.Amounts = null;
			this.timelineControl.AutoSize = true;
			this.timelineControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.SetColumnSpan(this.timelineControl, 2);
			this.timelineControl.CurrentAmount = 0F;
			this.timelineControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.timelineControl.Location = new System.Drawing.Point(37, 0);
			this.timelineControl.Margin = new System.Windows.Forms.Padding(0);
			this.timelineControl.MinimumSize = new System.Drawing.Size(16, 16);
			this.timelineControl.Name = "timelineControl";
			this.timelineControl.Size = new System.Drawing.Size(292, 16);
			this.timelineControl.TabIndex = 4;
			this.timelineControl.CurrentAmountChanged += new System.EventHandler(this.timelineControl_CurrentAmountChanged);
			this.timelineControl.AddAmount += new System.EventHandler<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AmountEventArgs>(this.timelineControl_AddAmount);
			this.timelineControl.RemoveAmount += new System.EventHandler<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AmountEventArgs>(this.timelineControl_RemoveAmount);
			this.timelineControl.ChangeAmount += new System.EventHandler<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.AmountEventArgs>(this.timelineControl_ChangeAmount);
			// 
			// inactiveLabel
			// 
			this.inactiveLabel.AutoSize = true;
			this.inactiveLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inactiveLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.inactiveLabel.Location = new System.Drawing.Point(8, 8);
			this.inactiveLabel.Name = "inactiveLabel";
			this.inactiveLabel.Size = new System.Drawing.Size(223, 12);
			this.inactiveLabel.TabIndex = 1;
			this.inactiveLabel.Text = "字幕アニメーションは現在有効ではありません。";
			this.inactiveLabel.Visible = false;
			// 
			// endLabel
			// 
			this.endLabel.AutoSize = true;
			this.endLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.endLabel.Location = new System.Drawing.Point(335, 3);
			this.endLabel.Margin = new System.Windows.Forms.Padding(6, 3, 0, 0);
			this.endLabel.Name = "endLabel";
			this.endLabel.Size = new System.Drawing.Size(31, 12);
			this.endLabel.TabIndex = 3;
			this.endLabel.Text = "00.00";
			// 
			// AnimateCaptionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.inactiveLabel);
			this.Controls.Add(this.activePanel);
			this.MinimumSize = new System.Drawing.Size(320, 0);
			this.Name = "AnimateCaptionControl";
			this.Padding = new System.Windows.Forms.Padding(8);
			this.Size = new System.Drawing.Size(382, 293);
			this.activePanel.ResumeLayout(false);
			this.activePanel.PerformLayout();
			this.parametersPanel.ResumeLayout(false);
			this.parametersPanel.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel activePanel;
		private System.Windows.Forms.CheckBox enabledCheckBox;
		private System.Windows.Forms.Panel parametersPanel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label beginLabel;
		private AnimationEntryControl xEntry;
		private AnimationEntryControl rotationEntry;
		private AnimationEntryControl alphaEntry;
		private AnimationEntryControl yEntry;
		private AnimationEntryControl fontSizeEntry;
		private AnimationEntryControl shadowDistanceEntry;
		private AnimationEntryControl letterSpacingEntry;
		private AnimationEntryControl lineSpacingEntry;
		private System.Windows.Forms.Label inactiveLabel;
		private TimelineControl timelineControl;
		private System.Windows.Forms.Label endLabel;
	}
}
