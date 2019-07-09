using System;
using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class SVBlockList
	{
		public SortedList<SVVector, SVBlock> Blocks { get; private set; } = new SortedList<SVVector, SVBlock>();
		public SortedDictionary<SVVector, SVBlock> Duplicates { get; private set; } = new SortedDictionary<SVVector, SVBlock>();

		public IEnumerable<string> Keys => Blocks.Keys.Select(t => t.ToString());

		public SVBlockList(IEnumerable<SVBlock> blocks)
		{
			foreach(var block in blocks)
			{
				if(Blocks.ContainsValue(block))
				{
					continue;
				}

				Blocks[block.Vector] = block;

				var index = Blocks.IndexOfValue(block);
				if(!CheckRightBlocks(block, index))
				{
					CheckLeftBlocks(block, index);
				}
			}
		}

		private void CheckLeftBlocks(SVBlock block, int index)
		{
			var count = index - 1;
			if(count <= 0)
			{
				return;
			}

			var leftRange = Blocks.Values
				.Take(count)
				.Where(b => block.Includes(b));

			foreach(var subblock in leftRange)
			{
				Blocks.Remove(subblock.Vector);
			}
		}

		private bool CheckRightBlocks(SVBlock block, int index)
		{
			var count = Blocks.Count - index - 1;
			if(count <= 0)
			{
				return false;
			}

			var includesBlock = Blocks.Values
				.Reverse()
				.Take(count)
				.Any(b => b.Includes(block));

			if(includesBlock)
			{
				return Blocks.Remove(block.Vector);
			}

			return false;
		}
	}
}
