using System;
using System.Linq;
using System.Collections.Generic;
using UUCSL.Core;
using System.Text;

namespace UUCSL.Benchmarks
{
	public static class TestData
	{
		public static IEnumerable<SVBlock> GenerateBlocks(int count = 35_500)
		{
			var rand = new Random(42);
			foreach (var i in Enumerable.Range(0, count))
			{
				var bytes = new byte[13];
				rand.NextBytes(bytes);
				var sb = new StringBuilder("SV ");
				sb.Append(string.Join(' ', bytes));
				sb.Append(']');
				var v = SVVector.FromSV(sb.ToString());
				yield return SVBlock.FromSV(v, new string[] { "ABC" });
			}
		}
	}
}
