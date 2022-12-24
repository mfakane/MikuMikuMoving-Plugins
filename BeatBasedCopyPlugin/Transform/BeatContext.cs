using System;
using System.Collections.Generic;
using System.Linq;

namespace Linearstar.MikuMikuMoving.BeatBasedCopyPlugin.Transform;

public class BeatContext
{
    public float StartupBeats { get; }
    public float BeatsPerMinute { get; }
    public float IntervalBeats { get; }
    public int RepeatCount { get; }
    public float FramesPerSecond { get; }

    public BeatContext(float startupBeats, float beatsPerMinute, float intervalBeats, int repeatCount, float framesPerSecond)
    {
        StartupBeats = startupBeats;
        BeatsPerMinute = beatsPerMinute;
        IntervalBeats = intervalBeats;
        RepeatCount = repeatCount;
        FramesPerSecond = framesPerSecond;
    }
    
    public IEnumerable<long> GetDestinationFrames(long markerPosition)
    {
        var framesPerBeat = FramesPerSecond / (BeatsPerMinute / 60f);
        var startsAt = StartupBeats * framesPerBeat;
        var intervalBeats = Math.Max(1, IntervalBeats);

        foreach (var item in GetBeats(framesPerBeat)
                     .Select(x => x + startsAt)
                     .Where(x => markerPosition <= x)
                     .Take((int)(RepeatCount * intervalBeats))
                     .Select((frameNumber, index) => new
                     {
                         FrameNumber = (long)frameNumber,
                         Index = index,
                     }))
            if (item.Index % IntervalBeats == 0)
                yield return item.FrameNumber;
    }

    static IEnumerable<long> GetBeats(float framesPerBeat)
    {
        for (var i = 0; ; i++)
            yield return (long)framesPerBeat * i;
    }
}