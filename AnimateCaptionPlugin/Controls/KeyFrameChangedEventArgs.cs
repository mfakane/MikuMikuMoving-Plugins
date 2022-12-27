using System;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Controls;

public class KeyFrameChangedEventArgs : EventArgs
{
	public float Progress { get; }

	public int Index { get; }

	public KeyFrameChangedEventArgs(int index, float progress)
	{
		Index = index;
		Progress = progress;
	}
}