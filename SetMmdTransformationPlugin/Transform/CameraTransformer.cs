using System;
using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO.MikuMikuDance;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public class CameraTransformer : KeyFrameTransformer<CameraFrameData, CameraMotionData?>
{
    CameraTransformer(Camera camera)
        : base(camera.Layers[0].Frames.GetSelectedKeyFrames(), null)
    {
    }

    public static CameraTransformer? FromScene(Scene scene) =>
        (scene.Cameras.FirstOrDefault(x => x.Layers.Any(layer => layer.SelectedFrames.Any()))
         ?? scene.ActiveCamera) is { } camera
            ? new CameraTransformer(camera)
            : null;
    
    protected override void WriteKeyFrames(VmdDocument vmdDocument, IReadOnlyCollection<CameraFrameData> keyFrames, long fromFrame, long toFrame)
    {
        foreach (var keyFrame in keyFrames)
            vmdDocument.CameraFrames.Add(new()
            {
                FrameTime = (uint)(keyFrame.FrameNumber - fromFrame),
                Position = keyFrame.Position.ToArray(),
                Angle = new[] { -keyFrame.Angle.X, (float)(keyFrame.Angle.Y - Math.PI), keyFrame.Angle.Z },
                FovInDegree = (int)MathHelper.ToDegrees(keyFrame.Fov),
                Ortho = /* TODO: get perspective */ false,
                Radius = -keyFrame.Radius,
                AngleInterpolation = new[] { keyFrame.InterpolRoteA.ToVmd(), keyFrame.InterpolRoteB.ToVmd() },
                XInterpolation = new[] { keyFrame.InterpolMoveA.ToVmd(), keyFrame.InterpolMoveB.ToVmd() },
                YInterpolation = new[] { keyFrame.InterpolMoveA.ToVmd(), keyFrame.InterpolMoveB.ToVmd() },
                ZInterpolation = new[] { keyFrame.InterpolMoveA.ToVmd(), keyFrame.InterpolMoveB.ToVmd() },
                RadiusInterpolation = new[] { keyFrame.InterpolDistA.ToVmd(), keyFrame.InterpolDistB.ToVmd() },
                FovInterpolation = new[] { keyFrame.InterpolFovA.ToVmd(), keyFrame.InterpolFovB.ToVmd() },
            });
    }

    public override bool WriteTo(VmdDocument vmdDocument, long fromFrame, long toFrame)
    {
        vmdDocument.ModelName = "カメラ・照明\0on Data";
        
        return base.WriteTo(vmdDocument, fromFrame, toFrame);
    }
}