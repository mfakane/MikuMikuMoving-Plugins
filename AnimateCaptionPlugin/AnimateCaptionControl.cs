using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin
{
	public partial class AnimateCaptionControl : UserControl
	{
		ICaption caption;
		AnimationData animationData;
		double currentFrame;
		bool changing = false;
		Action updates = null;

		public Scene Scene
		{
			get;
			set;
		}

		public double CurrentFrame
		{
			get
			{
				return currentFrame;
			}
			set
			{
				currentFrame = value;
				UpdateUIValues();
			}
		}

		public AnimationData AnimationData
		{
			get
			{
				return animationData;
			}
			set
			{
				animationData = value;
			}
		}

		public bool IsAnimationEnabled
		{
			get
			{
				return enabledCheckBox.Checked;
			}
			set
			{
				enabledCheckBox.Checked = value;
			}
		}

		public bool IsPluginEnabled
		{
			get
			{
				return activePanel.Visible;
			}
			set
			{
				inactiveLabel.Visible = !(activePanel.Visible = value);
			}
		}

		public ICaption Caption
		{
			get
			{
				return caption;
			}
			set
			{
				caption = value;
			}
		}

		public AnimateCaptionControl()
		{
			InitializeComponent();
			this.Font = SystemFonts.MessageBoxFont;
			this.Dock = DockStyle.Top;
		}

		void GetValueSet(AnimationEntryControl control, AnimationEntry entry)
		{
			var pair = entry.GetBeginEndFramePair(this.Caption.StartFrame, this.Caption.DurationFrame, this.CurrentFrame);
			var valueSet = new[] { pair.First().Value, pair.Last().Value };

			if (control.BeginValue != valueSet[0])
				control.BeginValue = valueSet[0];

			if (control.EndValue != valueSet[1])
				control.EndValue = valueSet[1];

			if (entry.Mode != control.Mode)
				control.Mode = entry.Mode;

			if (entry.EaseIn != control.EaseIn)
				control.EaseIn = entry.EaseIn;

			if (entry.EaseOut != control.EaseOut)
				control.EaseOut = entry.EaseOut;

			if (entry.IterationDuration != control.IterationDuration)
				control.IterationDuration = entry.IterationDuration;
		}

		void SetValueSet(AnimationEntryControl control, AnimationEntry entry)
		{
			if (entry.Mode != control.Mode)
				entry.Mode = control.Mode;

			if (entry.EaseIn != control.EaseIn)
				entry.EaseIn = control.EaseIn;

			if (entry.EaseOut != control.EaseOut)
				entry.EaseOut = control.EaseOut;

			if (entry.IterationDuration != control.IterationDuration)
				entry.IterationDuration = control.IterationDuration;

			entry.SetBeginEndFramePair(this.Caption.StartFrame, this.Caption.DurationFrame, this.CurrentFrame, new[] { control.BeginValue, entry.Mode == AnimationMode.None ? control.BeginValue : control.EndValue });
		}

		void UpdateUIValues()
		{
			if (activePanel.Enabled = this.Caption != null)
			{
				beginLabel.Text = this.Caption.StartFrame.ToString("0.00");
				endLabel.Text = (this.Caption.StartFrame + this.Caption.DurationFrame).ToString("0.00");
				timelineControl.CurrentAmount = (float)((this.CurrentFrame - this.Caption.StartFrame) / this.Caption.DurationFrame);
			}

			if (updates != null)
				updates();

			updates = null;

			if (this.AnimationData != null)
				timelineControl.Amounts = this.AnimationData.X.Frames.Select(_ => _.FrameAmount).ToArray();
			else
				timelineControl.Amounts = null;

			timelineControl.TryUpdate();

			if (parametersPanel.Enabled = this.Caption != null && this.AnimationData != null && this.IsAnimationEnabled)
				if (!changing)
					using (FinallyBlock.Create(changing = true, _ => changing = false))
					{
						GetValueSet(xEntry, this.AnimationData.X);
						GetValueSet(yEntry, this.AnimationData.Y);
						GetValueSet(alphaEntry, this.AnimationData.Alpha);
						GetValueSet(rotationEntry, this.AnimationData.Rotation);
						GetValueSet(fontSizeEntry, this.AnimationData.FontSize);
						GetValueSet(lineSpacingEntry, this.AnimationData.LineSpacing);
						GetValueSet(letterSpacingEntry, this.AnimationData.LetterSpacing);
						GetValueSet(shadowDistanceEntry, this.AnimationData.ShadowDistance);
					}
		}

		public void SetNewValues()
		{
			if (!changing)
				using (FinallyBlock.Create(changing = true, _ => changing = false))
					if (this.Caption != null && this.AnimationData != null)
					{
						SetValueSet(xEntry, this.AnimationData.X);
						SetValueSet(yEntry, this.AnimationData.Y);
						SetValueSet(alphaEntry, this.AnimationData.Alpha);
						SetValueSet(rotationEntry, this.AnimationData.Rotation);
						SetValueSet(fontSizeEntry, this.AnimationData.FontSize);
						SetValueSet(lineSpacingEntry, this.AnimationData.LineSpacing);
						SetValueSet(letterSpacingEntry, this.AnimationData.LetterSpacing);
						SetValueSet(shadowDistanceEntry, this.AnimationData.ShadowDistance);
					}
		}

		void timelineControl_CurrentAmountChanged(object sender, EventArgs e)
		{
			this.Scene.MarkerPosition = (int)(this.Caption.StartFrame + this.Caption.DurationFrame * timelineControl.CurrentAmount);
		}

		void timelineControl_AddAmount(object sender, AmountEventArgs e)
		{
			updates += () => this.AnimationData.AddFrame(this.Caption, this.Caption.StartFrame + this.Caption.DurationFrame * timelineControl.CurrentAmount);
		}

		void timelineControl_RemoveAmount(object sender, AmountEventArgs e)
		{
			updates += () => this.AnimationData.RemoveFrame(e.Index);
		}

		void timelineControl_ChangeAmount(object sender, AmountEventArgs e)
		{
			updates += () => this.AnimationData.MoveFrame(e.Index, e.Amount);
		}
	}
}
