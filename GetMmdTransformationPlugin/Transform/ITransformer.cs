namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Transform;

public interface ITransformer
{
    void PreviewTransform(ModelBinding binding);
    void SaveTransform(ModelBinding binding);
    void ResetPreview();
}