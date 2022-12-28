using System;
using System.Linq;
using DxMath;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.DetailedStatusPlugin;

[UsedImplicitly]
public class DetailedStatusPlugin : ResidentBase
{
    NonInteractiveTextImage? statusImage;

    public override void Initialize()
    {
        statusImage = new NonInteractiveTextImage(Scene);
        Scene.ScreenObjects.Add(statusImage);
    }

    public override void Update(float frame, float elapsedTime)
    {
        if (statusImage == null) return;

        statusImage.Visible = Scene.State == SceneState.Editing;

        if (!statusImage.Visible) return;

        var newStatusText = GetStatusText();

        statusImage.Text = newStatusText;
        statusImage.Position = new(
            Scene.SystemInformation.BoneOperationControlDock == ScreenDock.BottomLeft ? 96 : 1,
            Scene.ScreenSize.Height - statusImage.Image.Height + 2);
    }

    public override void Dispose()
    {
        if (statusImage != null)
        {
            Scene.ScreenObjects.Remove(statusImage);
            statusImage.Dispose();
            statusImage = null;
        }

        base.Dispose();
    }

    string GetStatusText()
    {
        switch (Scene.Mode)
        {
            case EditMode.AccessoryMode:
                if (Scene.ActiveAccessory is { } accessory)
                {
                    var selectedLayer = accessory.SelectedLayers.FirstOrDefault() ?? accessory.Layers.First();
                    var accData = selectedLayer.CurrentLocalMotion;

                    return FormatVector3(
                        $"{Localize("アクセサリ", "Accessory")} { selectedLayer.Name}",
                        MathHelper.ToDegrees(MathHelper.ToEulerAngle(accData.Rotation)),
                        accData.Move
                    );
                }
                break;
            case EditMode.EffectMode:
                if (Scene.ActiveEffect is { } effect)
                    return FormatVector3(
                        Localize("エフェクト", "Effect"),
                        MathHelper.ToDegrees(MathHelper.ToEulerAngle(effect.CurrentLocalMotion.Rotation)),
                        effect.CurrentLocalMotion.Move
                    );

                break;
            case EditMode.ModelMode:
                if (Scene.ActiveModel?.Bones.FirstOrDefault(x => x.SelectedLayers.Any()) is { } bone)
                {
                    var selectedLayer = bone.SelectedLayers.First();
                    var boneMotion = selectedLayer.CurrentLocalMotion;
                    var layerName = selectedLayer.LayerID.ToString();

                    try
                    {
                        layerName = selectedLayer.Name;
                    }
                    catch
                    {
                        // Name で例外が出る場合があることへの暫定対策として握りつぶし
                    }

                    return FormatVector3(
                        $"{Localize(bone.Name, bone.EnglishName)} {layerName}",
                        MathHelper.ToDegrees(MathHelper.ToEulerAngle(boneMotion.Rotation)),
                        (bone.BoneFlags & BoneType.XYZ) != 0 ? boneMotion.Move : null
                    );
                }

                break;
        }

        var selectedCamera = Scene.Cameras.SelectMany(x => x.Layers).FirstOrDefault(x => x.SelectedFrames.Any()) ??
                             Scene.Cameras.First().Layers.First();
        var cameraMotion = selectedCamera.CurrentLocalMotion;

        return FormatVector3(
            $"{Localize("カメラ", "Camera")} {selectedCamera.Name}",
            MathHelper.ToDegrees(cameraMotion.Angle),
            cameraMotion.Position
        );
    }

    string FormatVector3(string name, Vector3 rotation, Vector3? positionOrNull = null)
    {
        var posLabel = Localize("位置", "Pos ");
        var angLabel = Localize("角度", "Ang ");

        if (positionOrNull is { } position)
            return $"""
                    {name}
                    {posLabel}: X:{position.X:0.000000}, Y:{position.Y:0.000000}, Z:{position.Z:0.000000}
                    {angLabel}: X:{rotation.X:0.000000}, Y:{rotation.Y:0.000000}, Z:{rotation.Z:0.000000}
                    """;

        return $"""
                {name}
                {angLabel}: X:{rotation.X:0.000000}, Y:{rotation.Y:0.000000}, Z:{rotation.Z:0.000000}
                """;
    }

    public override string EnglishText => "Detailed Status";

    public override string Text => "詳細情報";

    public override string Description => Localize("選択されたボーンなどのオブジェクトの位置および回転を表示します。",
        "Shows detailed position/rotation information for selected bone or camera.");

    public override Guid GUID => new("f5baf169-b0f1-4809-bf89-35a972046f3a");
}