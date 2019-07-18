using BenchmarkDotNet.Attributes;
using UUCSL.Core;

namespace UUCSL.Benchmarks
{
	[CoreJob]
	public class BruteForceTest
	{
		[Benchmark]
		public void SVBlockTree()
		{
			var tree = new SVBlockTree();
			foreach(var block in TestData.GenerateBlocks())
			{
				tree = tree.Merge(new SVBlockTree(block));
			}
		}
	}
}
