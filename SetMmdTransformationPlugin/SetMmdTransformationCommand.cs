using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.Keystone.IO.MikuMikuDance;
using Linearstar.MikuMikuMoving.Framework;
using Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin;

[UsedImplicitly]
public class SetMmdTransformationCommand : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var mmdProcesses = Process.GetProcessesByName("MikuMikuDance");

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
			if (transformer is null 
			    or { HasMotion: false, HasKeyFrames: false } 
			    or not { SelectedMinimumFrameNumber: { }, SelectedMaximumFrameNumber: { } })
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

			Stream? vpdStream = null;
			Stream? vmdStream = null;
			
			if (transformer.HasMotion)
			{
				var vpdDocument = new VpdDocument();
				transformer.WriteTo(vpdDocument, f.ChangedBonesOnly);

				var vpdString = vpdDocument.GetFormattedText();
				vpdStream = new MemoryStream(VpdDocument.Encoding.GetBytes(vpdString));
			}

			if (transformer.HasKeyFrames)
			{
				var vmdDocument = new VmdDocument();
				transformer.WriteTo(
					vmdDocument,
					transformer.SelectedMinimumFrameNumber!.Value,
					transformer.SelectedMaximumFrameNumber!.Value
				);

				vmdStream = new MemoryStream();
				vmdDocument.Write(vmdStream);
				vmdStream.Position = 0;
			}
			
			if (vmdStream != null)
				using (vmdStream)
					MmdDrop.DropFile(f.SelectedMmd.MainWindowHandle, new("TempMotion" + f.SelectedMmd.Id + ".vmd", vmdStream));

			if (vpdStream != null)
				using (vpdStream)
					MmdDrop.DropFile(f.SelectedMmd.MainWindowHandle, new("TempPose" + f.SelectedMmd.Id + ".vpd", vpdStream)
					{
						Timeout = 500,
					});
		}
		finally
		{
			foreach (var i in mmdProcesses)
				i.Dispose();
		}
	}
	
	public override string EnglishText => "Set MMD\r\nTransformation";

	public override string Text => "MMD\r\nポーズ設定";

	public override string Description => Localize("現在の変形状態を MMD で現在選択されているモデルに設定します。", " Set current transformation to the current model on MMD.");

	public override Guid GUID => new("a3467169-214b-42a5-8249-45813043d12c");
}