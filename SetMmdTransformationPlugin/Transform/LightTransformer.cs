using System.Collections.Generic;
using System.Linq;
using Linearstar.Keystone.IO.MikuMikuDance;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public class LightTransformer : KeyFrameTransformer<LightFrameData, LightMotionData?>
{
    LightTransformer(Light light)
        : base(light.Frames.GetSelectedKeyFrames(), null)
    {
    }

    public static LightTransformer? FromScene(Scene scene) =>
        (scene.Lights.FirstOrDefault(x => x.SelectedFrames.Any())
         ?? scene.ActiveLight) is { } light
            ? new LightTransformer(light)
            : null;

    protected override void WriteKeyFrames(VmdDocument vmdDocument, IReadOnlyCollection<LightFrameData> keyFrames, long fromFrame, long toFrame)
    {
        foreach (var keyFrame in keyFrames)
            vmdDocument.LightFrames.Add(new()
            {
                FrameTime = (uint)(keyFrame.FrameNumber - fromFrame),
                Position = (-keyFrame.Position / 100).ToArray(),
                Color = keyFrame.Color.ToArray(),
            });
    }

    public override bool WriteTo(VmdDocument vmdDocument, long fromFrame, long toFrame)
    {
        vmdDocument.ModelName = "カメラ・照明\0on Data";

        return base.WriteTo(vmdDocument, fromFrame, toFrame);
    }
}