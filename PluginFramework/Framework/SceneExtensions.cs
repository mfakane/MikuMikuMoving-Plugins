using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework;

public static class SceneExtensions
{
    public static string Localize(this Scene scene, string ja, string en) =>
        scene.Language == "ja" ? ja : en;
    
    /// <summary>
    /// 可能な場合、キーフレームをアンドゥスタックに追加します。
    /// </summary>
    /// <param name="scene">現在のシーン。</param>
    /// <param name="recordAll">すべてのキーフレームを記録する場合は true。選択中のキーフレームを記録する場合は false。</param>
    /// <returns>アンドゥスタックが有効な範囲を示す <see cref="IDisposable"/>。</returns>
    public static IDisposable BeginUndoBlock(this Scene scene, bool recordAll = false) =>
		UndoBlock.IsUndoSupported(scene) ? new UndoBlock(scene, recordAll) : Disposable.DoNothing;

    class UndoBlock : IDisposable
    {
	    readonly Scene scene;
	    readonly bool recordAll;
	    readonly dynamic? controller;

	    public UndoBlock(Scene scene, bool recordAll)
	    {
		    this.scene = scene;
		    this.recordAll = recordAll;

		    var controllerField = scene.GetType().GetField("Controller", BindingFlags.Instance | BindingFlags.NonPublic);

		    controller = ExposedObject.Create(controllerField!.GetValue(scene));
		    PrepareUndo();
	    }
	    
	    public static bool IsUndoSupported(Scene scene) =>
		    scene.GetType().GetField("Controller", BindingFlags.Instance | BindingFlags.NonPublic) is { } field &&
		    field.GetValue(scene)?.GetType().GetField("UndoSystem", BindingFlags.Instance | BindingFlags.NonPublic) is { };

	    public void Dispose()
	    {
		    Commit();
	    }

	    IEnumerable<IKeyFrameBuffer> GetKeyFrameBuffers()
	    {
		    foreach (var model in scene.Models)
		    {
			    yield return KeyFrameBuffer.Create(model.PropertyFrames.GetKeyFrames(),
				    model.PropertyFrames.ReplaceAllKeyFrames);

			    foreach (var layer in model.Bones.SelectMany(x => x.Layers))
				    yield return KeyFrameBuffer.Create(layer.Frames.GetKeyFrames(), layer.Frames.ReplaceAllKeyFrames);
		    }

		    foreach (var layer in scene.Accessories.SelectMany(x => x.Layers))
			    yield return KeyFrameBuffer.Create(layer.Frames.GetKeyFrames(), layer.Frames.ReplaceAllKeyFrames);

		    foreach (var effect in scene.Effects)
			    yield return KeyFrameBuffer.Create(effect.Frames.GetKeyFrames(), effect.Frames.ReplaceAllKeyFrames);

		    foreach (var light in scene.Lights)
			    yield return KeyFrameBuffer.Create(light.Frames.GetKeyFrames(), light.Frames.ReplaceAllKeyFrames);

		    foreach (var layer in scene.Cameras.SelectMany(x => x.Layers))
			    yield return KeyFrameBuffer.Create(layer.Frames.GetKeyFrames(), layer.Frames.ReplaceAllKeyFrames);
	    }

	    void PrepareUndo()
	    {
		    var buffers = GetKeyFrameBuffers().ToArray();

		    if (recordAll) foreach (var buffer in buffers) buffer.SelectAll();
		    controller?.ActiveObject.AddSelectedToRemoveFrameData();

		    if (recordAll) foreach (var buffer in buffers) buffer.RestoreSelection();
	    }

	    void Commit()
	    {
		    var buffers = GetKeyFrameBuffers().ToArray();

		    if (recordAll) foreach (var buffer in buffers) buffer.SelectAll();
		    controller?.ActiveObject.AddSelectedToAddFrameData();

		    if (recordAll) foreach (var buffer in buffers) buffer.RestoreSelection();
		    controller?.UndoSystem.PushUndo();
	    }

	    interface IKeyFrameBuffer
	    {
		    void SelectAll();
		    void RestoreSelection();
	    }

	    static class KeyFrameBuffer
	    {
		    public static KeyFrameBuffer<T> Create<T>(List<T> originalKeyFrames, Action<List<T>> replaceAllKeyFrames)
			    where T : FrameData =>
			    new(
				    originalKeyFrames,
				    originalKeyFrames.Select(x =>
				    {
					    var frameData = (T)x.Clone();
					    frameData.Selected = true;
					    return frameData;
				    }).ToList(),
				    replaceAllKeyFrames
			    );
	    }

	    readonly record struct KeyFrameBuffer<T>(
		    List<T> OriginalKeyFrames,
		    List<T> SelectedKeyFrames,
		    Action<List<T>> ReplaceAllKeyFrames
	    ) : IKeyFrameBuffer
		    where T : FrameData
	    {
		    public void SelectAll() => ReplaceAllKeyFrames(SelectedKeyFrames);
		    public void RestoreSelection() => ReplaceAllKeyFrames(OriginalKeyFrames);
	    }
    }
}