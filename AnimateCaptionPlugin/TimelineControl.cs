using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public partial class TimelineControl : UserControl
	{
		IList<float> amounts;
		float currentAmount;
		int? draggingIndex;

		public event EventHandler CurrentAmountChanged;
		public event EventHandler<AmountEventArgs> AddAmount;
		public event EventHandler<AmountEventArgs> RemoveAmount;
		public event EventHandler<AmountEventArgs> ChangeAmount;

		public IList<float> Amounts
		{
			get;
			set;
		}

		public float CurrentAmount
		{
			get;
			set;
		}

		public TimelineControl()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		public void TryUpdate()
		{
			if (this.Amounts != null && amounts != null && !amounts.SequenceEqual(this.Amounts) ||
				currentAmount != this.CurrentAmount)
			{
				this.Invalidate();

				amounts = this.Amounts == null ? null : this.Amounts.ToArray();
				currentAmount = this.CurrentAmount;

				removeMenuItem.Enabled = this.Amounts != null && this.Amounts.Count > 2;
			}
		}

		void TimelineControl_Paint(object sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			var rect = this.ClientRectangle;
			var widthMinusOne = rect.Width - 1;
			var halfHeight = rect.Height / 2;
			var barRect = new Rectangle(0, halfHeight, widthMinusOne, rect.Height - halfHeight - 1);

			g.FillRectangle(SystemBrushes.Window, barRect);
			g.DrawRectangle(SystemPens.ControlDarkDark, barRect);

			if (this.Amounts != null)
			{
				var previousPoint = Point.Empty;

				for (var i = 0; i < this.Amounts.Count; i++)
				{
					var amount = this.Amounts[i];
					var pt = new Point((int)(widthMinusOne * amount), 0);

					if (this.CurrentAmount >= 1 &&
						i == this.Amounts.Count - 1 ||
						i > 0 &&
						this.CurrentAmount >= this.Amounts[i - 1] &&
						this.CurrentAmount < amount)
						g.FillRectangle(SystemBrushes.Highlight, previousPoint.X + 1, barRect.Y + 1, pt.X - previousPoint.X - 1, barRect.Height - 1);

					g.DrawLine(SystemPens.ControlDarkDark, pt, pt + new Size(0, rect.Height));
					previousPoint = pt;
				}
			}

			{
				var pt = new Point((int)(widthMinusOne * this.CurrentAmount), 0);

				g.DrawLine(SystemPens.Highlight, pt, pt + new Size(0, rect.Height));
			}
		}

		void TimelineControl_MouseDown(object sender, MouseEventArgs e)
		{
			var idx = GetUnderlyingAmountIndex(e.X);

			if (idx == 0 || idx == this.Amounts.Count - 1)
				idx = null;

			if (idx != null)
				this.Cursor = Cursors.SizeWE;

			draggingIndex = idx;
		}

		void TimelineControl_MouseUp(object sender, MouseEventArgs e)
		{
			draggingIndex = null;
			this.Cursor = this.DefaultCursor;
		}

		void TimelineControl_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
				if (draggingIndex != null)
				{
					if (ChangeAmount != null)
						ChangeAmount(this, new AmountEventArgs
						{
							Index = draggingIndex.Value,
							Amount = (float)(Math.Max(0, Math.Min(e.X, this.Width))) / this.Width,
						});
				}
				else
				{
					this.CurrentAmount = (float)(Math.Max(0, Math.Min(e.X, this.Width))) / this.Width;

					if (CurrentAmountChanged != null)
						CurrentAmountChanged(this, EventArgs.Empty);
				}
			else if (this.Amounts != null)
			{
				var idx = GetUnderlyingAmountIndex(e.X);

				if (idx == 0 || idx == this.Amounts.Count - 1)
					idx = null;

				this.Cursor = idx == null ? this.DefaultCursor : Cursors.SizeWE;
			}
		}

		int? GetUnderlyingAmountIndex(int x)
		{
			if (this.Amounts == null)
				return null;

			return this.Amounts.Select((_, idx) => new
			{
				Index = idx,
				X = this.Width * _,
			})
			.Where(_ => x >= _.X - 3 && x <= _.X + 3)
			.Select(_ => (int?)_.Index)
			.FirstOrDefault();
		}

		void addMenuItem_Click(object sender, EventArgs e)
		{
			if (AddAmount != null)
				AddAmount(this, new AmountEventArgs
				{
					Amount = this.CurrentAmount,
				});
		}

		void removeMenuItem_Click(object sender, EventArgs e)
		{
			if (RemoveAmount != null)
				RemoveAmount(this, new AmountEventArgs
				{
					Amount = this.Amounts.Last(_ => _ > this.CurrentAmount),
					Index = this.Amounts.Select((_, idx) => new
					{
						Amount = _,
						Index = idx,
					})
					.Where(_ => _.Amount > this.CurrentAmount)
					.Select(_ => _.Index)
					.Last(),
				});
		}
	}
}
