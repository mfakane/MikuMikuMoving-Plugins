using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin;

[UsedImplicitly]
public class BeatBasedCopyCommand : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var transformer = Transformer.Create(Scene);
		
		if (transformer is not { SelectedMinimumFrameNumber: { }, SelectedMaximumFrameNumber: { } })
		{
			MessageBox.Show(
				ApplicationForm,
				"対象のキーフレームがありません。\r\n対象のボーン、アクセサリ、カメラ、またはエフェクトのキーフレームを選択してから実行してください。",
				Localize(Text, EnglishText),
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
			);

			e.Cancel = true;

			return;
		}

		using var f = new BeatBasedCopyForm
		{
			BeginFrame = 0,
			BeatsPerMinute = 120,
			StartupBeats = 0,
			IntervalBeats = 1,
			Times = 4,
		};
		if (f.ShowDialog(ApplicationForm) != DialogResult.OK)
		{
			e.Cancel = true;

			return;
		}

		var context = new BeatContext(f.StartupBeats, f.BeatsPerMinute, f.IntervalBeats, f.Times, Scene.KeyFramePerSec);

		transformer.Copy(context, Scene.MarkerPosition);
	}

	public override string EnglishText => "Beat based\r\nCopy";

	public override string Text => "ビート\r\nコピー";

	public override string Description => "選択されたキーフレームを拍単位で位置を指定してコピーします。";

	public override Guid GUID => new("e7e50c23-7025-4d89-9ea6-171d023c11ab");
}