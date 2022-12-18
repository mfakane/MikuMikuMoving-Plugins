using System.Collections.Generic;
using System.Linq;
using DxMath;

namespace Linearstar.MikuMikuMoving.ApplyOffsetPlugin.Transform;

public class CompositeTransformer : ITransformer
{
    readonly IReadOnlyCollection<ITransformer> transformers;

    CompositeTransformer(IReadOnlyCollection<ITransformer> transformers) =>
        this.transformers = transformers;

    public static ITransformer? Create(IReadOnlyCollection<ITransformer> transformers) =>
        transformers.Any() ? new CompositeTransformer(transformers) : null;
    
    public IValueTransformer3? GetPositionOnlyTransformer()
    {
        var children = transformers
            .Select(i => i.GetPositionOnlyTransformer())
            .Where(i => i != null)
            .Cast<IValueTransformer3>()
            .ToArray();

        return children.Length == 0 ? null : new CompositeValueTransformer3(children);
    }

    public IValueTransformer3? GetRotationOnlyTransformer()
    {
        var children = transformers
            .Select(i => i.GetRotationOnlyTransformer())
            .Where(x => x != null)
            .Cast<IValueTransformer3>()
            .ToArray();
        
        return children.Length == 0 ? null : new CompositeValueTransformer3(children);
    }

    public IValueTransformer6? GetPositionRotationTransformer()
    {
        var children = transformers
            .Select(i => i.GetPositionRotationTransformer())
            .Where(x => x != null)
            .Cast<IValueTransformer6>()
            .ToArray();
        
        return children.Any() ? new CompositeValueTransformer6(children) : null;
    }

    public IValueTransformer? GetWeightTransformer()
    {
        var children = transformers
            .Select(i => i.GetWeightTransformer())
            .Where(x => x != null)
            .Cast<IValueTransformer>()
            .ToArray();

        return children.Any() ? new CompositeValueTransformer(children) : null;
    }

    public IValueTransformer? GetDistanceTransformer()
    {
        var children = transformers
            .Select(i => i.GetDistanceTransformer())
            .Where(x => x != null)
            .Cast<IValueTransformer>()
            .ToArray();
        
        return children.Any() ? new CompositeValueTransformer(children) : null;
    }

    public IValueTransformer3? GetColorTransformer()
    {
        var children = transformers
            .Select(i => i.GetColorTransformer())
            .Where(x => x != null)
            .Cast<IValueTransformer3>()
            .ToArray();

        return children.Any() ? new CompositeValueTransformer3(children) : null;
    }

    class CompositeValueTransformer : IValueTransformer
    {
        readonly IReadOnlyCollection<IValueTransformer> transformers;

        public CompositeValueTransformer(IReadOnlyCollection<IValueTransformer> transformers) =>
            this.transformers = transformers;

        public void PreviewTransform(float amount)
        {
            foreach (var i in transformers)
                i.PreviewTransform(amount);
        }

        public void SaveTransform(float amount)
        {
            foreach (var i in transformers)
                i.SaveTransform(amount);
        }

        public void ResetPreview()
        {
            foreach (var i in transformers)
                i.ResetPreview();
        }
    }
    
    class CompositeValueTransformer3 : IValueTransformer3
    {
        readonly IReadOnlyCollection<IValueTransformer3> valueTransformers;

        public bool CanTransformByLocal =>
            valueTransformers.Any(i => i.CanTransformByLocal);

        public CompositeValueTransformer3(IReadOnlyCollection<IValueTransformer3> valueTransformers) =>
            this.valueTransformers = valueTransformers;

        public void PreviewTransform(Vector3 amount, bool transformByLocal)
        {
            foreach (var i in valueTransformers)
                i.PreviewTransform(amount, transformByLocal);
        }

        public void SaveTransform(Vector3 amount, bool transformByLocal)
        {
            foreach (var i in valueTransformers)
                i.SaveTransform(amount, transformByLocal);
        }

        public void ResetPreview()
        {
            foreach (var i in valueTransformers)
                i.ResetPreview();
        }
    }
    
    class CompositeValueTransformer6 : IValueTransformer6
    {
        readonly IReadOnlyCollection<IValueTransformer6> valueTransformers;

        public bool CanTranslateByLocal =>
            valueTransformers.Any(i => i.CanTranslateByLocal);

        public bool CanRotateByLocal =>
            valueTransformers.Any(i => i.CanRotateByLocal);

        public CompositeValueTransformer6(IReadOnlyCollection<IValueTransformer6> valueTransformers) =>
            this.valueTransformers = valueTransformers;

        public void PreviewTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal)
        {
            foreach (var i in valueTransformers)
                i.PreviewTransform(translation, rotation, translateByLocal, rotateByLocal);
        }

        public void SaveTransform(Vector3 translation, Vector3 rotation, bool translateByLocal, bool rotateByLocal)
        {
            foreach (var i in valueTransformers)
                i.SaveTransform(translation, rotation, translateByLocal, rotateByLocal);
        }

        public void ResetPreview()
        {
            foreach (var i in valueTransformers)
                i.ResetPreview();
        }
    }
}