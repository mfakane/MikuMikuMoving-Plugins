﻿using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public class CameraLayerTransformer : KeyFrameTransformer<CameraFrameData>
{
    readonly CameraLayer layer;

    public override bool CanTranslateByLocal => false;

    public override bool CanRotateByLocal => false;
 
    public override bool CanApplyWeight => false;
    
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

    protected override CameraFrameData GetFrame(long frameNumber) =>
        layer.Frames.GetFrame(frameNumber);

    protected override void ApplyNoiseToKeyFrame(CameraFrameData keyFrame, NoiseValue value, bool translateByLocal, bool rotateByLocal, bool applyWeight)
    {
        keyFrame.Position += value.Position;
        keyFrame.Angle += value.Rotation;
    }
}