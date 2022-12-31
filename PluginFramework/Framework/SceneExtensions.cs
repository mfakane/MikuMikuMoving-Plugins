using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
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
			    foreach (var morph in model.Morphs)
				    yield return KeyFrameBuffer.Create(
					    morph.Frames.GetKeyFrames(),
					    morph.Frames.ReplaceAllKeyFrames,
					    morph.Selected,
					    x =>
					    {
						    if (morph.Selected == x) return;
						    morph.Selected = x;
					    }
				    );
			    
			    foreach (var layer in model.Bones.SelectMany(x => x.Layers))
				    yield return KeyFrameBuffer.Create(
					    layer.Frames.GetKeyFrames(),
					    layer.Frames.ReplaceAllKeyFrames,
					    layer.Selected,
					    x =>
					    {
						    if (layer.Selected == x) return;
						    layer.Selected = x;
					    }
				    );
		    }

		    foreach (var layer in scene.Accessories.SelectMany(x => x.Layers))
			    yield return KeyFrameBuffer.Create(
				    layer.Frames.GetKeyFrames(),
				    layer.Frames.ReplaceAllKeyFrames,
				    layer.GetSelected(),
				    x => layer.SetSelected(x)
			    );

		    foreach (var effect in scene.Effects)
			    yield return KeyFrameBuffer.Create(
				    effect.Frames.GetKeyFrames(),
				    effect.Frames.ReplaceAllKeyFrames,
				    effect.GetSelected(),
				    x => effect.SetSelected(x)
				);

		    foreach (var light in scene.Lights)
			    yield return KeyFrameBuffer.Create(light.Frames.GetKeyFrames(), light.Frames.ReplaceAllKeyFrames);

		    foreach (var layer in scene.Cameras.SelectMany(x => x.Layers))
			    yield return KeyFrameBuffer.Create(
				    layer.Frames.GetKeyFrames(),
				    layer.Frames.ReplaceAllKeyFrames,
				    layer.GetSelected(),
				    x => layer.SetSelected(x)
				);
	    }

	    void PrepareUndo()
	    {
		    if (controller?.ActiveObject == null) return;

		    var buffers = GetKeyFrameBuffers().ToArray();

		    foreach (var buffer in buffers)
		    {
			    if (recordAll)
					buffer.SelectAll();
			    else
				    buffer.SelectIfNeeded();
		    }
		    
		    controller.ActiveObject.AddSelectedToRemoveFrameData();

		    if (recordAll)
			    foreach (var buffer in buffers)
				    buffer.RestoreSelection();
	    }

	    void Commit()
	    {
		    if (controller?.ActiveObject == null) return;
		    
		    var buffers = GetKeyFrameBuffers().ToArray();

		    foreach (var buffer in buffers)
		    {
			    if (recordAll)
				    buffer.SelectAll();
			    else
				    buffer.SelectIfNeeded();
		    }
		    
		    controller.ActiveObject.AddSelectedToAddFrameData();
		    controller.UndoSystem.PushUndo();

		    if (recordAll)
				foreach (var buffer in buffers)
					buffer.RestoreSelection();
	    }

	    interface IKeyFrameBuffer
	    {
		    void SelectIfNeeded();
		    void SelectAll();
		    void RestoreSelection();
	    }

	    static class KeyFrameBuffer
	    {
		    public static KeyFrameBuffer<T> Create<T>(
			    List<T> originalKeyFrames,
			    Action<List<T>> replaceAllKeyFrames,
			    bool? isOwnerOriginallySelected = null,
			    Action<bool>? setOwnerSelected = null
		    )
			    where T : FrameData =>
			    new(
				    originalKeyFrames,
				    originalKeyFrames.Select(x =>
				    {
					    var frameData = (T)x.Clone();
					    frameData.Selected = true;
					    return frameData;
				    }).ToList(),
				    replaceAllKeyFrames,
				    isOwnerOriginallySelected,
				    setOwnerSelected
			    );
	    }

	    readonly record struct KeyFrameBuffer<T>(
		    List<T> OriginalKeyFrames,
		    List<T> SelectedKeyFrames,
		    Action<List<T>> ReplaceAllKeyFrames,
		    bool? IsOwnerOriginallySelected,
		    Action<bool>? SetOwnerSelected
	    ) : IKeyFrameBuffer
		    where T : FrameData
	    {
		    public void SelectIfNeeded()
		    {
			    if (IsOwnerOriginallySelected != true && OriginalKeyFrames.Any(x => x.Selected))
					SetOwnerSelected?.Invoke(true);
		    }

		    public void SelectAll()
		    {
			    if (IsOwnerOriginallySelected.HasValue) SetOwnerSelected?.Invoke(true);
			    ReplaceAllKeyFrames(SelectedKeyFrames);
		    }

		    public void RestoreSelection()
		    {
			    if (IsOwnerOriginallySelected.HasValue) SetOwnerSelected?.Invoke(IsOwnerOriginallySelected.Value);
			    ReplaceAllKeyFrames(OriginalKeyFrames);
		    }
	    }
    }
}