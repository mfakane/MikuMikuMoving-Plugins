using System;

namespace Linearstar.MikuMikuMoving.Framework
{
	class Lazy<T>
	{
		T value;
		Func<T> constructor;

		public bool HasInitialized
		{
			get;
			private set;
		}

		public T Value
		{
			get
			{
				return this.HasInitialized ? value : (value = constructor());
			}
		}

		public Lazy(Func<T> constructor)
		{
			this.constructor = constructor;
		}
	}
}
