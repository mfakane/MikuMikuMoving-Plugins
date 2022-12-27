using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public class AnimatedProperty
{
	public AnimationMode Mode { get; set; }

	public bool EaseIn { get; set; }

	public bool EaseOut { get; set; }

	public int IterationDurationFrames { get; set; }

	public SortedList<float, AnimationFrame> KeyFrames { get; private init; }

	public AnimatedProperty() => 
		KeyFrames = new();

	public static AnimatedProperty Parse(byte version, BinaryReader br)
	{
		if (version > 1) throw new ArgumentException(nameof(version));

		return new()
		{
			Mode = (AnimationMode)br.ReadByte(),
			EaseIn = br.ReadBoolean(),
			EaseOut = br.ReadBoolean(),
			IterationDurationFrames = br.ReadInt32(),
			KeyFrames = new(Enumerable.Range(0, br.ReadInt32()).Select(_ => AnimationFrame.Parse(version, br)).ToDictionary(x => x.Progress)),
		};
	}

	public void Write(BinaryWriter bw)
	{
		bw.Write((byte)Mode);
		bw.Write(EaseIn);
		bw.Write(EaseOut);
		bw.Write(IterationDurationFrames);
		bw.Write(KeyFrames.Count);

		foreach (var i in KeyFrames)
			i.Value.Write(bw);
	}

	public void Add(float value)
	{
		switch (Mode)
		{
			case AnimationMode.None:
			case AnimationMode.LinearInterpolation:
			case AnimationMode.NoInterpolation:
			case AnimationMode.LinearInterpolationFirstAndLastOnly:
			case AnimationMode.RandomFirstAndLast:
			case AnimationMode.RepeatFirstAndLast:
			{
				var allKeyFrames = KeyFrames.Values.ToArray();
				KeyFrames.Clear();

				foreach (var keyFrame in allKeyFrames)
					KeyFrames.Add(keyFrame.Progress, keyFrame with { Value = keyFrame.Value + value });
				break;
			}
			case AnimationMode.ByAcceleration:
			{
				var keyFrame = KeyFrames.Values[0];
				
				KeyFrames[KeyFrames.Keys[0]] = keyFrame with { Value = keyFrame.Value + value };
				
				break;
			}
		}
	}

	public AnimationPair SetPairValue(FrameTime elapsedTime, float newFromValue, float newToValue)
	{
		var pair = GetPairFromTime(elapsedTime);

		pair.FromKeyFrame.SetValue(newFromValue);
		pair.ToKeyFrame.SetValue(newToValue);

		return pair;
	}

	public AnimationPair GetPairFromTime(FrameTime elapsedTime)
	{
		elapsedTime = elapsedTime.Normalize();
		
		switch (Mode)
		{
			case AnimationMode.None:
				return new AnimationPair(
					new KeyFrameNode(KeyFrames, 0),
					new KeyFrameNode(KeyFrames, 0)
				);
			case AnimationMode.LinearInterpolation:
			case AnimationMode.NoInterpolation:
			{
				var fromNode = KeyFrames.Last(x => elapsedTime.Progress >= x.Value.Progress);
				var fromNodeIndex = KeyFrames.IndexOfKey(fromNode.Key);

				return new AnimationPair(
					new KeyFrameNode(KeyFrames, fromNodeIndex),
					new KeyFrameNode(KeyFrames, Math.Min(fromNodeIndex + 1, KeyFrames.Count - 1))
				);
			}
			case AnimationMode.LinearInterpolationFirstAndLastOnly:
			case AnimationMode.ByAcceleration:
			case AnimationMode.RandomFirstAndLast:
			case AnimationMode.RepeatFirstAndLast:
				return new AnimationPair(
					new KeyFrameNode(KeyFrames, 0),
					new KeyFrameNode(KeyFrames, KeyFrames.Count - 1)
				);
			default:
				throw new InvalidOperationException();
		}
	}
	
	float Ease(float t)
	{
		static float EaseInQuad(float t) => t * t;
		static float EaseOutQuad(float t) => 1 - EaseInQuad(1 - t);

		if (EaseIn && EaseOut)
			return t < 0.5f
				? EaseInQuad(t * 2) / 2
				: EaseOutQuad((t - 0.5f) * 2) / 2 + 0.5f;
		
		if (EaseIn) return EaseInQuad(t);
		if (EaseOut) return EaseOutQuad(t);
		
		return t;
	}

	public AnimationFrame GetFrame(FrameTime elapsedTime)
	{
		elapsedTime = elapsedTime.Normalize();
		
		switch (Mode)
		{
			case AnimationMode.None:
				return KeyFrames.Values[0];
			case AnimationMode.LinearInterpolation:
			case AnimationMode.LinearInterpolationFirstAndLastOnly:
			{
				var pair = GetPairFromTime(elapsedTime);

				return pair.Lerp(Ease(pair.GetAmountFromTime(elapsedTime)));
			}
			case AnimationMode.NoInterpolation:
			{
				var pair = GetPairFromTime(elapsedTime);

				return pair.GetAmountFromTime(elapsedTime) < 1
					? pair.FromFrame
					: pair.ToFrame;
			}
			case AnimationMode.ByAcceleration:
			{
				var pair = GetPairFromTime(elapsedTime);

				return pair.GetAcceleratedFrame(elapsedTime);
			}
			case AnimationMode.RandomFirstAndLast:
			{
				var pair = GetPairFromTime(elapsedTime);
				var durationFrames = Math.Max(1, IterationDurationFrames);
				var currentIteration = (int)(elapsedTime.ElapsedFrames / durationFrames);
				var remainingFrames = elapsedTime.ElapsedFrames % durationFrames;

				// 毎回同じ結果を得るため、開始時刻からシードを設定してから現在の繰り返し数に至るまでガチャを引いておく
				var random = new Random((int)(elapsedTime.StartFrame + pair.FromFrame.Value));
				for (var i = 0; i < currentIteration; i++)
					random.NextDouble();

				var minValue = pair.MinValue();
				var valueWidth = pair.MaxValue() - minValue;
				var currentValue = (float)(random.NextDouble() * valueWidth + minValue);
				var nextValue = (float)(random.NextDouble() * valueWidth + minValue);

				return new(
					elapsedTime.Progress, 
					MathHelper.Lerp(currentValue, nextValue, Ease(remainingFrames / durationFrames))
				);
			}
			case AnimationMode.RepeatFirstAndLast:
			{
				var pair = GetPairFromTime(elapsedTime);
				var durationFrames = Math.Max(1, IterationDurationFrames);
				var currentIteration = (int)(elapsedTime.ElapsedFrames / durationFrames);
				var remainingFrames = elapsedTime.ElapsedFrames % durationFrames;
				var currentValue = currentIteration % 2 == 0 ? pair.ToFrame.Value : pair.FromFrame.Value;
				var nextValue = currentIteration % 2 != 0 ? pair.ToFrame.Value : pair.FromFrame.Value;

				return new(
					elapsedTime.Progress, 
					MathHelper.Lerp(currentValue, nextValue, Ease(remainingFrames / durationFrames))
				);
			}
			default:
				throw new InvalidOperationException();
		}
	}

	readonly struct KeyFrameNode : IAnimationKeyFrame
	{
		readonly SortedList<float, AnimationFrame> list;
		readonly int index;

		public KeyFrameNode(SortedList<float, AnimationFrame> list, int index)
		{
			this.list = list;
			this.index = index;
		}

		public AnimationFrame Frame => list.Values[index];

		public void SetValue(float value) =>
			list[list.Keys[index]] = list.Values[index] with { Value = value };
	}
}