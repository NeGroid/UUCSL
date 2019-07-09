using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace UUCSL.Core.Tests
{
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			var fileContent = @"
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]
DND
DDC
DAN
ANN
ADD
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]
CDD
DNC
DDN
DAD
AND";
			var blocks = new List<SVBlock>();
			var patterns = 5;

			using(var reader = new StringReader(fileContent))
			{
				while(true)
				{
					var line = reader.ReadLine();
					if(string.IsNullOrEmpty(line))
					{
						break;
					}
					if(line.StartsWith("[SV"))
					{
						var svVector = SVVector.FromSV(line);
						var words = Enumerable.Range(0, patterns).Select(i => reader.ReadLine());
						var block = SVBlock.FromSV(svVector, patterns, words);
						blocks.Add(block);
					}
				}
			}

			var blockList = new SVBlockList(blocks);

			var svKeys = blockList.Keys.ToArray();

			Assert.Equal(new []
				{
					"[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]",
					"[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]"
				},
				svKeys);
		}
	}
}
