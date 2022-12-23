using System;
using System.Collections.Generic;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin.Transform;

public class NoiseContext
{
    public long FromFrame { get; }
    public long ToFrame { get; }
    public int KeyFrameInterval { get; }
    public int KeyShiftWidth { get; }
    public int NoiseInterval { get; }
    public NoiseValue NoiseWidth { get; }

    public NoiseContext(long fromFrame, long toFrame, int keyFrameInterval, int keyShiftWidth, int noiseInterval, NoiseValue noiseWidth)
    {
        FromFrame = fromFrame;
        ToFrame = toFrame;
        KeyFrameInterval = keyFrameInterval;
        KeyShiftWidth = keyShiftWidth;
        NoiseInterval = noiseInterval;
        NoiseWidth = noiseWidth;
    }
    
    public IEnumerable<NoiseValueWithFrameNumber> GetNoiseValues(ISet<long> existingKeyFrames)
    {
        var random = new Random();

        var nextKeyFrame = FromFrame + KeyFrameInterval;
        var currentNoiseFrame = new NoiseValueWithFrameNumber(
            FromFrame,
            GetShiftedFrameNumber(FromFrame),
            NoiseValue.Create(random, NoiseWidth)
        );
        var nextNoiseFrame = new NoiseValueWithFrameNumber(
            FromFrame + NoiseInterval,
            GetShiftedFrameNumber(FromFrame + NoiseInterval),
            NoiseValue.Create(random, NoiseWidth)
        );
        
        for (var currentFrame = FromFrame; currentFrame <= ToFrame; currentFrame++)
        {
            if (currentFrame >= nextNoiseFrame.FrameNumber)
            {
                // NoiseValue を更新
                currentNoiseFrame = nextNoiseFrame;
                nextNoiseFrame = new NoiseValueWithFrameNumber(
                    nextNoiseFrame.FrameNumber + NoiseInterval,
                    GetShiftedFrameNumber(nextNoiseFrame.FrameNumber + NoiseInterval),
                    NoiseValue.Create(random, NoiseWidth)
                );
            }

            var amount = (float)(currentFrame - currentNoiseFrame.FrameNumber) / (nextNoiseFrame.FrameNumber - currentNoiseFrame.FrameNumber);
            
            if (KeyFrameInterval == 0)
            {
                // 間隔 0 のときはキーフレームが存在する場合のみ上書きする
                if (existingKeyFrames.Contains(currentFrame))
                    yield return new(
                        currentFrame,
                        GetShiftedFrameNumber(currentFrame),
                        NoiseValue.Interpolate(currentNoiseFrame.Value, nextNoiseFrame.Value, amount)
                    );
            }
            else if (currentFrame >= nextKeyFrame || currentFrame == FromFrame)
            {
                // Interval ごとにキーフレームを作成する
                yield return new(
                    currentFrame,
                    GetShiftedFrameNumber(currentFrame),
                    NoiseValue.Interpolate(currentNoiseFrame.Value, nextNoiseFrame.Value, amount)
                );
                
                if (currentFrame > FromFrame)
                    nextKeyFrame += KeyFrameInterval;
            }
        }

        long GetShiftedFrameNumber(long frameNumber) =>
            KeyShiftWidth > 0
                ? Math.Min(Math.Max(FromFrame, frameNumber + random.Next(-KeyShiftWidth, KeyShiftWidth + 1)), ToFrame)
                : frameNumber;
    }
}