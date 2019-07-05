using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using UUCSL.Core;

namespace UUCSL.Benchmarks
{
	[CoreJob]
	public class BruteForceTest
	{
		[Benchmark]
		public void SortedDictionary()
		{
			var sortedBlocks = new SortedDictionary<Block, Block>(new BlockComparer());
			foreach(var block in TestData.GenerateBlocks())
			{
				sortedBlocks[block] = block;
			}
		}

		[Benchmark]
		public void SortedList()
		{
			var rand = new Random(42);
			var sortedBlocks = new SortedList<Block, Block>(new BlockComparer());
			foreach(var block in TestData.GenerateBlocks())
			{
				sortedBlocks[block] = block;
			}
		}

		private class BlockComparer : IComparer<Block>
		{
			public int Compare(Block x, Block y)
			{
				if(x.Words.Count > y.Words.Count)
				{
					return 1;
				}

				if(x.Equals(y))
				{
					return 0;
				}

				return -1;
			}
		}

		private class WordComparer : IComparer<Word>
		{
			public int Compare(Word x, Word y) => x.Value.CompareTo(y.Value);
		}
	}
}
