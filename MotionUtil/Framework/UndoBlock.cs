using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.Framework
{
	public class UndoBlock : IDisposable
	{
		readonly Scene scene;
		readonly object controller;

		public UndoBlock(Scene scene)
		{
			this.scene = scene;
			controller = scene.Member("Controller");

			PushUndo();
		}

		public void Dispose()
		{
			SetCurrentUndo();
		}

		void CopySequence()
		{
			switch (scene.Mode)
			{
				case EditMode.ModelMode:
					foreach (var i in scene.Models)
					{
						// FIXME: internal bug
						// i.PropertyFrames.ReplaceAllKeyFrames(i.PropertyFrames.GetKeyFrames());

						foreach (var j in i.Bones.SelectMany(_ => _.Layers).Select(_ => _.Frames))
							j.ReplaceAllKeyFrames(j.GetKeyFrames());
					}

					break;
				case EditMode.AccessoryMode:
					// FIXME: property frames
					foreach (var i in scene.Accessories.SelectMany(_ => _.Layers).Select(_ => _.Frames))
						i.ReplaceAllKeyFrames(i.GetKeyFrames());

					break;
				case EditMode.EffectMode:
					// FIXME: property frames
					foreach (var i in scene.Effects.Select(_ => _.Frames))
						i.ReplaceAllKeyFrames(i.GetKeyFrames());

					break;
				case EditMode.CameraMode:
					// FIXME: may lose perspective flag, CameraPropertyFrame needed
					foreach (var i in scene.Cameras.SelectMany(_ => _.Layers).Select(_ => _.Frames))
						i.ReplaceAllKeyFrames(i.GetKeyFrames());

					foreach (var i in scene.Lights.Select(_ => _.Frames))
						i.ReplaceAllKeyFrames(i.GetKeyFrames());

					scene.PropertyFrames.ReplaceAllKeyFrames(scene.PropertyFrames.GetKeyFrames());

					break;
			}

			/*var olist = (IDictionary)controller.Member("ObjectList");

			foreach (var i in scene.Models)
			{
				var obj = olist[i.ID];
				var type = obj.GetType();
				var propSeq = obj.Member("propSequence");
				var propCopy = ((IList)propSeq.Member("propdatalist")).Cast<object>().Select(DeepCopy).ToArray();

				propSeq.Member("ClearFrames");

				foreach (var j in propCopy)
					propSeq.Member("RegistProperty", j);

				var motionData = (IList<IList>)obj.Member("sequence").Member("motiondata");
				var registMotionFrame = type.GetMethods().First(_ => _.Name == "RegistFrame" && _.GetParameters().Select(p => p.ParameterType.Name).SequenceEqual(new[] { "Int32", "Int32", "Int64", "FrameData", "PropertyData" }));
				var clearFrames = type.GetMethod("ClearFrames", new[] { typeof(int), typeof(int) });

				foreach (var j in i.Bones.SelectMany(_ => _.Layers).Select(_ => _.Frames))
				{
					var bid = (int)j.Member("BoneID");
					var sid = (int)j.Member("LayerID");
					var frames = (IList)motionData[bid][sid];
					var copy = frames.Cast<object>().Select(DeepCopy).ToArray();

					clearFrames.Invoke(obj, new object[] { bid, sid });

					foreach (var k in copy)
						registMotionFrame.Invoke(obj, new object[] { bid, sid, (long)k.Member("FrameNumber"), k, null });
				}
			}*/
		}

		void PushUndo()
		{
			var resetSelection = new List<Action>();

			foreach (var i in scene.Models.SelectMany(o => o.Bones).SelectMany(b => b.Layers)
				.Concat(scene.Accessories.SelectMany(o => o.Layers))
				.SelectMany(_ => _.Frames)
				.Concat(scene.Effects.SelectMany(l => l.Frames)).Select(f => Tuple.Create<Func<bool>, Action<bool>>(() => f.Selected, _ => f.Selected = _))
				.Concat(scene.Lights.SelectMany(l => l.Frames).Select(f => Tuple.Create<Func<bool>, Action<bool>>(() => f.Selected, _ => f.Selected = _)))
				.Concat(scene.Cameras.SelectMany(o => o.Layers).SelectMany(l => l.Frames).Select(f => Tuple.Create<Func<bool>, Action<bool>>(() => f.Selected, _ => f.Selected = _))))
			{
				var selected = i.Item1();

				resetSelection.Add(() => i.Item2(selected));
				i.Item2(true);
			}

			controller.Member("ActiveObject").Member("AddSelectedToRemoveFrameData");

			foreach (var i in resetSelection)
				i();

			CopySequence();
		}

		void SetCurrentUndo()
		{
			var resetSelection = new List<Action>();

			foreach (var i in scene.Models.SelectMany(o => o.Bones).SelectMany(b => b.Layers)
				.Concat(scene.Accessories.SelectMany(o => o.Layers))
				.SelectMany(_ => _.Frames)
				.Concat(scene.Effects.SelectMany(l => l.Frames)).Select(f => Tuple.Create<Func<bool>, Action<bool>>(() => f.Selected, _ => f.Selected = _))
				.Concat(scene.Lights.SelectMany(l => l.Frames).Select(f => Tuple.Create<Func<bool>, Action<bool>>(() => f.Selected, _ => f.Selected = _)))
				.Concat(scene.Cameras.SelectMany(o => o.Layers).SelectMany(l => l.Frames).Select(f => Tuple.Create<Func<bool>, Action<bool>>(() => f.Selected, _ => f.Selected = _))))
			{
				var selected = i.Item1();

				resetSelection.Add(() => i.Item2(selected));
				i.Item2(true);
			}

			controller.Member("ActiveObject").Member("AddSelectedToAddFrameData");

			foreach (var i in resetSelection)
				i();

			controller.Member("UndoSystem").Member("PushUndo");
		}

		static readonly MethodInfo memberwiseClone = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
		static readonly Type listType = typeof(List<>);
		static readonly Type dictionaryType = typeof(Dictionary<,>);

		static object DeepCopy(object obj)
		{
			if (obj == null)
				return null;

			var type = obj.GetType();

			if (type.IsGenericType)
			{
				var genericTypeDefinition = type.GetGenericTypeDefinition();

				if (genericTypeDefinition == listType)
				{
					var obj2 = Activator.CreateInstance(listType.MakeGenericType(type.GetGenericArguments()), null);
					var method = obj2.GetType().GetMethod("Add");

					foreach (var i in (IList)obj)
						method.Invoke(obj2, new[] { DeepCopy(i) });

					return obj2;
				}
				else if (genericTypeDefinition == dictionaryType)
				{
					var obj2 = Activator.CreateInstance(dictionaryType.MakeGenericType(type.GetGenericArguments()), null);
					var method2 = obj2.GetType().GetMethod("Add");

					foreach (DictionaryEntry i in (IDictionary)obj)
						method2.Invoke(obj2, new[] { i.Key, DeepCopy(i.Value) });

					return obj2;
				}
			}

			if (type.Name == "MediaFile")
				return obj;

			var clone = type.GetMethod("Clone");

			if (clone != null)
				return clone.Invoke(obj, null);

			obj = memberwiseClone.Invoke(obj, null);

			foreach (var i in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(_ => _.FieldType.IsClass))
				i.SetValue(obj, DeepCopy(i.GetValue(obj)));

			return obj;
		}
	}
}
