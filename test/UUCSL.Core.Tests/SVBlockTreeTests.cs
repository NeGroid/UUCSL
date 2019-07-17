using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UUCSL.Core.Tests
{
	public class SVBlockTreeTests
	{
		private const string _file = @"[NCS = NCDA8]
[Deuterated = False]
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]
CDD
DNC
DDN
DAD
AND
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]
DND
DDC
DAN
ANN
ADD
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 1 0 0 1 1 1 0 1]
NDD
DNN
DCD
DDA
ADN
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 1 0 0 0 1 1 1 0 1]
CNN
DNA
DCD
DDN
ADD
[ELB samples = 3 patterns = 5]
[SV 0 0 0 1 0 0 0 0 1 1 1 0 1]
NNN
CDD
DDN
DAD
AND";

		[Fact]
		public void Create_SVBlockTree_from_file_as_root()
		{
			var tree = CreateTree(_file);

			Assert.Null(tree.Block);
		}

		[Fact]
		public void Create_SVBlockTree_from_file()
		{
			var tree = CreateTree(_file);
			var children = tree.Children.Select(t => t.Block.ToString()).ToArray();

			Assert.Equal(new [] { "" }, children);
		}

		private static SVBlockTree CreateTree(string file)
		{
			if (file is null)
			{
				throw new ArgumentNullException(nameof(file));
			}

			var tree = new SVBlockTree();

			foreach (var block in ReadLines())
			{
				tree = tree.Add(block);
			}

			return tree;
		}

		private static IEnumerable<SVBlock> ReadLines()
		{
			var allLines = _file.Split(Environment.NewLine);
			var lines = allLines.Where(t => !string.IsNullOrEmpty(t) && !t.StartsWith("#")).ToArray();
			var index = 2;
			while (index < lines.Length)
			{
				var elbLine = new ELBLine(lines[index]); // [ELB samples = 3 patterns = 5]

				var vector = SVVector.FromSV(lines[index + 1]); // [SV 0 0 0 0 0 0 0 1 1 1 1 0 1]

				var words = Enumerable.Range(index + 2, elbLine.Patterns).Select(i => lines[i]);

				var block = SVBlock.FromSV(vector, words);
				yield return block;
				index += elbLine.Patterns + 2;
			}
		}
	}
}
