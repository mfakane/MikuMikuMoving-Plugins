using System.IO;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public readonly record struct AnimationFrame(
    float Progress,
    float Value
)
{
    public static AnimationFrame Lerp(AnimationFrame a, AnimationFrame b, float amount) =>
        new(
            MathHelper.Lerp(a.Progress, b.Progress, amount),
            MathHelper.Lerp(a.Value, b.Value, amount)
        );

    public static AnimationFrame Parse(byte version, BinaryReader br)
    {
        return new(br.ReadSingle(), br.ReadSingle());
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(Progress);
        bw.Write(Value);
    }
}