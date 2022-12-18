using System.Collections.Generic;
using System.Linq;
using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public class LightTransformer : ITransformer
{
    readonly Light light;

    LightTransformer(Light light)
    {
        this.light = light;
    }
    
    public static IEnumerable<LightTransformer> FromScene(Scene scene) =>
        scene.Lights
            .Where(light => light.SelectedFrames.Any())
            .DefaultIfEmpty(scene.ActiveLight)
            .Where(x => x != null)
            .Select(light => new LightTransformer(light));

    public IValueTransformer3 GetPositionOnlyTransformer() =>
        new LightPositionTransformer(light);

    public IValueTransformer3 GetColorTransformer() =>
        new LightColorTransformer(light);
    
    IValueTransformer6? ITransformer.GetPositionRotationTransformer() => null;
    IValueTransformer3? ITransformer.GetRotationOnlyTransformer() => null;
    IValueTransformer? ITransformer.GetWeightTransformer() => null;
    IValueTransformer? ITransformer.GetDistanceTransformer() => null;

    class LightPositionTransformer : IValueTransformer3
    {
        readonly Light light;
        readonly Vector3 initialPosition;

        public LightPositionTransformer(Light light)
        {
            this.light = light;
            initialPosition = light.CurrentMotion.Position;
        }
        
        public bool CanTransformByLocal => false;

        public void PreviewTransform(Vector3 amount, bool transformByLocal)
        {
            var motion = light.CurrentMotion;
            motion.Position = initialPosition + amount;
            light.CurrentMotion = motion;
        }

        public void SaveTransform(Vector3 amount, bool transformByLocal)
        {
            var keyFrames = light.Frames.GetKeyFrames();

            foreach (var frame in keyFrames.Where(frame => frame.Selected))
            {
                frame.Position += amount;
                frame.Selected = true;
            }
            
            light.Frames.ReplaceAllKeyFrames(keyFrames);
        }

        public void ResetPreview()
        {
            var motion = light.CurrentMotion;
            motion.Position = initialPosition;
            light.CurrentMotion = motion;
        }
    }
    
    class LightColorTransformer : IValueTransformer3
    {
        readonly Light light;
        readonly Vector3 initialColor;

        public LightColorTransformer(Light light)
        {
            this.light = light;
            initialColor = light.CurrentMotion.Color;
        }
        
        public bool CanTransformByLocal => false;

        public void PreviewTransform(Vector3 amount, bool transformByLocal)
        {
            var motion = light.CurrentMotion;
            motion.Color = initialColor + amount;
            light.CurrentMotion = motion;
        }

        public void SaveTransform(Vector3 amount, bool transformByLocal)
        {
            var keyFrames = light.Frames.GetKeyFrames();

            foreach (var frame in keyFrames.Where(frame => frame.Selected))
            {
                frame.Color += amount;
            }
            
            light.Frames.ReplaceAllKeyFrames(keyFrames);
        }

        public void ResetPreview()
        {
            var motion = light.CurrentMotion;
            motion.Color = initialColor;
            light.CurrentMotion = motion;
        }
    }
}