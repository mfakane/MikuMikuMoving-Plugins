using System;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.Framework;
using Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin;

[UsedImplicitly]
public class RevertFramesCommand : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var transformer = Transformer.Create(Scene);

		if (transformer == null ||
		    transformer.SelectedMinimumFrameNumber is not { } selectedMinimumFrameNumber ||
		    transformer.SelectedMaximumFrameNumber is not { } selectedMaximumFrameNumber)
		{
			e.Cancel = true;
			
			return;
		}

		using (Scene.BeginUndoBlock())
			transformer.RevertFrames(selectedMinimumFrameNumber, selectedMaximumFrameNumber);
	}

	public override string EnglishText => "Revert Frames";

	public override string Text => "フレーム時間反転";

	public override string Description => "選択されたキーフレームを時間軸方向に反転します。";

	public override Guid GUID => new("2abcad24-ce8a-476f-8fcd-4590ab9497e4");
}