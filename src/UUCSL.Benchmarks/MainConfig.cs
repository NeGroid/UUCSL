using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Toolchains.CsProj;
#pragma warning disable 0618

namespace UUCSL.Benchmarks
{
	public class MainConfig : ManualConfig
	{
		public MainConfig()
		{
			Add(Job.Default.With(Platform.X64).With(CsProjCoreToolchain.NetCoreApp30));

			Add(MemoryDiagnoser.Default);
			Add(new MinimalColumnProvider());
			Add(MemoryDiagnoser.Default);
			Orderer = new DefaultOrderer(SummaryOrderPolicy.SlowestToFastest);
			Add(MarkdownExporter.GitHub);
			Add(new ConsoleLogger());
		}
	}
}
