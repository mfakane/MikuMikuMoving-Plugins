using DxMath;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public class CaptionTransformer : ITransformer
{
    readonly ICaption caption;

    public CaptionTransformer(ICaption caption)
    {
        this.caption = caption;
    }
    
    public IValueTransformer3 GetPositionOnlyTransformer() =>
        new CaptionPositionTransformer(caption);
    
    IValueTransformer6? ITransformer.GetPositionRotationTransformer() => null;
    IValueTransformer3? ITransformer.GetRotationOnlyTransformer() => null;
    IValueTransformer? ITransformer.GetWeightTransformer() => null;
    IValueTransformer? ITransformer.GetDistanceTransformer() => null;
    IValueTransformer3? ITransformer.GetColorTransformer() => null;
    
    class CaptionPositionTransformer : IValueTransformer3
    {
        readonly ICaption caption;
        readonly Vector3 initialLocation;
        
        public bool CanTransformByLocal => false;

        public CaptionPositionTransformer(ICaption caption)
        {
            this.caption = caption;
            initialLocation = caption.Location;
        }

        public void PreviewTransform(Vector3 amount, bool transformByLocal)
        {
            caption.Location = initialLocation + amount;
        }

        public void SaveTransform(Vector3 amount, bool transformByLocal)
        {
            caption.Location = initialLocation + amount;
        }

        public void ResetPreview()
        {
            caption.Location = initialLocation;
        }
    }
}