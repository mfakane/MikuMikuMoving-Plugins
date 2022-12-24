using Linearstar.Keystone.IO.MikuMikuDance;

namespace Linearstar.MikuMikuMoving.SetMmdTransformationPlugin.Transform;

public interface ITransformer
{
    long? SelectedMinimumFrameNumber { get; }
    long? SelectedMaximumFrameNumber { get; }
    bool HasKeyFrames { get; }
    bool HasMotion { get; }
    bool WriteTo(VmdDocument vmdDocument, long fromFrame, long toFrame);
    bool WriteTo(VpdDocument vpdDocument, bool changedOnly);
}
