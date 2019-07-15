using System;
using System.Linq;
using System.Text;

namespace UUCSL.Core
{
	public class SVVector : IComparable<SVVector>, IEquatable<SVVector>
	{
		public byte[] Value { get; private set; }

		public int Length => Value.Length;

		private SVVector(string key)
		{
			Value = Encoding.ASCII.GetBytes(key);

			var zero = (int)'0';

			for(int i = 0; i < Value.Length; i++)
			{
				Value[i] = (byte)((int)Value[i] - zero);
			}
		}

		/// <summary>
		/// Creates an instance from SV templated string
		/// </summary>
		/// <param name="svStirng">[SV 0 0 0 0 1 0 1 4 0 2 1 0 2]</param>
		/// <returns>SVVector</returns>
		public static SVVector FromSV(string svString)
		{
			if(string.IsNullOrEmpty(svString))
			{
				throw new ArgumentNullException(nameof(svString));
			}

			var sb = new StringBuilder();
			foreach(var digit in svString.Select(t => t).Where(ch => Char.IsDigit(ch)))
			{
				sb.Append(digit);
			}
			return new SVVector(sb.ToString());
		}

		public override string ToString()
		{
			var sb = new StringBuilder("[SV");
			foreach(var byteN in Value)
			{
				sb.Append(' ').Append((int)byteN - (int)0);
			}
			sb.Append(']');
			return sb.ToString();
		}

		// Summary:
		//     Compares two bytes array one by one till bytes are equal.
		//
		// Parameters:
		//   other:
		//   	A vector to compare with.
		public int CompareTo(SVVector other)
		{
			if(ReferenceEquals(this, other))
			{
				return 0;
			}

			if(Length != other.Length)
			{
				return Length.CompareTo(other.Length);
			}

			var result = 0;
			for(var i = 0; i < Length; i++)
			{
				result = Value[i].CompareTo(other.Value[i]);
				if(result != 0)
				{
					return result;
				}
			}

			return result;
		}

		public bool Equals(SVVector other) => this.CompareTo(other) == 0;

		public bool Includes(SVVector other)
		{
			if(Length >= other.Length)
			{
				for(var i = other.Length - 1; i > -1; i--)
				{
					if(Value[i].CompareTo(other.Value[i]) < 0)
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}
	}
}
