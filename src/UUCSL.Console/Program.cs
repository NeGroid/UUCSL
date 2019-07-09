using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UUCSL.Console
{
	internal static partial class Program
	{
		public static Task Main(string[] args)
		{
			var host = new HostBuilder()
				.ConfigureHostConfiguration(configHost =>
				{
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("hostsettings.json", optional: true);
					configHost.AddEnvironmentVariables(prefix: "PREFIX_");
					configHost.AddCommandLine(args);
				})
				.ConfigureAppConfiguration((hostContext, configApp) =>
				{
					configApp.AddJsonFile("appsettings.json", optional: true);
					configApp.AddJsonFile(
						$"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
						optional: true);
					configApp.AddEnvironmentVariables(prefix: "PREFIX_");
					configApp.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) => 
				{
					services.AddHostedService<NCSTaskService>();
				})
				.ConfigureLogging((hostContext, configLogging) => {})
				.UseConsoleLifetime()
				.Build();

			return host.RunAsync();
		}
	}
}
