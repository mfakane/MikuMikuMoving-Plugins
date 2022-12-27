using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;

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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.shadowDistanceEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.letterSpacingEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.lineSpacingEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.fontSizeEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.rotationEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.alphaEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.yEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.xEntry = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.AnimationEntryControl();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.beginLabel = new System.Windows.Forms.Label();
            this.endLabel = new System.Windows.Forms.Label();
            this.timelineControl = new Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.TimelineControl();
            this.inactiveLabel = new System.Windows.Forms.Label();
            this.activePanel = new System.Windows.Forms.Panel();
            this.parametersPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.activePanel.SuspendLayout();
            this.parametersPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // shadowDistanceEntry
            // 
            this.shadowDistanceEntry.AutoSize = true;
            this.shadowDistanceEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.shadowDistanceEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.shadowDistanceEntry.Location = new System.Drawing.Point(0, 161);
            this.shadowDistanceEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.shadowDistanceEntry.Name = "shadowDistanceEntry";
            this.shadowDistanceEntry.NumericMaximum = 100F;
            this.shadowDistanceEntry.NumericMinimum = 0F;
            this.shadowDistanceEntry.ParameterName = "影距離";
            this.shadowDistanceEntry.Size = new System.Drawing.Size(304, 23);
            this.shadowDistanceEntry.TabIndex = 7;
            this.shadowDistanceEntry.TrackMaximum = 100F;
            this.shadowDistanceEntry.TrackMinimum = 0F;
            // 
            // letterSpacingEntry
            // 
            this.letterSpacingEntry.AutoSize = true;
            this.letterSpacingEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.letterSpacingEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.letterSpacingEntry.Location = new System.Drawing.Point(0, 138);
            this.letterSpacingEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.letterSpacingEntry.Name = "letterSpacingEntry";
            this.letterSpacingEntry.NumericMaximum = 100F;
            this.letterSpacingEntry.NumericMinimum = -100F;
            this.letterSpacingEntry.ParameterName = "字間";
            this.letterSpacingEntry.Size = new System.Drawing.Size(304, 23);
            this.letterSpacingEntry.TabIndex = 6;
            this.letterSpacingEntry.TrackMaximum = 100F;
            this.letterSpacingEntry.TrackMinimum = -100F;
            // 
            // lineSpacingEntry
            // 
            this.lineSpacingEntry.AutoSize = true;
            this.lineSpacingEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.lineSpacingEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.lineSpacingEntry.Location = new System.Drawing.Point(0, 115);
            this.lineSpacingEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.lineSpacingEntry.Name = "lineSpacingEntry";
            this.lineSpacingEntry.NumericMaximum = 100F;
            this.lineSpacingEntry.NumericMinimum = -100F;
            this.lineSpacingEntry.ParameterName = "行間";
            this.lineSpacingEntry.Size = new System.Drawing.Size(304, 23);
            this.lineSpacingEntry.TabIndex = 5;
            this.lineSpacingEntry.TrackMaximum = 100F;
            this.lineSpacingEntry.TrackMinimum = -100F;
            // 
            // fontSizeEntry
            // 
            this.fontSizeEntry.AutoSize = true;
            this.fontSizeEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fontSizeEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.fontSizeEntry.Location = new System.Drawing.Point(0, 92);
            this.fontSizeEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.fontSizeEntry.Name = "fontSizeEntry";
            this.fontSizeEntry.NumericMaximum = 1024F;
            this.fontSizeEntry.NumericMinimum = 0F;
            this.fontSizeEntry.ParameterName = "サイズ";
            this.fontSizeEntry.Size = new System.Drawing.Size(304, 23);
            this.fontSizeEntry.TabIndex = 4;
            this.fontSizeEntry.TrackMaximum = 512F;
            this.fontSizeEntry.TrackMinimum = 0F;
            // 
            // rotationEntry
            // 
            this.rotationEntry.AutoSize = true;
            this.rotationEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rotationEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.rotationEntry.Location = new System.Drawing.Point(0, 69);
            this.rotationEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.rotationEntry.Name = "rotationEntry";
            this.rotationEntry.NumericMaximum = 360F;
            this.rotationEntry.NumericMinimum = -360F;
            this.rotationEntry.ParameterName = "角度";
            this.rotationEntry.Size = new System.Drawing.Size(304, 23);
            this.rotationEntry.TabIndex = 3;
            this.rotationEntry.TrackMaximum = 360F;
            this.rotationEntry.TrackMinimum = -360F;
            // 
            // alphaEntry
            // 
            this.alphaEntry.AutoSize = true;
            this.alphaEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.alphaEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.alphaEntry.Location = new System.Drawing.Point(0, 46);
            this.alphaEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.alphaEntry.Name = "alphaEntry";
            this.alphaEntry.NumericMaximum = 100F;
            this.alphaEntry.NumericMinimum = 0F;
            this.alphaEntry.ParameterName = "不透明度";
            this.alphaEntry.Size = new System.Drawing.Size(304, 23);
            this.alphaEntry.TabIndex = 2;
            this.alphaEntry.TrackMaximum = 1F;
            this.alphaEntry.TrackMinimum = 0F;
            // 
            // yEntry
            // 
            this.yEntry.AutoSize = true;
            this.yEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.yEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.yEntry.Location = new System.Drawing.Point(0, 23);
            this.yEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.yEntry.Name = "yEntry";
            this.yEntry.NumericMaximum = 2000F;
            this.yEntry.NumericMinimum = -1000F;
            this.yEntry.ParameterName = "Y";
            this.yEntry.Size = new System.Drawing.Size(304, 23);
            this.yEntry.TabIndex = 1;
            this.yEntry.TrackMaximum = 1280F;
            this.yEntry.TrackMinimum = 0F;
            // 
            // xEntry
            // 
            this.xEntry.AutoSize = true;
            this.xEntry.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.xEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.xEntry.Location = new System.Drawing.Point(0, 0);
            this.xEntry.MinimumSize = new System.Drawing.Size(0, 23);
            this.xEntry.Name = "xEntry";
            this.xEntry.NumericMaximum = 2000F;
            this.xEntry.NumericMinimum = -1000F;
            this.xEntry.ParameterName = "X";
            this.xEntry.Size = new System.Drawing.Size(304, 23);
            this.xEntry.TabIndex = 0;
            this.xEntry.TrackMaximum = 1280F;
            this.xEntry.TrackMinimum = 0F;
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.enabledCheckBox.Location = new System.Drawing.Point(0, 0);
            this.enabledCheckBox.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(304, 16);
            this.enabledCheckBox.TabIndex = 0;
            this.enabledCheckBox.Text = "アニメーションを使用する";
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            this.enabledCheckBox.CheckedChanged += new System.EventHandler(this.enabledCheckBox_CheckedChanged);
            // 
            // beginLabel
            // 
            this.beginLabel.AutoSize = true;
            this.beginLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.beginLabel.Location = new System.Drawing.Point(0, 8);
            this.beginLabel.Margin = new System.Windows.Forms.Padding(6, 3, 0, 0);
            this.beginLabel.Name = "beginLabel";
            this.beginLabel.Size = new System.Drawing.Size(31, 12);
            this.beginLabel.TabIndex = 0;
            this.beginLabel.Text = "00.00";
            // 
            // endLabel
            // 
            this.endLabel.AutoSize = true;
            this.endLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.endLabel.Location = new System.Drawing.Point(273, 8);
            this.endLabel.Margin = new System.Windows.Forms.Padding(6, 3, 0, 0);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(31, 12);
            this.endLabel.TabIndex = 3;
            this.endLabel.Text = "00.00";
            // 
            // timelineControl
            // 
            this.timelineControl.AutoSize = true;
            this.timelineControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.timelineControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timelineControl.Location = new System.Drawing.Point(31, 8);
            this.timelineControl.Margin = new System.Windows.Forms.Padding(0);
            this.timelineControl.MinimumSize = new System.Drawing.Size(16, 16);
            this.timelineControl.Name = "timelineControl";
            this.timelineControl.Size = new System.Drawing.Size(242, 16);
            this.timelineControl.TabIndex = 4;
            this.timelineControl.CurrentTimeChanged += new System.EventHandler<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.ValueChangedEventArgs<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation.FrameTime>>(this.timelineControl_CurrentTimeChanged);
            this.timelineControl.AddKeyFrame += new System.EventHandler<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.KeyFrameChangedEventArgs>(this.timelineControl_AddKeyFrame);
            this.timelineControl.RemoveKeyFrame += new System.EventHandler<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.KeyFrameChangedEventArgs>(this.timelineControl_RemoveKeyFrame);
            this.timelineControl.MoveKeyFrame += new System.EventHandler<Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls.KeyFrameChangedEventArgs>(this.timelineControl_MoveKeyFrame);
            // 
            // inactiveLabel
            // 
            this.inactiveLabel.AutoSize = true;
            this.inactiveLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inactiveLabel.Location = new System.Drawing.Point(8, 8);
            this.inactiveLabel.Name = "inactiveLabel";
            this.inactiveLabel.Size = new System.Drawing.Size(223, 12);
            this.inactiveLabel.TabIndex = 1;
            this.inactiveLabel.Text = "字幕アニメーションは現在有効ではありません。";
            // 
            // activePanel
            // 
            this.activePanel.AutoSize = true;
            this.activePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.activePanel.Controls.Add(this.parametersPanel);
            this.activePanel.Controls.Add(this.panel2);
            this.activePanel.Controls.Add(this.enabledCheckBox);
            this.activePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.activePanel.Location = new System.Drawing.Point(8, 8);
            this.activePanel.Name = "activePanel";
            this.activePanel.Size = new System.Drawing.Size(304, 244);
            this.activePanel.TabIndex = 2;
            this.activePanel.Visible = false;
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
            this.parametersPanel.Location = new System.Drawing.Point(0, 48);
            this.parametersPanel.Margin = new System.Windows.Forms.Padding(0);
            this.parametersPanel.Name = "parametersPanel";
            this.parametersPanel.Size = new System.Drawing.Size(304, 196);
            this.parametersPanel.TabIndex = 2;
            this.parametersPanel.Visible = false;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.timelineControl);
            this.panel2.Controls.Add(this.endLabel);
            this.panel2.Controls.Add(this.beginLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 16);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panel2.Size = new System.Drawing.Size(304, 32);
            this.panel2.TabIndex = 1;
            // 
            // AnimateCaptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.activePanel);
            this.Controls.Add(this.inactiveLabel);
            this.MinimumSize = new System.Drawing.Size(320, 0);
            this.Name = "AnimateCaptionControl";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(320, 260);
            this.activePanel.ResumeLayout(false);
            this.activePanel.PerformLayout();
            this.parametersPanel.ResumeLayout(false);
            this.parametersPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.CheckBox enabledCheckBox;
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
        private System.Windows.Forms.Panel activePanel;
        private System.Windows.Forms.Panel parametersPanel;
        private System.Windows.Forms.Panel panel2;
    }
}
