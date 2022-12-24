using System;
using System.Collections.Generic;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.GetMmdTransformationPlugin.Transform;

public class MorphTransformer : ITransformer
{
    readonly Morph morph;
    readonly State initialState;

    MorphTransformer(Morph morph)
    {
        this.morph = morph;
        initialState = new State(morph);
    }
    
    public static IEnumerable<MorphTransformer> FromModel(Model model) =>
        model.Morphs
            .Select(x => new MorphTransformer(x));

    public void PreviewTransform(ModelBinding binding)
    {
        if (!binding.Morphs.TryGetValue(morph.MorphID, out var target)) return;

        var mmdMorph = target.MmdMorph;
        var weight = mmdMorph.Weight;

        // 計算誤差で未登録モーフを増やしたくないので、誤差以内は無視する
        if (State.HasDifference(initialState.Weight, weight))
            new State(weight, true).Apply(morph);
        else
            initialState.Apply(morph);
    }

    public void SaveTransform(ModelBinding binding) =>
        PreviewTransform(binding);

    public void ResetPreview() =>
        initialState.Apply(morph);

    record State(float Weight, bool Selected)
    {
        public State(Morph morph)
            : this(morph.CurrentWeight, morph.Selected)
        {
        }

        public static bool HasDifference(float a, float b)
        {
            const float precision = 0.000001f;

            return Math.Abs(a - b) > precision;
        }

        public void Apply(Morph morph)
        {
            // 計算誤差で未登録モーフを増やしたくないので、誤差以内は無視する
            if (HasDifference(morph.CurrentWeight, Weight))
                morph.CurrentWeight = Weight;
            
            morph.Selected = Selected;
        }
    }
}