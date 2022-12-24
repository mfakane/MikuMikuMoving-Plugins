using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public class CameraLayerTransformer : KeyFrameTransformer<CameraFrameData>
{
    readonly CameraLayer layer;

    CameraLayerTransformer(CameraLayer layer)
        : base(layer.Frames.GetKeyFrames()) =>
        this.layer = layer;

    public static IEnumerable<CameraLayerTransformer> FromScene(Scene scene) =>
        scene.Cameras
            .SelectMany(camera => camera.Layers)
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new CameraLayerTransformer(layer));

    protected override void ReplaceAllKeyFrames(IEnumerable<CameraFrameData> keyFrames) =>
        layer.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
}