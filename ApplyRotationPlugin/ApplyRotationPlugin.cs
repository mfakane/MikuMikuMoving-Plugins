using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DxMath;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.ApplyRotationPlugin.Transform;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin;

[UsedImplicitly]
public class ApplyRotationPlugin : CommandBase
{
	public override void Run(CommandArgs e)
	{
		var transformer = Transformer.Create(Scene);
		
		if (transformer == null)
		{
			MessageBox.Show(
				ApplicationForm,
				Localize
				(
					"対象のモデル、アクセサリ、またはエフェクトがありません。\r\n対象のキーフレームを選択してから実行してください。",
					"No target selected.\r\nPlease select one or more keyframes to rotate."
				),
				Localize(Text, EnglishText),
				MessageBoxButtons.OK,
				MessageBoxIcon.Information
			);

			e.Cancel = true;

			return;
		}

		var assembly = Assembly.GetExecutingAssembly();

		using (EnableScreenObject())
		{
			using var originStream = assembly.GetManifestResourceStream("Origin")!;
			using var originMarker = new ScreenImage_2D(Point.Empty, originStream);
			using var f = new ApplyRotationForm(Scene.Language);
			
			try
			{
				Scene.ScreenObjects.Add(originMarker);
				f.ValueChanged += (sender, e2) =>
				{
					// ReSharper disable AccessToDisposedClosure
					if (!originMarker.Disposed)
					{
						// 中心座標を画面上の位置に変換してマーカーに反映
						var camera = Scene.ActiveCamera ?? Scene.Cameras.First();
						var pt = Vector3.Project
						(
							e2.Position,
							0,
							0,
							Scene.ScreenSize.Width,
							Scene.ScreenSize.Height,
							Scene.SystemInformation.NearPlane,
							Scene.SystemInformation.FarPlane,
							camera.ViewMatrix * camera.ProjectionMatrix
						);
						originMarker.Position = new Point((int)(pt.X - originMarker.Size.Width / 2f), (int)(pt.Y - originMarker.Size.Height / 2f));
					}
					// ReSharper restore AccessToDisposedClosure

					// ポーズに反映
					transformer.PreviewTransform(e2.Position, e2.Rotation, e2.IsMoveOnly);
				};

				var rt = f.ShowDialog(ApplicationForm);

				if (rt != DialogResult.OK)
				{
					transformer.ResetPreview();
					e.Cancel = true;

					return;
				}

				using (Scene.BeginUndoBlock())
					transformer.SaveTransform(f.Position, f.Rotation, f.IsMoveOnly);
			}
			finally
			{
				Scene.ScreenObjects.Remove(originMarker);
			}
		}
	}

	public override string EnglishText => "Apply\r\nRotation";

	public override string Text => "任意中心\r\n回転付加";

	public override string Description => Localize("任意の点を中心に位置および角度を回転します。", "Rotates the selected motion by the specified rotation and origin.");

	public override Guid GUID => new("fd853cb9-6b70-4bed-98ce-906e0431db5e");
}