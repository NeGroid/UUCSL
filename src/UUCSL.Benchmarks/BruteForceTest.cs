using BenchmarkDotNet.Attributes;
using UUCSL.Core;

namespace UUCSL.Benchmarks
{
	[CoreJob]
	public class BruteForceTest
	{
		[Benchmark]
		public void SVBlockList()
		{
			_ = new SVBlockList(TestData.GenerateBlocks());
		}
	}
}
