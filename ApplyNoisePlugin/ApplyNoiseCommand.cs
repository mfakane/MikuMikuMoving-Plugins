using System;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin;

[UsedImplicitly]
public class ApplyNoiseCommand : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var transformer = Transformer.Create(Scene);

		if (transformer is not { SelectedMinimumFrameNumber: { }, SelectedMaximumFrameNumber: { } })
		{
			MessageBox.Show(
				ApplicationForm,
				Localize
				(
					"対象のモデル、カメラ、アクセサリ、またはエフェクトがありません。\r\n対象のキーフレームを選択してから実行してください。",
					"No target selected.\r\nPlease select one or more keyframes to apply some noise."
				),
				Localize(Text, EnglishText),
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
			);

			e.Cancel = true;

			return;
		}

		using var f = new ApplyNoiseForm
		{
			KeyFrameInterval = 5,
			KeyShiftWidth = 0,
			NoiseValueInterval = 5,
			NoiseValue = NoiseValue.Default,
			IsPositionEnabled = transformer.CanApplyTranslation,
			IsPositionLocalVisible = transformer.CanTranslateByLocal,
			IsRotationEnabled = transformer.CanApplyRotation,
			IsRotationLocalVisible = transformer.CanRotateByLocal,
			IsEnvironmentEnabled = false,
			IsPositionLocal = transformer.CanRotateByLocal,
			IsRotationLocal = transformer.CanRotateByLocal,
		};
		if (f.ShowDialog() != DialogResult.OK)
		{
			e.Cancel = true;

			return;
		}

		var context = new NoiseContext(
			transformer.SelectedMinimumFrameNumber.Value,
			transformer.SelectedMaximumFrameNumber.Value,
			f.KeyFrameInterval,
			f.KeyShiftWidth,
			f.NoiseValueInterval,
			f.NoiseValue
		);

		using (Scene.BeginUndoBlock())
			transformer.ApplyNoise(context, f.IsPositionLocal, f.IsRotationLocal);
	}

	public override string Description => "選択したキーフレームの移動および回転値に指定したオフセットを与えます。";

	public override string EnglishText => "Apply\r\nNoise";

	public override string Text => "ノイズ\r\n付加";

	public override Guid GUID => new("3c92a157-b847-4eb6-9e2d-a1df9786dfcc");
}