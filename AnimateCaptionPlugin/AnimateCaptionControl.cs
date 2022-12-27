using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin;

public partial class AnimateCaptionControl : UserControl
{
	bool suppressValueChanged;
	
	public event EventHandler<ValueChangedEventArgs<FrameTime>>? CurrentTimeChanged;
	public event EventHandler<ValueChangedEventArgs<bool>>? IsAnimationEnabledChanged; 
	public event EventHandler<KeyFrameChangedEventArgs>? AddKeyFrame; 
	public event EventHandler<KeyFrameChangedEventArgs>? RemoveKeyFrame; 
	public event EventHandler<KeyFrameChangedEventArgs>? MoveKeyFrame;
	public event EventHandler<PropertyChangedEventArgs>? PropertyChanged;

	public FrameTime CurrentTime
	{
		get => timelineControl.CurrentTime;
		set => timelineControl.CurrentTime = value;
	}

	public AnimateCaptionControl()
	{
		InitializeComponent();
		Font = SystemFonts.MessageBoxFont;
		Dock = DockStyle.Top;
		SetViewEvent();
	}

	void SetViewEvent()
	{
		xEntry.ValueChanged += ValueChangedHandler(xEntry, CaptionPropertyKind.X);
		yEntry.ValueChanged += ValueChangedHandler(yEntry, CaptionPropertyKind.Y);
		alphaEntry.ValueChanged += ValueChangedHandler(alphaEntry, CaptionPropertyKind.Alpha);
		rotationEntry.ValueChanged += ValueChangedHandler(rotationEntry, CaptionPropertyKind.Rotation);
		fontSizeEntry.ValueChanged += ValueChangedHandler(fontSizeEntry, CaptionPropertyKind.FontSize);
		lineSpacingEntry.ValueChanged += ValueChangedHandler(lineSpacingEntry, CaptionPropertyKind.LineSpacing);
		letterSpacingEntry.ValueChanged += ValueChangedHandler(letterSpacingEntry, CaptionPropertyKind.LetterSpacing);
		shadowDistanceEntry.ValueChanged += ValueChangedHandler(shadowDistanceEntry, CaptionPropertyKind.ShadowDistance);

		EventHandler<EntryChangedEventArgs> ValueChangedHandler(
			AnimationEntryControl control,
			CaptionPropertyKind kind
		) =>
			(sender, e) =>
			{
				if (suppressValueChanged) return;
				suppressValueChanged = true;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(
					kind,
					e.Mode,
					e.EaseIn,
					e.EaseOut,
					e.IterationDuration,
					e.BeginValue,
					e.EndValue
				));
				suppressValueChanged = false;
			};
	}

	void SetPropertyToView(AnimationEntryControl control, AnimatedProperty property)
	{
		var pair = property.GetPairFromTime(CurrentTime);

		if (Math.Abs(control.BeginValue - pair.FromFrame.Value) > float.Epsilon)
			control.BeginValue = pair.FromFrame.Value;

		if (Math.Abs(control.EndValue - pair.ToFrame.Value) > float.Epsilon)
			control.EndValue = pair.ToFrame.Value;

		if (property.Mode != control.Mode)
			control.Mode = property.Mode;

		if (property.EaseIn != control.EaseIn)
			control.EaseIn = property.EaseIn;

		if (property.EaseOut != control.EaseOut)
			control.EaseOut = property.EaseOut;

		if (property.IterationDurationFrames != control.IterationDuration)
			control.IterationDuration = property.IterationDurationFrames;
	}

	void SetViewToProperty(AnimationEntryControl control, AnimatedProperty property)
	{
		if (property.Mode != control.Mode)
			property.Mode = control.Mode;

		if (property.EaseIn != control.EaseIn)
			property.EaseIn = control.EaseIn;

		if (property.EaseOut != control.EaseOut)
			property.EaseOut = control.EaseOut;

		if (property.IterationDurationFrames != control.IterationDuration)
			property.IterationDurationFrames = control.IterationDuration;

		property.SetPairValue(
			CurrentTime,
			control.BeginValue,
			property.Mode == AnimationMode.None ? control.BeginValue : control.EndValue
		);
	}

	void UpdateTimelineView()
	{
		beginLabel.Text = CurrentTime.StartFrame.ToString("0.00");
		endLabel.Text = (CurrentTime.StartFrame + CurrentTime.DurationFrames).ToString("0.00");
		timelineControl.CurrentTime = CurrentTime;
	}

	void timelineControl_CurrentTimeChanged(object sender, ValueChangedEventArgs<FrameTime> e)
	{
		CurrentTimeChanged?.Invoke(this, e);
		UpdateTimelineView();
	}

	void timelineControl_AddKeyFrame(object sender, KeyFrameChangedEventArgs e)
	{
		AddKeyFrame?.Invoke(this, e);
		UpdateTimelineView();
	}

	void timelineControl_RemoveKeyFrame(object sender, KeyFrameChangedEventArgs e)
	{
		RemoveKeyFrame?.Invoke(this, e);
		UpdateTimelineView();
	}

	void timelineControl_MoveKeyFrame(object sender, KeyFrameChangedEventArgs e)
	{
		MoveKeyFrame?.Invoke(this, e);
		UpdateTimelineView();
	}
	
	void enabledCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		IsAnimationEnabledChanged?.Invoke(this, new ValueChangedEventArgs<bool>(enabledCheckBox.Checked));
	}

	public void OnFrameUpdated(float currentFrame, ICaption? selectedCaption, AnimationData? animation)
	{
		if (selectedCaption == null) return;
		
		timelineControl.CurrentTime = selectedCaption.GetTimeFromFrame(currentFrame);

		if (animation != null)
		{
			SetAnimationToView(animation);
		}
	}

	public void OnEnabled()
	{
		SuspendLayout();
		inactiveLabel.Text = "字幕が選択されていません。";
		ResumeLayout();
	}

	public void OnDisabled()
	{
		SuspendLayout();
		activePanel.Visible = false;
		inactiveLabel.Visible = true;
		inactiveLabel.Text = "字幕アニメーションは現在有効ではありません。";
		ResumeLayout();
	}

	public void SetAnimationToView(AnimationData? animation)
	{
		if (animation == null) return;

		suppressValueChanged = true;

		timelineControl.KeyFrames = animation.X.KeyFrames.Keys.ToArray();
		
		SetPropertyToView(xEntry, animation.X);
		SetPropertyToView(yEntry, animation.Y);
		SetPropertyToView(alphaEntry, animation.Alpha);
		SetPropertyToView(rotationEntry, animation.Rotation);
		SetPropertyToView(fontSizeEntry, animation.FontSize);
		SetPropertyToView(lineSpacingEntry, animation.LineSpacing);
		SetPropertyToView(letterSpacingEntry, animation.LetterSpacing);
		SetPropertyToView(shadowDistanceEntry, animation.ShadowDistance);

		suppressValueChanged = false;
	}

	public void OnCaptionSelected(FrameTime currentTime, AnimationData? animation)
	{
		SuspendLayout();
		inactiveLabel.Visible = false;
		activePanel.Visible = true;
		enabledCheckBox.Checked = animation != null;
		parametersPanel.Visible = animation != null;
		CurrentTime = currentTime;
		UpdateTimelineView();
		SetAnimationToView(animation);
		ResumeLayout();
	}

	public void OnCaptionDeselected()
	{
		SuspendLayout();
		activePanel.Visible = false;
		inactiveLabel.Visible = true;
		ResumeLayout();
	}

	public void OnEnableCaptionAnimation(AnimationData? animation)
	{
		SuspendLayout();
		parametersPanel.Visible = true;
		SetAnimationToView(animation);
		ResumeLayout();
	}

	public void OnDisableCaptionAnimation()
	{
		SuspendLayout();
		parametersPanel.Visible = false;
		ResumeLayout();
	}

	public void KeyFramesChanged(AnimationData animation)
	{
		timelineControl.KeyFrames = animation.X.KeyFrames.Keys.ToArray();
	}
}