using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public class LightTransformer : KeyFrameTransformer<LightFrameData>
{
    readonly Light light;
    
    LightTransformer(Light light)
        : base(light.Frames.GetKeyFrames()) =>
        this.light = light;

    public static IEnumerable<LightTransformer> FromScene(Scene scene) =>
        scene.Lights
            .Where(light => light.SelectedFrames.Any())
            .Select(light => new LightTransformer(light));
    
    protected override void ReplaceAllKeyFrames(IEnumerable<LightFrameData> keyFrames) => 
        light.Frames.ReplaceAllKeyFrames(keyFrames.ToList());
}