using System;

namespace Linearstar.MikuMikuMoving.Framework
{
	class FinallyBlock<T> : IDisposable
	{
		T value;
		Action<T> dispose;

		public FinallyBlock(T value, Action<T> dispose)
		{
			this.value = value;
			this.dispose = dispose;
		}

		public void Dispose()
		{
			dispose(value);
		}

		public static implicit operator T(FinallyBlock<T> self)
		{
			return self.value;
		}
	}

	static class FinallyBlock
	{
		public static FinallyBlock<object> Create(Action post)
		{
			return Create(null, post);
		}

		public static FinallyBlock<object> Create(Action pre, Action post)
		{
			if (pre != null)
				pre();

			return new FinallyBlock<object>(null, _ => post());
		}

		public static FinallyBlock<T> Create<T>(T value, Action<T> dispose)
		{
			return new FinallyBlock<T>(value, dispose);
		}
	}
}
