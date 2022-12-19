using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

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
            case EditMode.AccessoryMode:
                return CompositeTransformer.Create(scene.Accessories
                    .SelectMany(MotionLayerTransformer.FromAccessory)
                    .ToArray());
            case EditMode.CameraMode:
                return CompositeTransformer.Create(CameraLayerTransformer.FromScene(scene).ToArray());
            case EditMode.EffectMode:
                return CompositeTransformer.Create(scene.Effects
                    .Where(x => x.SelectedFrames.Any())
                    .Select(x => new EffectTransformer(x))
                    .ToArray());
            default:
                return null;
        }
    }
    
}