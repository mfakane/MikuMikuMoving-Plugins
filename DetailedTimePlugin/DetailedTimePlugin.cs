using System;
using System.IO;
using System.Windows.Forms;
using JetBrains.Annotations;
using Linearstar.MikuMikuMoving.Framework;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.DetailedTimePlugin;

[UsedImplicitly]
public class DetailedTimePlugin : ResidentBase, ICanSavePlugin, IHaveUserControl
{
    NonInteractiveTextImage? statusImage;
    readonly DetailedTimeControl control = new();

    public override void Initialize()
    {
        statusImage = new NonInteractiveTextImage(Scene)
        {
            VisibleWhenPlaying = true,
        };
        Scene.ScreenObjects.Add(statusImage);
    }

    public override void Update(float frame, float elapsedTime)
    {
        if (statusImage == null) return;

        statusImage.Visible = Scene.State != SceneState.AVI_Rendering;

        if (!statusImage.Visible) return;

        var newStatusText = GetStatusText(frame == 0 ? Scene.MarkerPosition : (long)frame);

        statusImage.Text = newStatusText;
        statusImage.Position = new(Scene.ScreenSize.Width - statusImage.Image.Width - 68, 2);
    }

    public override void Dispose()
    {
        if (statusImage != null)
        {
            Scene.ScreenObjects.Remove(statusImage);
            statusImage.Dispose();
            statusImage = null;
        }

        base.Dispose();
    }

    public void OnLoadProject(Stream stream)
    {
        using var br = new BinaryReader(stream);

        control.BeginFrame = br.ReadInt64();
        control.BeatsPerMinute = br.ReadSingle();
        control.BeatsPerMeasure = br.ReadInt32();
        control.Resolution = br.ReadInt32();
    }

    public Stream OnSaveProject()
    {
        var ms = new MemoryStream();
        using var bw = new BinaryWriter(ms);

        bw.Write(control.BeginFrame);
        bw.Write(control.BeatsPerMinute);
        bw.Write(control.BeatsPerMeasure);
        bw.Write(control.Resolution);
        bw.Flush();
        ms.Position = 0;

        return ms;
    }

    public UserControl CreateControl() => control;

    string GetStatusText(float frame)
    {
        frame -= control.BeginFrame;

        if (frame < 0)
            return $"{TimeSpan.Zero:hh':'mm':'ss'.'fff} Meas {0,3:0}:{0:0}:{0,4:0}";

        var beatsPerMeas = control.BeatsPerMeasure;
        var time = TimeSpan.FromSeconds(frame / Scene.KeyFramePerSec);
        var beatFrames = GetFramesPerBeat(control.BeatsPerMinute, Scene.KeyFramePerSec);
        var totalBeats = frame / beatFrames;
        var meas = totalBeats / beatsPerMeas + 1;
        var beats = totalBeats % beatsPerMeas + 1;
        var ticks = (totalBeats - (int)totalBeats) * control.Resolution;

        return $"{time:hh':'mm':'ss'.'fff} Meas {(int)meas,3:0}:{(int)beats:0}:{(int)ticks,4:0}";
    }

    static double GetFramesPerBeat(float beatsPerMinute, float framesPerSecond) =>
        framesPerSecond / (beatsPerMinute / 60.0);

    public override string EnglishText => "Detailed Time";

    public override string Text => "詳細時間";

    public override string Description => Localize("小節単位の時間を表示します。", "Shows detailed time in measures.");

    public override Guid GUID => new("cf05d377-8b6e-4254-b67c-240fe2a4fea8");
}