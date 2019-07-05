using System;
using System.Linq;
using System.Collections.Generic;
using UUCSL.Core;

namespace UUCSL.Benchmarks
{
	public static class TestData
	{
		public static IEnumerable<Block> GenerateBlocks(int count = 100_000)
		{
			var rand = new Random(42);
			foreach(var i in Enumerable.Range(0, count))
			{
				var words = new List<Word>();
				foreach(var j in Enumerable.Range(0, 5))
				{
					var bytes = new byte[8];
					rand.NextBytes(bytes);
					words.Add(new Word(bytes));
				}
				var block = new Block(words.ToArray());
				yield return block;
			}
		}
	}
}
