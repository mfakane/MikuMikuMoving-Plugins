using System;
using System.Collections.Generic;
using System.IO;

namespace Linearstar.Keystone.IO
{
	static class Util
	{
		public static IEnumerable<T> Repeat<T>(T element)
		{
			while (true)
				yield return element;
		}

		public static long GetRemainingLength(this BinaryReader bw)
		{
			return bw.BaseStream.Length - bw.BaseStream.Position;
		}

		public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
		{
			foreach (var i in self)
				action(i);
		}
	}
}
