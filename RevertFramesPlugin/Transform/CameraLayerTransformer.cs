using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

public class CameraLayerTransformer : KeyFrameTransformer<CameraFrameData, CameraFrameInterpolation>
{
    readonly CameraLayer layer;
    
    CameraLayerTransformer(CameraLayer layer)
        : base(layer.Frames.GetKeyFrames())
    {
        this.layer = layer;
    }

    public static IEnumerable<CameraLayerTransformer> FromScene(Scene scene) =>
        scene.Cameras
            .SelectMany(camera => camera.Layers)
            .Where(layer => layer.SelectedFrames.Any())
            .Select(layer => new CameraLayerTransformer(layer));

    protected override void ReplaceAllKeyFrames(IEnumerable<CameraFrameData> keyFrames) =>
        layer.Frames.ReplaceAllKeyFrames(keyFrames.ToList());

    protected override CameraFrameInterpolation GetInterpolation(CameraFrameData keyFrame) =>
        CameraFrameInterpolation.FromFrame(keyFrame);

    protected override CameraFrameInterpolation GetInvertedInterpolation(CameraFrameData keyFrame) =>
        CameraFrameInterpolation.FromFrame(keyFrame).Invert();

    protected override void SetInterpolation(CameraFrameData keyFrame, CameraFrameInterpolation interpolation)
    {
        keyFrame.InterpolDistA = interpolation.Da;
        keyFrame.InterpolDistB = interpolation.Db;
        keyFrame.InterpolFovA = interpolation.Fa;
        keyFrame.InterpolFovB = interpolation.Fb;
        keyFrame.InterpolMoveA = interpolation.Ma;
        keyFrame.InterpolMoveB = interpolation.Mb;
        keyFrame.InterpolRoteA = interpolation.Ra;
        keyFrame.InterpolRoteB = interpolation.Rb;
    }
}

public record CameraFrameInterpolation(
    InterpolatePoint Da,
    InterpolatePoint Db,
    InterpolatePoint Fa,
    InterpolatePoint Fb,
    InterpolatePoint Ma,
    InterpolatePoint Mb,
    InterpolatePoint Ra,
    InterpolatePoint Rb
)
{
    public static CameraFrameInterpolation FromFrame(CameraFrameData frame) =>
        new(
            frame.InterpolDistA,
            frame.InterpolDistB,
            frame.InterpolFovA,
            frame.InterpolFovB,
            frame.InterpolMoveA,
            frame.InterpolMoveB,
            frame.InterpolRoteA,
            frame.InterpolRoteB
        );
        
    public CameraFrameInterpolation Invert() =>
        new(
            this.Rb.Invert(),
            this.Ra.Invert(),
            this.Fb.Invert(),
            this.Fa.Invert(),
            this.Mb.Invert(),
            this.Ma.Invert(),
            this.Rb.Invert(),
            this.Ra.Invert()
        );
}