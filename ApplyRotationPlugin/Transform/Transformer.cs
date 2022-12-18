using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin.Transform;

public static class Transformer
{
    public static ITransformer? Create(Scene scene)
    {
        switch (scene.Mode)
        {
            case EditMode.ModelMode when scene.ActiveModel is { } activeModel:
                return CompositeTransformer.Create(activeModel.Bones
                    .SelectMany(x => MotionLayerTransformer.FromBone(activeModel, x))
                    .ToArray());
            case EditMode.AccessoryMode when scene.ActiveAccessory is { } activeAccessory:
                return CompositeTransformer.Create(MotionLayerTransformer.FromAccessory(activeAccessory).ToArray());
            case EditMode.EffectMode when scene.ActiveEffect is { } activeEffect:
                return new EffectTransformer(activeEffect);
            default:
                return null;
        }
    }
}