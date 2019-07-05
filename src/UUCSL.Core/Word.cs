using System;
using System.Runtime.InteropServices;
using System.Text;

namespace UUCSL.Core
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Word : IComparable<Word>
	{
		[FieldOffset(0)]
		public ulong Value;

		[FieldOffset(0)]
		public byte X;

		[FieldOffset(1)]
		public byte N;

		[FieldOffset(2)]
		public byte C;

		[FieldOffset(3)]
		public byte D;

		[FieldOffset(4)]
		public byte A;

		[FieldOffset(5)]
		public byte F;

		[FieldOffset(6)]
		public byte S;

		[FieldOffset(7)]
		public byte T;

		public Word(string word)
		{
			Value = X = N = C = D = A = F = S = T = 0;

			foreach(var character in word)
			{
				Set(character);
			}
		}

		public Word(byte[] bytes, byte count = 0)
		{
			Value = 0;
			X = bytes[0];
			N = bytes[1];
			C = bytes[2];
			D = bytes[3];
			A = bytes[4];
			F = bytes[5];
			S = bytes[6];
			T = bytes[7];
		}

		public static Func<string, Word> Create = word => new Word(word);

		public override string ToString()
		{
			var sb = new StringBuilder();
			for(var i = 0; i < X; i++)
			{
				sb.Append('X');
			}
			for(var i = 0; i < N; i++)
			{
				sb.Append('N');
			}
			for(var i = 0; i < C; i++)
			{
				sb.Append('C');
			}
			for(var i = 0; i < D; i++)
			{
				sb.Append('D');
			}
			for(var i = 0; i < D; i++)
			{
				sb.Append('D');
			}
			for(var i = 0; i < A; i++)
			{
				sb.Append('A');
			}
			for(var i = 0; i < F; i++)
			{
				sb.Append('F');
			}
			for(var i = 0; i < S; i++)
			{
				sb.Append('S');
			}
			for(var i = 0; i < T; i++)
			{
				sb.Append('T');
			}

			return sb.ToString();
		}

		private void Set(char character)
		{
			switch(character)
			{
				case 'X':
					X = (byte)(X + 1);
					break;
				case 'N':
					N = (byte)(N + 1);
					break;
				case 'C':
					C = (byte)(C + 1);
					break;
				case 'D':
					D = (byte)(D  + 1);
					break;
				case 'A':
					A = (byte)(A + 1);
					break;
				case 'F':
					F = (byte)(F  + 1);
					break;
				case 'S':
					S = (byte)(S + 1);
					break;
				case 'T':
					T = (byte)(T + 1);
					break;
				default:
				break;
			}
		}

		public int CompareTo(Word other) => this.Value.CompareTo(other.Value);
	}
}
