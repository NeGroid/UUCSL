using BenchmarkDotNet.Running;

namespace UUCSL.Benchmarks
{
	static class Program
	{
		static void Main(string[] args)
		{
			BenchmarkRunner.Run<BruteForceTest>(new MainConfig());
		}
	}
}
