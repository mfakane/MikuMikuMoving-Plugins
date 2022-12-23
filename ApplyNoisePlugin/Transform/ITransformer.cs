namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public interface ITransformer
{
    long? SelectedMinimumFrameNumber { get; }
    long? SelectedMaximumFrameNumber { get; }
    bool CanApplyTranslation { get; }
    bool CanTranslateByLocal { get; }
    bool CanApplyRotation { get; }
    bool CanRotateByLocal { get; }
    void ApplyNoise(NoiseContext context, bool translateByLocal, bool rotateByLocal);
}