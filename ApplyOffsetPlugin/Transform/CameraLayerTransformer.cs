using System.Collections.Generic;
using System.Linq;
using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public class CameraLayerTransformer : ITransformer
{
    readonly CameraMotionTransformer transformer;

    CameraLayerTransformer(CameraLayer layer)
    {
        transformer = new CameraMotionTransformer(layer);
    }

    public static IEnumerable<CameraLayerTransformer> FromScene(Scene scene) =>
        scene.Cameras
            .SelectMany(camera => camera.Layers)
            .Where(layer => layer.SelectedFrames.Any())
            .DefaultIfEmpty(scene.ActiveCamera?.Layers.First())
            .Where(x => x != null)
            .Select(layer => new CameraLayerTransformer(layer!));

    public IValueTransformer6 GetPositionRotationTransformer() =>
        transformer;

    public IValueTransformer GetDistanceTransformer() =>
        transformer;

    IValueTransformer3? ITransformer.GetPositionOnlyTransformer() => null;
    IValueTransformer3? ITransformer.GetRotationOnlyTransformer() => null;
    IValueTransformer? ITransformer.GetWeightTransformer() => null;
    IValueTransformer3? ITransformer.GetColorTransformer() => null;

    class CameraMotionTransformer : IValueTransformer, IValueTransformer6
    {
        readonly CameraLayer layer;
        readonly CameraMotionData initialPreviewMotion;
        
        public bool CanTranslateByLocal => true;
        public bool CanRotateByLocal => false;
        
        public CameraMotionTransformer(CameraLayer layer)
        {
            this.layer = layer;
            initialPreviewMotion = layer.CurrentLocalMotion;
        }
        
        public void PreviewTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal)
        {
            layer.CurrentLocalMotion = TransformCore(initialPreviewMotion, translation, rotation, translateByLocal);
        }

        public void SaveTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal)
        {
            var keyFrames = layer.Frames.GetKeyFrames();
            
            foreach (var frame in keyFrames.Where(frame => frame.Selected))
            {
                var currentMotion = new CameraMotionData(frame.Position, frame.Angle, frame.Radius, frame.Fov);
                var newMotion = TransformCore(currentMotion, translation, rotation, translateByLocal);

                frame.Angle = newMotion.Angle;
                frame.Position = newMotion.Position;
                frame.Selected = true;
            }
            
            layer.Frames.ReplaceAllKeyFrames(keyFrames);
        }

        CameraMotionData TransformCore(CameraMotionData motion, Vector3 translation, Vector3 rotation, bool translateByLocal)
        {
            var newMotion = new CameraMotionData(motion.Position, motion.Angle, motion.Radius, motion.Fov);

            newMotion.Angle += rotation;
            newMotion.Position = ApplyTranslation(newMotion.Position, translation, newMotion.Angle, translateByLocal);

            return newMotion;
        }

        public void PreviewTransform(float amount)
        {
            var currentLocalMotion = layer.CurrentLocalMotion;
            
            currentLocalMotion.Radius = initialPreviewMotion.Radius + amount;
            layer.CurrentLocalMotion = currentLocalMotion;
        }

        public void SaveTransform(float amount)
        {
            var keyFrames = layer.Frames.GetKeyFrames();

            foreach (var frame in keyFrames.Where(frame => frame.Selected))
            {
                frame.Radius += amount;
                frame.Selected = true;
            }

            layer.Frames.ReplaceAllKeyFrames(keyFrames);
        }

        public void ResetPreview()
        {
            layer.CurrentLocalMotion = initialPreviewMotion;
        }

        static Vector3 ApplyTranslation(Vector3 currentTranslation, Vector3 translationOffset, Vector3 currentRotation, bool transformByLocal)
        {
            if (!transformByLocal)
                return currentTranslation + translationOffset;

            var localQuaternion = Quaternion.RotationYawPitchRoll(-currentRotation.Y, -currentRotation.X, currentRotation.Z);
            
            return currentTranslation +
                   Vector3.TransformCoordinate(translationOffset, Matrix.RotationQuaternion(localQuaternion));
        }
    }
}