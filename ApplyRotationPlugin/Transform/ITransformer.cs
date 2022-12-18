using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyRotationPlugin.Transform;

public interface ITransformer
{
    void PreviewTransform(Vector3 origin, Vector3 rotation, bool translateOnly);
    void SaveTransform(Vector3 origin, Vector3 rotation, bool translateOnly);
    void ResetPreview();
}
