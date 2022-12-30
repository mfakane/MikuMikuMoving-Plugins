using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.Keystone.IO.MikuMikuDance;
using Linearstar.MikuMikuMoving.Framework;
using Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Interop;
using Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin;

[UsedImplicitly]
public class SetMmdTransformationCommand : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var mmdProcesses = MmdDropTarget.GetTargetProcesses();

		e.Cancel = true;

		try
		{
			if (!mmdProcesses.Any())
			{
				MessageBox.Show(
					ApplicationForm,
					Localize
					(
						"MikuMikuDance が起動されていません。\r\nMikuMikuDance が起動している状態で実行してください。",
						"Cannot find MikuMikuDance.\r\nPlease start MikuMikuDance to proceed."
					),
					Localize(Text, EnglishText),
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);

				return;
			}

			var transformer = Transformer.Create(Scene);
			if (transformer is null or { HasMotion: false, HasKeyFrames: false })
			{
				MessageBox.Show(
					ApplicationForm,
					Localize
					(
						"モデルの現在の変形状態およびモデル、カメラ、または照明のキーフレームのみ送信できます。\r\nモデル、カメラ、または照明を選択してください。",
						"Only the current model transformation or model, camera, light keyframes can be sent.\r\nPlease select them first."
					),
					Localize(Text, EnglishText),
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
				);
				return;
			}

			using var f = new SetMmdTransformationForm(Scene.Language, mmdProcesses);
			
			if (f.ShowDialog(ApplicationForm) != DialogResult.OK)
				return;

			var selectedInstance = f.SelectedMmdInstance;
			TempFile? vpdFile = null;
			TempFile? vmdFile = null;
			
			if (transformer.HasMotion)
			{
				var vpdDocument = new VpdDocument();
				transformer.WriteTo(vpdDocument, f.ChangedBonesOnly);

				var vpdString = vpdDocument.GetFormattedText();

				vpdFile = new TempFile($"TempPose{selectedInstance.Id}_{Environment.TickCount}.vpd");
				File.WriteAllText(vpdFile.FileName, vpdString, VpdDocument.Encoding);
			}

			if (transformer is
			    {
				    HasKeyFrames: true, 
				    SelectedMinimumFrameNumber: { } minFrame,
				    SelectedMaximumFrameNumber: { } maxFrame,
			    })
			{
				var vmdDocument = new VmdDocument();
				transformer.WriteTo(vmdDocument, minFrame, maxFrame);

				vmdFile = new TempFile($"TempMotion{selectedInstance.Id}_{Environment.TickCount}.vmd");

				using var stream = File.OpenWrite(vmdFile.FileName);
				
				vmdDocument.Write(stream);
			}

			selectedInstance.DoDragDrop(vpdFile?.FileName, vmdFile?.FileName);
		}
		finally
		{
			foreach (var i in mmdProcesses)
				i.Dispose();
		}
	}
	
	public override string EnglishText => "Set MMD Transformation";

	public override string Text => "MMD ポーズ設定";

	public override string Description => Localize("現在の変形状態を MMD で現在選択されているモデルに設定します。", " Set current transformation to the current model on MMD.");

	public override Guid GUID => new("a3467169-214b-42a5-8249-45813043d12c");
}