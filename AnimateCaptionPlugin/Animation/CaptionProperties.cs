using System;
using MikuMikuPlugin;

namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public sealed class CaptionProperties : IEquatable<CaptionProperties>
{
	public float X { get; init; }
	public float Y { get; init; }
	public float Alpha { get; init; }
	public float Rotation { get; init; }
	public float FontSize { get; init; }
	public float LineSpacing { get; init; }
	public float LetterSpacing { get; init; }
	public float ShadowDistance { get; init; }

	public static CaptionProperties FromCaption(ICaption caption) =>
		new()
		{
			X = caption.Location.X,
			Y = caption.Location.Y,
			Alpha = caption.Alpha,
			Rotation = caption.Rotate,
			FontSize = caption.FontSize,
			LineSpacing = caption.LineSpace,
			LetterSpacing = caption.LetterSpace,
			ShadowDistance = caption.ShadowDistance,
		};
	
	public static CaptionProperties FromCaption(Caption caption) =>
		new()
		{
			X = caption.Location.X,
			Y = caption.Location.Y,
			Alpha = caption.Alpha,
			Rotation = caption.Rotate,
			FontSize = caption.FontSize,
			LineSpacing = caption.LineSpace,
			LetterSpacing = caption.LetterSpace,
			ShadowDistance = caption.ShadowDistance,
		};

	public void Apply(ICaption caption)
	{
		caption.Location = new(X, Y, 0);
		caption.Alpha = Alpha;
		caption.Rotate = Rotation;
		caption.FontSize = Math.Max(FontSize, 0.1f);
		caption.LineSpace = LineSpacing;
		caption.LetterSpace = LetterSpacing;
		caption.ShadowDistance = ShadowDistance;
	}

	public static CaptionProperties operator +(CaptionProperties a, CaptionProperties b) =>
		new()
		{
			X = a.X + b.X,
			Y = a.Y + b.Y,
			Alpha = a.Alpha + b.Alpha,
			Rotation = a.Rotation + b.Rotation,
			FontSize = a.FontSize + b.FontSize,
			LineSpacing = a.LineSpacing + b.LineSpacing,
			LetterSpacing = a.LetterSpacing + b.LetterSpacing,
			ShadowDistance = a.ShadowDistance + b.ShadowDistance,
		};

	public static CaptionProperties operator -(CaptionProperties a, CaptionProperties b) =>
		new()
		{
			X = a.X - b.X,
			Y = a.Y - b.Y,
			Alpha = a.Alpha - b.Alpha,
			Rotation = a.Rotation - b.Rotation,
			FontSize = a.FontSize - b.FontSize,
			LineSpacing = a.LineSpacing - b.LineSpacing,
			LetterSpacing = a.LetterSpacing - b.LetterSpacing,
			ShadowDistance = a.ShadowDistance - b.ShadowDistance,
		};

	public bool Equals(CaptionProperties? other)
	{
		const float tolerance = 0.1f;
		
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Math.Abs(X - other.X) < tolerance && 
		       Math.Abs(Y - other.Y) < tolerance && 
		       Alpha.Equals(other.Alpha) && 
		       Rotation.Equals(other.Rotation) &&
		       FontSize.Equals(other.FontSize) &&
		       LineSpacing.Equals(other.LineSpacing) &&
		       LetterSpacing.Equals(other.LetterSpacing) &&
		       ShadowDistance.Equals(other.ShadowDistance);
	}

	public int GetDifference(CaptionProperties other)
	{
		const float tolerance = 0.1f;
		var result = 0;
		
		if (Math.Abs(X - other.X) >= tolerance) result++;
		if (Math.Abs(Y - other.Y) >= tolerance) result++;
		if (!Alpha.Equals(other.Alpha)) result++;
		if (!Rotation.Equals(other.Rotation)) result++;
		if (!FontSize.Equals(other.FontSize)) result++;
		if (!LineSpacing.Equals(other.LineSpacing)) result++;
		if (!LetterSpacing.Equals(other.LetterSpacing)) result++;
		if (!ShadowDistance.Equals(other.ShadowDistance)) result++;

		return result;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != typeof(CaptionProperties)) return false;
		return Equals((CaptionProperties)obj);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			var hashCode = X.GetHashCode();
			hashCode = (hashCode * 397) ^ Y.GetHashCode();
			hashCode = (hashCode * 397) ^ Alpha.GetHashCode();
			hashCode = (hashCode * 397) ^ Rotation.GetHashCode();
			hashCode = (hashCode * 397) ^ FontSize.GetHashCode();
			hashCode = (hashCode * 397) ^ LineSpacing.GetHashCode();
			hashCode = (hashCode * 397) ^ LetterSpacing.GetHashCode();
			hashCode = (hashCode * 397) ^ ShadowDistance.GetHashCode();
			return hashCode;
		}
	}

	public static bool operator ==(CaptionProperties? left, CaptionProperties? right) => Equals(left, right);

	public static bool operator !=(CaptionProperties? left, CaptionProperties? right) => !Equals(left, right);
}
