using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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

		public static object Member(this object self, string name, params object[] args)
		{
			var m = self.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

			if (m is MethodInfo)
				return ((MethodInfo)m).Invoke(self, args);
			else if (m is PropertyInfo)
				return ((PropertyInfo)m).GetValue(self, args);
			else if (m is FieldInfo)
				return ((FieldInfo)m).GetValue(self);
			else
				return null;
		}

		public static void SetMember(this object self, string name, object value, params object[] args)
		{
			var m = self.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();

			if (m is PropertyInfo)
				((PropertyInfo)m).SetValue(self, value, args);
			else if (m is FieldInfo)
				((FieldInfo)m).SetValue(self, value);
			else
				throw new InvalidOperationException();
		}
	}
}
