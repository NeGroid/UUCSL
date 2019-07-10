using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UUCSL.Core;

namespace UUCSL.Console
{
	internal class NCSTaskService : IHostedService
	{
		private readonly ILogger<NCSTaskService> _logger;
		private readonly string _filePath;

		public NCSTaskService(ILoggerFactory loggerFactory, IOptions<TaskOptions> file)
		{
			_logger = loggerFactory.CreateLogger<NCSTaskService>();
			
			if(!Path.IsPathRooted(file.Value.File))
			{
				_filePath = Path.GetFullPath(file.Value.File);
			}

			if(!File.Exists(_filePath))
			{
				throw new FileNotFoundException($"Could not find input file: '{_filePath}'");
			}
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("NCSTaskService started");

			using(var stream = File.OpenRead(_filePath))
			using (StreamReader reader = new StreamReader(stream))
			{
				await reader.ReadLineAsync(); // [NCS = NCDA8]
				await reader.ReadLineAsync(); // [Deuterated = False]

				while(true)
				{
					if(cancellationToken.IsCancellationRequested)
					{
						return;
					}

					string elbLine = await reader.ReadLineAsync(); // [ELB samples = 3 patterns = 5]
					if(string.IsNullOrEmpty(elbLine))
					{
						break;
					}
					SVVector vector = SVVector.FromSV( await reader.ReadLineAsync()); // [SV 0 0 0 0 0 0 0 1 1 1 1 0 1]
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("NCSTaskService ended");
			return Task.FromResult(true);
		}

		private class ELBLine
		{
			public int Samples { get; private set; }
			public int Patterns { get; private set; }

			public ELBLine(string line)
			{
				//[ELB samples = 3 patterns = 12]
				var nums = line
					.Replace("[ELB samples = ", string.Empty)
					.Replace("patterns = ", string.Empty)
					.Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(int.Parse)
					.ToArray();

				Samples = nums[0];
				Patterns = nums[1];
			}
		}
	}
}
