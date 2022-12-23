using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public static class Transformer
{
    public static ITransformer? Create(Scene scene)
    {
        switch (scene.Mode)
        {
            case EditMode.ModelMode when scene.ActiveModel is { } activeModel:
                return CompositeTransformer.Create(activeModel.Bones
                    .SelectMany(MotionLayerTransformer.FromBone)
                    .ToArray());
            case EditMode.AccessoryMode:
                return CompositeTransformer.Create(scene.Accessories
                    .SelectMany(MotionLayerTransformer.FromAccessory)
                    .ToArray());
            case EditMode.CameraMode:
                return CompositeTransformer.Create(CameraLayerTransformer.FromScene(scene).ToArray());
            case EditMode.EffectMode when scene.ActiveEffect is { } activeEffect:
                return new EffectTransformer(activeEffect);
            default:
                return null;
        }
    }
    
}