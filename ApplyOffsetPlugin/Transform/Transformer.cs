using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public static class Transformer
{
    public static ITransformer? Create(Scene scene)
    {
        switch (scene.Mode)
        {
            case EditMode.ModelMode when scene.ActiveModel is { } activeModel:
                return CompositeTransformer.Create(activeModel.Bones
                    .SelectMany(MotionLayerTransformer.FromBone)
                    .Cast<ITransformer>()
                    .Concat(MorphTransformer.FromModel(activeModel))
                    .ToArray());
            case EditMode.AccessoryMode when scene.ActiveAccessory is { } activeAccessory:
                return CompositeTransformer.Create(MotionLayerTransformer.FromAccessory(activeAccessory).ToArray());
            case EditMode.CameraMode:
                return CompositeTransformer.Create(CameraLayerTransformer.FromScene(scene)
                    .Cast<ITransformer>()
                    .Concat(LightTransformer.FromScene(scene))
                    .ToArray());
            case EditMode.EffectMode when scene.ActiveEffect is { } activeEffect:
                return new EffectTransformer(activeEffect);
            case EditMode.CaptionMode:
                return CompositeTransformer.Create(scene.SelectedCaptions
                    .Select(x => new CaptionTransformer(x))
                    .ToArray());
            default:
                return null;
        }
    }
}