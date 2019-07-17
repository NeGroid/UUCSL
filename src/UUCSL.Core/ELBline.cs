using System;
using System.Linq;

namespace UUCSL.Core
{
	public class ELBLine
	{
		public int Samples { get; }
		public int Patterns { get; }

		public ELBLine(string line)
		{
			if (string.IsNullOrEmpty(line))
			{
				throw new ArgumentNullException(nameof(line));
			}

			//[ELB samples = 3 patterns = 12]
			var nums = line
				.Replace("[ELB samples = ", string.Empty)
				.Replace("patterns = ", string.Empty)
				.Replace("]", string.Empty)
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(int.Parse)
				.ToArray();

			Samples = nums[0];
			Patterns = nums[1];
		}
	}
}
