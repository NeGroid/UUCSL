using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace UUCSL.Console
{
	internal class NCSTaskService : IHostedService
	{
		private readonly ILogger<NCSTaskService> _logger;

		public NCSTaskService(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<NCSTaskService>();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("2000", "NCSTaskService started");

			return Task.FromResult(true);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("2000", "NCSTaskService ended");
			return Task.FromResult(true);
		}
	}
}