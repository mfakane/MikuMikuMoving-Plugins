namespace Linearstar.MikuMikuMoving.AnimateCaptionPlugin.Animation;

public enum AnimationMode : byte
{
	/// <summary>
	/// なし。
	/// </summary>
	None,
	/// <summary>
	/// 線形補間。
	/// </summary>
	LinearInterpolation,
	/// <summary>
	/// 瞬間移動。
	/// </summary>
	NoInterpolation,
	/// <summary>
	/// 中間点無視。
	/// </summary>
	LinearInterpolationFirstAndLastOnly,
	/// <summary>
	/// 移動量指定。
	/// </summary>
	ByAcceleration,
	/// <summary>
	/// ランダム移動。
	/// </summary>
	RandomFirstAndLast,
	/// <summary>
	/// 反復移動。
	/// </summary>
	RepeatFirstAndLast,
}