using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace UUCSL.Console
{
	internal static partial class Program
	{
		public static readonly Dictionary<string, string> _switchMappings = new Dictionary<string, string>
		{
			{ "-i", "File" }
		};

		public static Task Main(string[] args)
		{
			var host = new HostBuilder()
				.ConfigureHostConfiguration(configHost => {
					configHost.SetBasePath(Directory.GetCurrentDirectory());
					configHost.AddJsonFile("hostsettings.json", optional: true);
					configHost.AddEnvironmentVariables(prefix: "PREFIX_");
					configHost.AddCommandLine(args, _switchMappings);
				})
				.ConfigureAppConfiguration((hostContext, configApp) => {
					configApp.AddJsonFile("appsettings.json", optional: true);
					configApp.AddJsonFile(
						$"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
						optional: true);
					configApp.AddEnvironmentVariables(prefix: "PREFIX_");
					configApp.AddCommandLine(args, _switchMappings);
				})
				.ConfigureServices((hostContext, services) => {
					services.Configure<TaskOptions>(hostContext.Configuration);
					services.AddHostedService<NCSTaskService>();
				})
				.UseSerilog((hostContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration))
				.UseConsoleLifetime()
				.Build();

			return host.RunAsync();
		}
	}
}
