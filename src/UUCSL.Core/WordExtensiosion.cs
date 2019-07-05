using System.Linq;
using System.Numerics;

namespace UUCSL.Core
{
	public static class WordExtensiosion
	{
		public static BigInteger ToBigInteger(this Word word)
		{
			var all = new int[] { word.X, word.N, word.C, word.D, word.A, word.F, word.S, word.T };
			var baseNumber = all.Max();
			if(baseNumber < 10)
			{
				baseNumber = 10;
			}

			BigInteger number = word.T +
							    word.S * baseNumber +
								word.F * (baseNumber ^ 2) +
								word.A * (baseNumber ^ 3) +
								word.D * (baseNumber ^ 4) +
								word.C * (baseNumber ^ 5) +
								word.N * (baseNumber ^ 6) +
								word.X * (baseNumber ^ 7);

			return number;
		}
	}
}
