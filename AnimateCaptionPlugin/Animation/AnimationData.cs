using System.IO;
using System.Linq;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public class AnimationData
{
	public AnimatedProperty X { get; private init; }
	public AnimatedProperty Y { get; private init; }
	public AnimatedProperty Alpha { get; private init; }
	public AnimatedProperty Rotation { get; private init; }
	public AnimatedProperty FontSize { get; private init; }
	public AnimatedProperty LineSpacing { get; private init; }
	public AnimatedProperty LetterSpacing { get; private init; }
	public AnimatedProperty ShadowDistance { get; private init; }

	AnimatedProperty[] Properties => new[]
	{
		X,
		Y,
		Alpha,
		Rotation,
		FontSize,
		LineSpacing,
		LetterSpacing,
		ShadowDistance,
	};
	
	public AnimationData()
	{
		X = new();
		Y = new();
		Alpha = new();
		Rotation = new();
		FontSize = new();
		LineSpacing = new();
		LetterSpacing = new();
		ShadowDistance = new();
	}

	public AnimationData(ICaption caption)
		: this()
	{
		Add(0, CaptionProperties.FromCaption(caption));
		Add(1, CaptionProperties.FromCaption(caption));
	}
	
	public AnimationData(Caption caption)
		: this()
	{
		Add(0, CaptionProperties.FromCaption(caption));
		Add(1, CaptionProperties.FromCaption(caption));
	}

	public static AnimationData Parse(byte version, BinaryReader br, out int captionIndex)
	{
		captionIndex = br.ReadInt32();
		
		return new()
		{
			X = AnimatedProperty.Parse(version, br),
			Y = AnimatedProperty.Parse(version, br),
			Alpha = AnimatedProperty.Parse(version, br),
			Rotation = AnimatedProperty.Parse(version, br),
			FontSize = AnimatedProperty.Parse(version, br),
			LineSpacing = AnimatedProperty.Parse(version, br),
			LetterSpacing = AnimatedProperty.Parse(version, br),
			ShadowDistance = AnimatedProperty.Parse(version, br),
		};
	}

	public void Write(BinaryWriter bw, int captionIndex)
	{
		bw.Write(captionIndex);

		foreach (var property in Properties)
			property.Write(bw);
	}

	public void AddKeyFrame(FrameTime time)
	{
		foreach (var property in Properties)
			AddKeyFrameTo(property, time);
	}

	public void RemoveKeyFrame(int index)
	{
		foreach (var property in Properties)
		{
			property.KeyFrames.RemoveAt(index);
		}
	}

	public void MoveKeyFrame(int index, float progress)
	{
		if (progress <= 0 || progress >= 1) return;
		
		foreach (var property in Properties)
		{
			var keyFrame = property.KeyFrames.Values[index] with { Progress = progress };
			
			property.KeyFrames.RemoveAt(index);
			property.KeyFrames.Add(keyFrame.Progress, keyFrame);
		}
	}

	public void UpdateProperties(CaptionProperties fromValues, CaptionProperties toValues)
	{
		var diff = toValues - fromValues;
		
		X.Add(diff.X);
		Y.Add(diff.Y);
		Alpha.Add(diff.Alpha);
		Rotation.Add(diff.Rotation);
		FontSize.Add(diff.FontSize);
		LineSpacing.Add(diff.LineSpacing);
		LetterSpacing.Add(diff.LetterSpacing);
		ShadowDistance.Add(diff.ShadowDistance);
	}

	void AddKeyFrameTo(AnimatedProperty entry, FrameTime time)
	{
		if (time.Progress <= 0 || time.Progress >= 1) return;
		
		entry.KeyFrames.Add(time.Progress, new AnimationFrame(time.Progress, entry.GetFrame(time).Value));
	}

	void Add(float progress, CaptionProperties values)
	{
		X.KeyFrames.Add(progress, new AnimationFrame(progress, values.X));
		Y.KeyFrames.Add(progress, new AnimationFrame(progress, values.Y));
		Alpha.KeyFrames.Add(progress, new AnimationFrame(progress, values.Alpha));
		Rotation.KeyFrames.Add(progress, new AnimationFrame(progress, values.Rotation));
		FontSize.KeyFrames.Add(progress, new AnimationFrame(progress, values.FontSize));
		LineSpacing.KeyFrames.Add(progress, new AnimationFrame(progress, values.LineSpacing));
		LetterSpacing.KeyFrames.Add(progress, new AnimationFrame(progress, values.LetterSpacing));
		ShadowDistance.KeyFrames.Add(progress, new AnimationFrame(progress, values.ShadowDistance));
	}

	CaptionProperties GetInitialValues() =>
		new()
		{
			X = X.KeyFrames.Values[0].Value,
			Y = Y.KeyFrames.Values[0].Value,
			Alpha = Alpha.KeyFrames.Values[0].Value,
			Rotation = Rotation.KeyFrames.Values[0].Value,
			FontSize = FontSize.KeyFrames.Values[0].Value,
			LineSpacing = LineSpacing.KeyFrames.Values[0].Value,
			LetterSpacing = LetterSpacing.KeyFrames.Values[0].Value,
			ShadowDistance = ShadowDistance.KeyFrames.Values[0].Value,
		};
	
	CaptionProperties GetValuesAtTime(FrameTime elapsedTime) =>
		new()
		{
			X = X.GetFrame(elapsedTime).Value,
			Y = Y.GetFrame(elapsedTime).Value,
			Alpha = Alpha.GetFrame(elapsedTime).Value,
			Rotation = Rotation.GetFrame(elapsedTime).Value,
			FontSize = FontSize.GetFrame(elapsedTime).Value,
			LineSpacing = LineSpacing.GetFrame(elapsedTime).Value,
			LetterSpacing = LetterSpacing.GetFrame(elapsedTime).Value,
			ShadowDistance = ShadowDistance.GetFrame(elapsedTime).Value,
		};

	public void Apply(ICaption caption, float currentFrame)
	{
		var values = GetValuesAtTime(caption.GetTimeFromFrame(currentFrame));

		values.Apply(caption);
	}

	public void Reset(ICaption caption)
	{
		var values = GetInitialValues();
		
		values.Apply(caption);
	}
}