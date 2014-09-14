using System;
using System.Collections.Generic;
using System.IO;

namespace Linearstar.MikuMikuMoving.Framework
{
	static partial class Util
	{
		public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> self)
		{
			var rt = self.First;

			if (rt != null)
				do
					yield return rt;
				while ((rt = rt.Next) != null);
		}

		public static void CopyTo(this Stream self, Stream destination, int bufferSize)
		{
			var buf = new byte[bufferSize];
			var c = 0;

			while ((c = self.Read(buf, 0, buf.Length)) > 0)
				destination.Write(buf, 0, c);
		}

		public static void CopyTo(this Stream self, Stream destination)
		{
			CopyTo(self, destination, 4096);
		}

		public static string EnvorinmentNewLine(string s)
		{
			return s.Replace("\r\n", Environment.NewLine);
		}

		public static bool IsEnglish(string lang)
		{
			return lang == "en";
		}

		public static string Bilingual(string lang, string ja, string en)
		{
			return IsEnglish(lang) ? en : ja;
		}
	}
}
