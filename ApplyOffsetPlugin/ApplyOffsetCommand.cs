using System;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin;

[UsedImplicitly]
public class ApplyOffsetCommand : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var transformer = Transformer.Create(Scene);
		var positionRotationTransformer = transformer?.GetPositionRotationTransformer();
		var positionTransformer = transformer?.GetPositionOnlyTransformer();
		var rotationTransformer = transformer?.GetRotationOnlyTransformer();
		var weightTransformer = transformer?.GetWeightTransformer();
		var distanceTransformer = transformer?.GetDistanceTransformer();
		var colorTransformer = transformer?.GetColorTransformer();

		if (transformer == null ||
		    positionRotationTransformer == null &&
		    positionTransformer == null &&
		    rotationTransformer == null &&
		    weightTransformer == null &&
		    distanceTransformer == null &&
		    colorTransformer == null)
		{
			MessageBox.Show(
				ApplicationForm,
				"対象がありません。\r\n対象のキーフレームを選択してから実行してください。",
				Localize(Text, EnglishText),
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
			);

			e.Cancel = true;

			return;
		}
		
		using var f = new ApplyOffsetForm
		{
			IsPositionVisible = positionRotationTransformer != null || positionTransformer != null,
			IsPositionLocalVisible = positionRotationTransformer?.CanTranslateByLocal == true || positionTransformer?.CanTransformByLocal == true,
			IsRotationVisible = positionRotationTransformer != null || rotationTransformer != null,
			IsRotationLocalVisible = positionRotationTransformer?.CanRotateByLocal == true || rotationTransformer?.CanTransformByLocal == true,
			IsWeightVisible = weightTransformer != null,
			IsDistanceVisible = distanceTransformer != null,
			IsColorVisible = colorTransformer != null,
		};
		f.ValueChanged += (sender, e2) =>
		{
			if (e2.PositionAndRotation is { } pr)
			{
				positionRotationTransformer?.PreviewTransform(
					pr.Position,
					pr.Rotation,
					pr.IsPositionLocal,
					pr.IsRotationLocal
				);
				positionTransformer?.PreviewTransform(pr.Position, pr.IsPositionLocal);
				rotationTransformer?.PreviewTransform(pr.Rotation, pr.IsRotationLocal);
			}
			
			if (e2.Weight is { } weight)
				weightTransformer?.PreviewTransform(weight);
			
			if (e2.Distance is { } distance)
				distanceTransformer?.PreviewTransform(distance);
			
			if (e2.Color is { } color)
				colorTransformer?.PreviewTransform(color, false);
		};

		var rt = f.ShowDialog(ApplicationForm);
		
		if (rt != DialogResult.OK)
		{
			ResetPreview();
			e.Cancel = true;

			return;
		}

		using (new UndoBlock(Scene))
		{
			positionRotationTransformer?.SaveTransform(f.Position, f.Rotation, f.IsPositionLocal, f.IsRotationLocal);
			positionTransformer?.SaveTransform(f.Position, f.IsPositionLocal);
			rotationTransformer?.SaveTransform(f.Rotation, f.IsRotationLocal);
			weightTransformer?.SaveTransform(f.Weight);
			distanceTransformer?.SaveTransform(f.Distance);
			colorTransformer?.SaveTransform(f.Color, false);
		}

		void ResetPreview()
		{
			positionRotationTransformer?.ResetPreview();
			positionTransformer?.ResetPreview();
			rotationTransformer?.ResetPreview();
			weightTransformer?.ResetPreview();
			distanceTransformer?.ResetPreview();
			colorTransformer?.ResetPreview();
		}
	}

	public override string Description => "選択したキーフレームの移動および回転値に指定したオフセットを与えます。";

	public override string EnglishText => "Apply\r\nOffset";

	public override string Text => "オフセット\r\n付加";

	public override Guid GUID => new("9d7c25d5-87e7-4855-9f75-6ca58c317c0f");
}