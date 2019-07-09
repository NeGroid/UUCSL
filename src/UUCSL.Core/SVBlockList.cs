using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class SVBlockList
	{
		private readonly IEnumerable<SVBlock> _blocks;

		public SortedList<SVVector, SVBlock> Blocks { get; private set; } = new SortedList<SVVector, SVBlock>();
		public SortedDictionary<SVVector, SVBlock> Duplicates { get; private set; } = new SortedDictionary<SVVector, SVBlock>();


		public IEnumerable<string> Keys => Blocks.Keys.Select(t => t.ToString());

		public SVBlockList(IEnumerable<SVBlock> blocks)
		{
			// Shoudld we try to sort blocks befor assigng to make a sorted list faster?

			_blocks = blocks;
		}

		public SVBlockList ToBalanced()
		{
			// Add blocks to a sortable list.
			// A comparasion is based on the SV vector value.
			foreach(var block in _blocks)
			{
				Blocks[block.Vector] = block;
			}
			/*
				The sorted ist then starts from the smallest vector value.

				0 0 0 0 0 0 0 0 0 0 0 0 1
				0 0 0 0 0 0 0 0 0 0 0 1 1
				.........................
				0 0 0 2 0 0 1 0 0 0 0 0 0
				0 0 0 1 0 0 0 0 0 0 0 0 0
				.........................
				1 0 1 0 0 0 0 0 0 0 0 0 1
				2 0 1 0 0 0 0 0 0 1 0 0 1
			*/

			RemoveDuplicates();

			return this;
		}

		private bool MoveToDuplicates(SVBlock block)
		{
			if(Blocks.Remove(block.Vector))
			{
				Duplicates[block.Vector] = block;
				return true;
			}
			return false;
		}

		private void RemoveDuplicates()
		{
			var count = Blocks.Count;
			var left = Blocks.First().Value;
			var right = Blocks.Last().Value;

			RemoveDuplicates(left, right, count);
		}

		private bool RemoveDuplicates(SVBlock left, SVBlock right, int count)
		{
			if(count < 2)
			{
				return false;
			}
			if(count == 2)
			{
				if(right.Includes(left))
				{
					return MoveToDuplicates(left);
				}

				return false;
			}
			if(count < 4) // 3
			{
				//Split blocks (1, (2, 3))
				var removed = RemoveDuplicates(Blocks.Values[1], Blocks.Values[2], 2);
				return removed || RemoveDuplicates(Blocks.Values[0], Blocks.Values[1], 2);
			}
			/*
				Splitting blocks into the left part (0, (count / 2) - 1):

				c0 = 5
				f = 0,
				t = 4,
				c = c0 =>
			 	c = c / 2 = 5 / 2 = 2
			 	t = c - 1 = 1
			*/
			var leftCount = count / 2;
			var leftTo = leftCount - 1;
			var newRight = Blocks.Values[leftTo];
			var removedLeft = RemoveDuplicates(left, newRight, leftCount);

			/*
				Splitting blocks into the right part (count / 2, count - 1, count - (count / 2) + 1)):

				f = c
			 	c = c0 - (c / 2) + 1 = 5 - 2 + 1 = 4
			 */
			var newFrom = Blocks.Values[leftCount];
			var removedRight = RemoveDuplicates(newFrom, right, count - leftCount + 1);

			return removedLeft || removedRight;
		}
	}
}
