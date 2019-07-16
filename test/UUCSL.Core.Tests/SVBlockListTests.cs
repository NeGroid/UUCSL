using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace UUCSL.Core.Tests
{
	public class SVBlockListTests
	{
		[Fact]
		public void Create_SVBlock_from_SV_string()
		{
			const string fileContent = @"[ELB samples = 3 patterns = 5]
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
			const int patterns = 5;

			using (var reader = new StringReader(fileContent))
			{
				while (true)
				{
					var line = reader.ReadLine();
					if (string.IsNullOrEmpty(line))
					{
						break;
					}
					if (line.StartsWith("[SV"))
					{
						var svVector = SVVector.FromSV(line);
						var words = Enumerable.Range(0, patterns).Select(_ => reader.ReadLine());
						var block = SVBlock.FromSV(svVector, words);
						blocks.Add(block);
					}
				}
			}

			var blockList = new SVBlockList(blocks);
			var svKeys = blockList.Keys.ToArray();

			Assert.Equal(new[]
				{
					"[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]",
					"[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]"
				},
				svKeys);
		}

		[Fact]
		public void Create_SVBlock_from_SV_string_with_equal_block()
		{
			const string fileContent = @"[ELB samples = 3 patterns = 5]
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
AND
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]
CDD
DNC
DDN
DAD
AND";
			var blocks = new List<SVBlock>();
			const int patterns = 5;

			using (var reader = new StringReader(fileContent))
			{
				while (true)
				{
					var line = reader.ReadLine();
					if (string.IsNullOrEmpty(line))
					{
						break;
					}
					if (line.StartsWith("[SV"))
					{
						var svVector = SVVector.FromSV(line);
						var words = Enumerable.Range(0, patterns).Select(_ => reader.ReadLine());
						var block = SVBlock.FromSV(svVector, words);
						blocks.Add(block);
					}
				}
			}

			var blockList = new SVBlockList(blocks);
			var svKeys = blockList.Keys.ToArray();

			Assert.Equal(new[]
				{
					"[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]",
					"[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]"
				},
				svKeys);
		}

		[Fact]
		public void Create_SVBlock_from_SV_string_with_sub_block()
		{
			const string fileContent = @"[ELB samples = 3 patterns = 5]
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
AND
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 0 0 1 0 1 1 0 1]
CDD
DNC
DDN
DAD
AND
[ELB samples = 3 patterns = 5]
[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]
CDD
DNC
DDN
DAD
AND";
			var blocks = new List<SVBlock>();
			const int patterns = 5;

			using (var reader = new StringReader(fileContent))
			{
				while (true)
				{
					var line = reader.ReadLine();
					if (string.IsNullOrEmpty(line))
					{
						break;
					}
					if (line.StartsWith("[SV"))
					{
						var svVector = SVVector.FromSV(line);
						var words = Enumerable.Range(0, patterns).Select(_ => reader.ReadLine());
						var block = SVBlock.FromSV(svVector, words);
						blocks.Add(block);
					}
				}
			}

			var blockList = new SVBlockList(blocks);
			var svKeys = blockList.Keys.ToArray();

			Assert.Equal(new[]
				{
					"[SV 0 0 0 0 0 0 0 1 1 1 1 0 1]",
					"[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]"
				},
				svKeys);
		}

		[Fact]
		public void Check_if_long_equal_vectors_are_equal()
		{
			var v1 = SVVector.FromSV("[SV 0 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 ]");
			var v2 = SVVector.FromSV("[SV 0 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 ]");

			Assert.True(v1.Equals(v2));
		}

		[Fact]
		public void Compares_long_equal_vectors()
		{
			var v1 = SVVector.FromSV("[SV 0 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 ]");
			var v2 = SVVector.FromSV("[SV 0 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 ]");

			Assert.Equal(0, v1.CompareTo(v2));
		}

		[Fact]
		public void Compares_long_vectors_of_different_length()
		{
			var v1 = SVVector.FromSV("[SV 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 ]");
			var v2 = SVVector.FromSV("[SV 0 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 ]");

			Assert.Equal(-1, v1.CompareTo(v2));
		}

		[Fact]
		public void Compares_long_vectors_of_different_length2()
		{
			var v1 = SVVector.FromSV("[SV 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 0]");
			var v2 = SVVector.FromSV("[SV 0 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0]");

			Assert.Equal(1, v1.CompareTo(v2));
		}

		[Fact]
		public void Check_if_vector_includes_other_vector()
		{
			var v1 = SVVector.FromSV("[SV 1 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 0]");
			var v2 = SVVector.FromSV("[SV 0 0 0 1 0 0 2 1 0 0 1 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0]");

			Assert.True(v1.Includes(v2));
		}

		[Fact]
		public void Check_if_vector_does_not_include_other_vector()
		{
			var v1 = SVVector.FromSV("[SV 1 0 0 1 0 0 3 1 0 0 1 0 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 0]");
			var v2 = SVVector.FromSV("[SV 0 1 0 1 0 0 2 1 0 0 1 0 0 0 0 0 1 0 0 0 0 0 0 0 0 0 0 0]");

			Assert.False(v1.Includes(v2));
		}
	}
}
