using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.Framework;
using Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Mmd;
using Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Transform;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin;

[UsedImplicitly]
public class GetMmdTransformationCommand : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var activeModel = Scene.ActiveModel;
		var transformer = Transformer.Create(activeModel);
		
		if (activeModel == null || transformer == null)
		{
			MessageBox.Show(
				ApplicationForm,
				Localize
				(
					"適用するモデルがありません。\r\nモデルを選択している状態で実行してください。",
					"No model to apply.\r\nPlease select a model to proceed."
				),
				Localize(Text, EnglishText),
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
			);
			e.Cancel = true;

			return;
		}

		var mmdInstances = Process.GetProcessesByName("MikuMikuDance")
			.Where(x => x.Responding)
			.Select(x => new MmdImport(x))
			.ToArray();
		
		try
		{
			if (!mmdInstances.Any())
			{
				MessageBox.Show(
					ApplicationForm,
					Localize
					(
						"MikuMikuDance が起動されていません。\r\nMikuMikuDance が起動中であり、モデルが読み込まれている状態で実行してください。",
						"Cannot find MikuMikuDance.\r\nPlease start MikuMikuDance and load a model to proceed."
					),
					Localize(Text, EnglishText),
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
				e.Cancel = true;

				return;
			}

			mmdInstances = mmdInstances.Where(x => x.Models.Any()).ToArray();

			if (!mmdInstances.Any())
			{
				MessageBox.Show(
					ApplicationForm,
					Localize
					(
						"MikuMikuDance にモデルが読み込まれていません。\r\nモデルが読み込まれている状態で実行してください。",
						"No model found on MikuMikuDance.\r\nNeeds at least one model loaded on MikuMikuDance to proceed."
					),
					Localize(Text, EnglishText),
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
				e.Cancel = true;

				return;
			}

			using var f = new GetMmdTransformationForm(Scene.Language, mmdInstances);
			ModelBinding? binding = null;
			
			f.SelectedModelChanged += (sender, e2) =>
			{
				if (f.SelectedModel == null)
				{
					transformer.ResetPreview();
					return;
				}
				
				if (binding?.MmdModel.GetHashCode() != f.SelectedModel.GetHashCode())
					binding = ModelBinding.Create(activeModel, f.SelectedModel);
				
				transformer.PreviewTransform(binding);
			};

			if (f.ShowDialog(ApplicationForm) != DialogResult.OK ||
			    f.SelectedModel == null)
			{
				e.Cancel = true;
				transformer.ResetPreview();
				
				return;
			}

			binding ??= ModelBinding.Create(activeModel, f.SelectedModel);
			transformer.SaveTransform(binding);
		}
		finally
		{
			foreach (var i in mmdInstances)
				i.Dispose();
		}
	}

	public override string EnglishText => "Get MMD\r\nTransformation";

	public override string Text => "MMD\r\nポーズ取得";

	public override string Description => Localize("MikuMikuDance のモデル変形状態を取得します。", "Receives model transformation status from MikuMikuDance.");

	public override Guid GUID => new("2c309851-7f3e-4b62-87f9-44e5c492a0f8");
}