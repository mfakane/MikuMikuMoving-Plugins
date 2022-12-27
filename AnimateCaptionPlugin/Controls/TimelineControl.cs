using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;

public partial class TimelineControl : UserControl
{
	FrameTime currentTime;
	int? draggingIndex;
	IReadOnlyList<float> keyFrames = Array.Empty<float>();

	public event EventHandler<ValueChangedEventArgs<FrameTime>>? CurrentTimeChanged;
	public event EventHandler<KeyFrameChangedEventArgs>? AddKeyFrame;
	public event EventHandler<KeyFrameChangedEventArgs>? RemoveKeyFrame;
	public event EventHandler<KeyFrameChangedEventArgs>? MoveKeyFrame;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IReadOnlyList<float> KeyFrames
	{
		get => keyFrames;
		set
		{
			if (keyFrames.SequenceEqual(value)) return;

			keyFrames = value;
			removeMenuItem.Enabled = value.Count > 2;
			Invalidate();
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public FrameTime CurrentTime
	{
		get => currentTime;
		set
		{
			if (currentTime == value) return;
			
			currentTime = value;
			CurrentTimeChanged?.Invoke(this, new(CurrentTime));
			Invalidate();
		}
	}

	public TimelineControl()
	{
		InitializeComponent();
		DoubleBuffered = true;
		SetStyle(ControlStyles.UserPaint, true);
		SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		SetStyle(ControlStyles.ResizeRedraw, true);
	}
	
	void TimelineControl_Paint(object sender, PaintEventArgs e)
	{
		var g = e.Graphics;
		var rect = ClientRectangle;
		var widthMinusOne = rect.Width - 1;
		var halfHeight = rect.Height / 2;
		var barRect = new Rectangle(0, halfHeight, widthMinusOne, rect.Height - halfHeight - 1);

		g.FillRectangle(SystemBrushes.Window, barRect);
		g.DrawRectangle(SystemPens.ControlDarkDark, barRect);

		var previousPoint = Point.Empty;
		var currentAmount = CurrentTime.Progress;

		for (var i = 0; i < KeyFrames.Count; i++)
		{
			var amount = KeyFrames[i];
			var pt = new Point((int)(widthMinusOne * amount), 0);

			if (currentAmount >= 1 &&
			    i == KeyFrames.Count - 1 ||
			    i > 0 &&
			    currentAmount >= KeyFrames[i - 1] &&
			    currentAmount < amount)
				g.FillRectangle(SystemBrushes.Highlight, previousPoint.X + 1, barRect.Y + 1, pt.X - previousPoint.X - 1, barRect.Height - 1);

			g.DrawLine(SystemPens.ControlDarkDark, pt, pt + new Size(0, rect.Height));
			previousPoint = pt;
		}

		{
			var pt = new Point((int)(widthMinusOne * currentAmount), 0);

			g.DrawLine(SystemPens.Highlight, pt, pt + new Size(0, rect.Height));
		}
	}

	void TimelineControl_MouseDown(object sender, MouseEventArgs e)
	{
		var idx = GetKeyFrameIndexAtX(e.X);

		if (idx == 0 || idx == KeyFrames.Count - 1)
			idx = null;

		if (idx != null)
			Cursor = Cursors.SizeWE;

		draggingIndex = idx;
	}

	void TimelineControl_MouseUp(object sender, MouseEventArgs e)
	{
		draggingIndex = null;
		Cursor = DefaultCursor;
	}

	void TimelineControl_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			var newProgress = MathHelper.Clamp((float)e.X / Width, 0f, 1);
			
			if (draggingIndex is { } index)
			{
				MoveKeyFrame?.Invoke(this, new(index, newProgress));
			}
			else
			{
				CurrentTime = CurrentTime.AtProgress(newProgress);
			}
		}
		else
		{
			var idx = GetKeyFrameIndexAtX(e.X);

			if (idx == 0 || idx == KeyFrames.Count - 1)
				idx = null;

			Cursor = idx == null ? DefaultCursor : Cursors.SizeWE;
		}
	}

	int? GetKeyFrameIndexAtX(int mouseX) =>
		KeyFrames.Select((x, idx) => new
			{
				Index = idx,
				X = Width * x,
			})
			.Where(x => mouseX >= x.X - 3 && mouseX <= x.X + 3)
			.Select(x => (int?)x.Index)
			.FirstOrDefault();

	int GetKeyFrameIndexAtProgress(float progress)
	{
		for (var i = 0; i < KeyFrames.Count; i++)
		{
			if (KeyFrames[i] > progress) return i - 1;
		}

		return KeyFrames.Count - 1;
	}

	void addMenuItem_Click(object sender, EventArgs e) => 
		AddKeyFrame?.Invoke(this, new(-1, CurrentTime.Progress));

	void removeMenuItem_Click(object sender, EventArgs e)
	{
		var index = GetKeyFrameIndexAtProgress(CurrentTime.Progress);

		RemoveKeyFrame?.Invoke(this, new(index, CurrentTime.Progress));
	}
}