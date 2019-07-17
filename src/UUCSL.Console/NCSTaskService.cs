using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

			if (!Path.IsPathRooted(file.Value.File))
			{
				_filePath = Path.GetFullPath(file.Value.File);
			}
			else
			{
				_filePath = file.Value.File;
			}

			if (!File.Exists(_filePath))
			{
				throw new FileNotFoundException($"Could not find input file: '{_filePath}'");
			}
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			var startedAt = DateTime.Now;
			_logger.LogInformation("NCSTaskService started");
			var allLines = await File.ReadAllLinesAsync(_filePath, cancellationToken);

			var lines = allLines.Where(t => !string.IsNullOrEmpty(t) && !t.StartsWith("#")).ToArray();

			if (lines.Length < 2)
			{
				throw new InvalidOperationException($"Too few lines in the file '{_filePath}");
			}

			var blocks = new List<SVBlock>();
			var index = 2;
			while (index < lines.Length)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				_logger.LogDebug($"ELB: {lines[index]}");
				var elbLine = new ELBLine(lines[index]); // [ELB samples = 3 patterns = 5]

				_logger.LogDebug($"SV: {lines[index + 1]}");
				var vector = SVVector.FromSV(lines[index + 1]); // [SV 0 0 0 0 0 0 0 1 1 1 1 0 1]

				var words = Enumerable.Range(index + 2, elbLine.Patterns).Select(i => lines[i]);
				_logger.LogDebug($"Words ({elbLine.Patterns}): {string.Join(' ', words)}");

				var block = SVBlock.FromSV(vector, words);
				blocks.Add(block);
				index += elbLine.Patterns + 2;
			}

			var blockList = new SVBlockList(blocks);
			var time = (DateTime.Now - startedAt).TotalSeconds;
			_logger.LogInformation(string.Join(Environment.NewLine, blockList.Blocks.Select(t => t.Key)));
			_logger.LogInformation($"{blockList.Blocks.Count} blocks from total {blocks.Count} found for total time {time}");
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("NCSTaskService ended");
			return Task.FromResult(true);
		}
	}
}
