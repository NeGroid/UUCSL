using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace UUCSL.Core
{
	public struct SVVector : IComparable<SVVector>, IEquatable<SVVector>
	{
		public byte[] Value;

		public byte Byte0 => Value[0];

		public byte Byte1 => Value[1];

		public byte Byte2 => Value[2];

		public byte Byte3 => Value[3];

		public byte Byte4 => Value[4];

		public byte Byte5 => Value[5];

		public byte Byte6 => Value[6];

		public byte Byte7 => Value[7];

		public byte Byte8 => Value[8];

		public byte Byte9 => Value[9];

		public byte Byte10 => Value[10];

		public byte Byte11 => Value[11];

		public byte Byte12 => Value[12];

		public BitArray Mask;

		private SVVector(string key)
		{
			Value = Encoding.ASCII.GetBytes(key);
			Mask = new BitArray(new bool[Value.Length]);

			var zero = (int)'0';

			for(int i = 0; i < Value.Length; i++)
			{
				Value[i] = (byte)((int)Value[i] - zero);
				Mask.Set(i, (int)Value[i] > 0);
			}
		}

		/// <summary>
		/// Creates an instance from SV templated string
		/// </summary>
		/// <param name="svStirng">[SV 0 0 0 0 1 0 1 4 0 2 1 0 2]</param>
		/// <returns>SVVector</returns>
		public static SVVector FromSV(string svString)
		{
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
			foreach(var byteN in BytesN)
			{
				sb.Append(' ').Append((int)byteN(this) - (int)0);
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
			int result = 0;
			foreach(var byteN in BytesN)
			{
				result = byteN(this).CompareTo(byteN(other));
				if(result > 0)
				{
					return 1;
				}
				else if(result < 1)
				{
					return -1;
				}
			}

			return result;
		}

		public bool Equals(SVVector other) => this.CompareTo(other) == 0;

		public bool Includes(SVVector other)
		{
			foreach(var byteN in BytesN)
			{
				if(byteN(this).CompareTo(byteN(other)) > 0)
				{
					return false;
				}
			}

			return true;
		}

		private static Func<SVVector, byte>[] BytesN = new Func<SVVector, byte>[13]
		{
			v => v.Byte0,
			v => v.Byte1,
			v => v.Byte2,
			v => v.Byte3,
			v => v.Byte4,
			v => v.Byte5,
			v => v.Byte6,
			v => v.Byte7,
			v => v.Byte8,
			v => v.Byte9,
			v => v.Byte10,
			v => v.Byte11,
			v => v.Byte12
		};
	}
}
