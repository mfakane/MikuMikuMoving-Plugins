using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public interface ITransformer
{
    IValueTransformer3? GetPositionOnlyTransformer();
    IValueTransformer3? GetRotationOnlyTransformer();
    IValueTransformer6? GetPositionRotationTransformer();
    IValueTransformer? GetWeightTransformer();
    IValueTransformer? GetDistanceTransformer();
    IValueTransformer3? GetColorTransformer();
}

public interface IValueTransformer
{
    void PreviewTransform(float amount);
    void SaveTransform(float amount);
    void ResetPreview();
}

public interface IValueTransformer3
{
    bool CanTransformByLocal { get; }
    void PreviewTransform(Vector3 amount, bool transformByLocal);
    void SaveTransform(Vector3 amount, bool transformByLocal);
    void ResetPreview();
}

public interface IValueTransformer6
{
    bool CanTranslateByLocal { get; }
    bool CanRotateByLocal { get; }
    void PreviewTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal);
    void SaveTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal);
    void ResetPreview();
}
