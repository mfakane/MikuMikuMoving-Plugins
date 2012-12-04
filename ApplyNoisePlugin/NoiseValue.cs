using DxMath;
using Linearstar.MikuMikuMoving.Framework;

namespace Linearstar.MikuMikuMoving.ApplyNoisePlugin
{
	public struct NoiseValue
	{
		Quaternion rotationQuaternion;

		public Vector3 PositionWidth
		{
			get;
			set;
		}

		public Vector3 RotationWidth
		{
			get;
			set;
		}

		public float GravityWidth
		{
			get;
			set;
		}

		public Vector3 GravityDirectionWidth
		{
			get;
			set;
		}

		public Quaternion RotationQuaternion
		{
			get
			{
				if (rotationQuaternion != new Quaternion())
					return rotationQuaternion;

				var rot = MathHelper.ToRadians(this.RotationWidth);

				return rotationQuaternion = Quaternion.RotationYawPitchRoll(rot.Y, rot.X, rot.Z);
			}
		}

		public NoiseValue Interpolate(NoiseValue nextValue, float amount)
		{
			return new NoiseValue
			{
				PositionWidth = Vector3.Lerp(this.PositionWidth, nextValue.PositionWidth, amount),
				RotationWidth = Vector3.Lerp(this.RotationWidth, nextValue.RotationWidth, amount),
				GravityWidth = MathHelper.Lerp(this.GravityWidth, nextValue.GravityWidth, amount),
				GravityDirectionWidth = Vector3.Lerp(this.GravityDirectionWidth, nextValue.GravityDirectionWidth, amount),
			};
		}
	}
}
