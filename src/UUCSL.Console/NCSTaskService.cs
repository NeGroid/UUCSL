using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace UUCSL.Console
{
	internal class NCSTaskService : IHostedService
	{
		private readonly ILogger<NCSTaskService> _logger;
		private readonly string _file;

		public NCSTaskService(ILoggerFactory loggerFactory, IOptions<TaskOptions> file)
		{
			_logger = loggerFactory.CreateLogger<NCSTaskService>();
			_file = file.Value.File;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("NCSTaskService started");
			return Task.FromResult(true);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("NCSTaskService ended");
			return Task.FromResult(true);
		}
	}
}
