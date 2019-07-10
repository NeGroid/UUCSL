using System;
using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class SVBlock : IComparable<SVBlock>, IEquatable<SVBlock>
	{
		private SVVector Key;

		public string[] Words { get; private set; }

		public SVVector Vector => Key;

		public SVBlock(SVVector key, string words)
		{
			Key = key;
			Words = words.Split(new [] { Environment.NewLine, " "}, StringSplitOptions.RemoveEmptyEntries);

			foreach(var word in Words)
			{
				string.Intern(word);
			}
		}

		public static SVBlock FromSV(SVVector key, IEnumerable<string> words) =>
			new SVBlock(key, string.Join(' ', words));

		public int CompareTo(SVBlock other) => Key.CompareTo(other.Key);

		public bool Equals(SVBlock other)
		{
			if(Words.Length > other.Words.Length)
			{
				return false;
			}

			return Key.Equals(other.Key);
		}

		public bool Includes(SVBlock other) => Key.Includes(other.Key);

		public override string ToString() => Key.ToString();
	}
}
