using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public static class Transformer
{
    public static ITransformer? Create(Scene scene)
    {
        switch (scene.Mode)
        {
            case EditMode.ModelMode when scene.ActiveModel is { } activeModel:
                return ModelTransformer.Create(activeModel);
            case EditMode.CameraMode:
                return CompositeTransformer.Create(new ITransformer?[]
                    {
                        CameraTransformer.FromScene(scene),
                        LightTransformer.FromScene(scene),
                    }
                    .Where(x => x != null)
                    .ToArray()!);
            default:
                return null;
        }
    }
}