namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	partial class TimelineControl
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
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMenuItem,
            this.removeMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(207, 48);
			// 
			// addMenuItem
			// 
			this.addMenuItem.Name = "addMenuItem";
			this.addMenuItem.Size = new System.Drawing.Size(206, 22);
			this.addMenuItem.Text = "現在地点に中間点を追加";
			this.addMenuItem.Click += new System.EventHandler(this.addMenuItem_Click);
			// 
			// removeMenuItem
			// 
			this.removeMenuItem.Enabled = false;
			this.removeMenuItem.Name = "removeMenuItem";
			this.removeMenuItem.Size = new System.Drawing.Size(206, 22);
			this.removeMenuItem.Text = "現在の中間点を削除";
			this.removeMenuItem.Click += new System.EventHandler(this.removeMenuItem_Click);
			// 
			// TimelineControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ContextMenuStrip = this.contextMenuStrip1;
			this.MinimumSize = new System.Drawing.Size(16, 16);
			this.Name = "TimelineControl";
			this.Size = new System.Drawing.Size(16, 16);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.TimelineControl_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TimelineControl_MouseUp);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem addMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeMenuItem;
	}
}
