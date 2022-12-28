using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;
using Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin;

[UsedImplicitly]
public class AnimateCaptionPlugin : ResidentBase, IHaveUserControl, ICanSavePlugin
{
	readonly AnimateCaptionControl control = new();
	Animator? animator;
	SceneState? state;

	Animator Animator => animator ??= new Animator(Scene);
	
	AnimateCaptionControl? Control => Scene.State == MikuMikuPlugin.SceneState.Editing ? control : null;
	
	public override void Update(float currentFrame, float elapsedTime)
	{
		if (currentFrame == 0)
			currentFrame = Scene.MarkerPosition;

		var newState = SceneState.FromScene(Scene, currentFrame);

		if (state is { SelectedCaption: { } selectedCaption })
		{
			// 前回は何かを選択していた
			
			if (newState.SelectedICaption is { } newSelectedCaption)
			{
				// 今回も何かを選択している

				if (selectedCaption.Index == newSelectedCaption.GetIndex())
				{
					// 前回と同じものを選択し続けてる
				
					var selectedCaptionProperties = CaptionProperties.FromCaption(selectedCaption);
					var newSelectedCaptionProperties = CaptionProperties.FromCaption(newSelectedCaption);
					var difference = selectedCaptionProperties.GetDifference(newSelectedCaptionProperties);
				
					if (difference >= 1)
					{
						var animation = Animator.OnCaptionChanged(newSelectedCaption, selectedCaptionProperties, newSelectedCaptionProperties);
						
						Control?.SetAnimationToView(animation);
					}
				}
				else
				{
					// 前回と違うものを選択した
					
					Animator.OnCaptionDeselected(selectedCaption);
					Control?.OnCaptionDeselected();
					Animator.OnCaptionSelected(newSelectedCaption);
					Control?.OnCaptionSelected(newSelectedCaption.GetTimeFromFrame(currentFrame), Animator[newSelectedCaption]);
				}
			}
			else
			{
				// 選択を解除した
				
				var selectedCaptionProperties = CaptionProperties.FromCaption(selectedCaption);
				
				if (!newState.Captions.Any(x => x.GetDifference(selectedCaptionProperties) <= 1))
				{
					// 選択していたものが削除された
					
					Animator.OnCaptionDeselected(selectedCaption);
					Control?.OnCaptionDeselected();
					Animator.OnCaptionDeleted(selectedCaption.Index);
				}
				else
				{
					// 選択していたものが存在する
					
					Control?.OnCaptionDeselected();
					Animator.OnCaptionDeselected(selectedCaption);
				}
			}
		}
		else
		{
			// 前回は何も選択していない

			if (newState.SelectedICaption is { } newSelectedCaption)
			{
				// 新しく何かを選択した
				
				Animator.OnCaptionSelected(newSelectedCaption);
				Control?.OnCaptionSelected(newSelectedCaption.GetTimeFromFrame(currentFrame), Animator[newSelectedCaption]);
			}
			else
			{
				// 今回も何も選択していない
			}
		}

		if (state == null ||
		    Math.Abs(state.Frame - newState.Frame) > float.Epsilon)
		{
			Animator.OnFrameUpdated(newState.Frame, newState.SelectedICaption);
			Control?.OnFrameUpdated(newState.Frame, newState.SelectedICaption, Animator[newState.SelectedCaption]);
		}

		state = SceneState.FromScene(Scene, currentFrame);
	}

	public void OnLoadProject(Stream stream)
	{
		using var br = new BinaryReader(stream);

		animator = Animator.Parse(Scene, br);
	}

	public Stream OnSaveProject()
	{
		using var bw = new BinaryWriter(new MemoryStream());
		
		Animator.Write(bw);
		bw.Flush();

		return new MemoryStream(((MemoryStream)bw.BaseStream).ToArray());
	}

	public override void Initialize()
	{
		control.CurrentTimeChanged += (_, e) => Scene.MarkerPosition = (long)e.Value.CurrentFrame;
		control.IsAnimationEnabledChanged += (_, e) =>
		{
			if (state?.SelectedICaption is not { } selectedCaption) return;
			
			if (e.Value)
			{
				var animation = Animator.EnableCaptionAnimation(selectedCaption);
				Control?.OnEnableCaptionAnimation(animation);
			}
			else
			{
				Animator.DisableCaptionAnimation(selectedCaption);
				Control?.OnDisableCaptionAnimation();
			}
		};
		control.AddKeyFrame += (_, e) =>
		{
			if (state?.SelectedICaption is not { } selectedCaption) return;
			if (Animator[selectedCaption] is not { } animation) return;

			animation.AddKeyFrame(selectedCaption.GetTimeFromProgress(e.Progress));
			Control?.KeyFramesChanged(animation);
		};
		control.RemoveKeyFrame += (_, e) =>
		{
			if (state?.SelectedICaption is not { } selectedCaption) return;
			if (Animator[selectedCaption] is not { } animation) return;
			
			animation.RemoveKeyFrame(e.Index);
			Control?.KeyFramesChanged(animation);
		};
		control.MoveKeyFrame += (_, e) =>
		{
			if (state?.SelectedICaption is not { } selectedCaption) return;
			if (Animator[selectedCaption] is not { } animation) return;

			animation.MoveKeyFrame(e.Index, e.Progress);
			Control?.KeyFramesChanged(animation);
		};
		control.PropertyChanged += ValueChanged;

		void ValueChanged(object sender, PropertyChangedEventArgs e)
		{
			if (state?.SelectedICaption is not { } selectedCaption) return; 
			if (Animator[selectedCaption] is not { } animation) return;

			var property = e.Kind switch
			{
				CaptionPropertyKind.X => animation.X,
				CaptionPropertyKind.Y => animation.Y,
				CaptionPropertyKind.Alpha => animation.Alpha,
				CaptionPropertyKind.Rotation => animation.Rotation,
				CaptionPropertyKind.FontSize => animation.FontSize,
				CaptionPropertyKind.LineSpacing => animation.LineSpacing,
				CaptionPropertyKind.LetterSpacing => animation.LetterSpacing,
				CaptionPropertyKind.ShadowDistance => animation.ShadowDistance,
				_ => null,
			};
			if (property == null) return;
			
			property.Mode = e.Mode;
			property.EaseIn = e.EaseIn;
			property.EaseOut = e.EaseOut;
			property.IterationDurationFrames = e.IterationDurationFrames;
			property.SetPairValue(control.CurrentTime, e.FromValue, e.ToValue);

			var currentFrame = Scene.MarkerPosition;
			animation.Apply(selectedCaption, currentFrame);
			state = SceneState.FromScene(Scene, currentFrame);
		}
	}

	public override void Enabled()
	{
		state = null;
		Animator.OnEnabled();
		Control?.OnEnabled();
	}

	public override void Disabled()
	{
		Animator.OnDisabled();
		Control?.OnDisabled();
		state = null;
	}

	public override string EnglishText => "Animate Captions";

	public override string Text => "字幕アニメーション";

	public override string Description => "字幕の座標や角度などをアニメーションできます。";

	public override Guid GUID => new("03f63956-256a-434f-8d45-3f9745faf6fa");

	public UserControl CreateControl() => control;
}