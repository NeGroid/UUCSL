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

			Assert.Equal(new[] {
				"[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]",
				"[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]",
				"[SV 0 0 0 0 0 1 0 0 1 1 1 0 1]",
				"[SV 0 0 0 0 1 0 0 0 1 1 1 0 1]",
				"[SV 0 0 0 1 0 0 0 0 1 1 1 0 1]" },
				children);
		}

		[Fact]
		public void Create_SVBlockTree_from_file_correctly()
		{
			const string file = @"[NCS = NCDA8]
[Deuterated = False]
[ELB samples = 3 patterns = 5]
[SV 0 0 0 1 0 0 0 0 1 1 1 0 1]
NNN
CDD
DDN
DAD
AND
[ELB samples = 3 patterns = 5]
[SV 0 0 0 2 0 0 0 0 1 1 1 0 1]
NNN
CDD
DDN
DAD
AND";
			var tree = CreateTree(file);
			var children = tree.Children.Select(t => t.Block.ToString()).ToArray();

			Assert.Equal("[SV 0 0 0 2 0 0 0 0 1 1 1 0 1]", tree.Block?.ToString());
			Assert.Equal(new[] {
				"[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]",
				"[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]",
				"[SV 0 0 0 0 0 1 0 0 1 1 1 0 1]",
				"[SV 0 0 0 0 1 0 0 0 1 1 1 0 1]",
				"[SV 0 0 0 1 0 0 0 0 1 1 1 0 1]" },
				children);
		}

		[Fact]
		public void Real_data_test()
		{
			const string file = @"[NCS = NCDA8]
[Deuterated = False]
# iterator = 8
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 1 1 0 0 1 0 0 2 ]
NND
DAN
DAD
ANN
ADD

# iterator = 10
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 1 0 0 0 2 0 0 2 ]
NND
DAN
DAD
ADN
ADD

# iterator = 27
[ELB samples = 3 patterns = 8]
[SV 0 0 2 0 2 0 1 1 0 1 1 0 0 ]
NNA
NCN
NDC
NAD
CNN
CDD
DXD
DDX

# iterator = 28
[ELB samples = 3 patterns = 8]
[SV 0 0 1 0 2 0 1 1 1 1 1 0 0 ]
NNA
NCN
NDC
NAD
CNN
CDD
DXD
DDN

# iterator = 31
[ELB samples = 3 patterns = 10]
[SV 0 0 1 0 2 0 1 1 0 3 1 0 1 ]
NNA
NCN
NDC
NAD
CNN
CDD
DXD
DDA
DAN
ADN

# iterator = 32
[ELB samples = 3 patterns = 9]
[SV 0 0 1 0 2 0 1 1 0 2 1 0 1 ]
NNA
NCN
NDC
NAD
CNN
CDD
DXD
DDA
ADN

# iterator = 34
[ELB samples = 3 patterns = 9]
[SV 0 0 1 0 2 0 1 1 0 3 1 0 0 ]
NNA
NCN
NDC
NAD
CNN
CDD
DXD
DAN
ADN";

			var tree = CreateTree(file);
			var children = tree.Children.Select(t => t.Block.ToString()).ToArray();

			Assert.Equal("[SV 0 0 0 2 0 0 0 0 1 1 1 0 1]", tree.Block?.ToString());
			Assert.Equal(new[] {
				"[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]",
				"[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]",
				"[SV 0 0 0 0 0 1 0 0 1 1 1 0 1]",
				"[SV 0 0 0 0 1 0 0 0 1 1 1 0 1]",
				"[SV 0 0 0 1 0 0 0 0 1 1 1 0 1]" },
				children);
		}

		private static SVBlockTree CreateTree(string file)
		{
			if (file is null)
			{
				throw new ArgumentNullException(nameof(file));
			}

			var tree = new SVBlockTree();

			foreach (var block in ReadLines(file))
			{
				tree = tree.Merge(new SVBlockTree(block));
			}

			return tree;
		}

		private static IEnumerable<SVBlock> ReadLines(string file)
		{
			var allLines = file.Split(Environment.NewLine);
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
