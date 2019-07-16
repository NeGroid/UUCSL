using System;
using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class SVBlock : IComparable<SVBlock>, IEquatable<SVBlock>
	{
		public string[] Words { get; }

		public SVVector Vector { get; }

		public SVBlock(SVVector key, string words)
		{
			Vector = key;
			Words = words.Split(new[] { Environment.NewLine, " " }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var word in Words)
			{
				string.Intern(word);
			}
		}

		public static SVBlock FromSV(SVVector key, IEnumerable<string> words) =>
			new SVBlock(key, string.Join(' ', words));

		public int CompareTo(SVBlock other) => Vector.CompareTo(other.Vector);

		public bool Equals(SVBlock other)
		{
			if (Words.Length > other.Words.Length)
			{
				return false;
			}

			return Vector.Equals(other.Vector);
		}

		public bool Includes(SVBlock other) => Vector.Includes(other.Vector);

		public override string ToString() => Vector.ToString();
	}
}
