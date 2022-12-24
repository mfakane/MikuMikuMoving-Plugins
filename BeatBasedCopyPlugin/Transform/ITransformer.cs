namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public interface ITransformer
{
    long? SelectedMinimumFrameNumber { get; }
    long? SelectedMaximumFrameNumber { get; }
    void Copy(BeatContext context, long markerPosition);
}