namespace Linearstar.MikuMikuMoving.RevertFramesPlugin.Transform;

public interface ITransformer
{
    long? SelectedMinimumFrameNumber { get; }
    long? SelectedMaximumFrameNumber { get; }
    void RevertFrames(long fromFrame, long toFrame);
}