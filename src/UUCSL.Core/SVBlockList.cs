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
				Blocks[block.Vector] = block;
				var index = Blocks.IndexOfValue(block);
				if(index < Blocks.Count - 1)
				{
					// check all right blocks
					var rightRange = Enumerable.Range(index + 1, Blocks.Count - index - 1).Select(i => Blocks.Values[i]);
					var includedByRight = false;
					foreach(var right in rightRange)
					{
						includedByRight = right.Includes(block);
						if(includedByRight)
						{
							Blocks.Remove(block.Vector);
							break;
						}
					}

					if(!includedByRight)
					{
						var leftRange = Enumerable.Range(0, index).Reverse().Select(i => Blocks.Values[i]);
						var includesLeft = false;
						foreach(var left in leftRange)
						{
							includesLeft = block.Includes(left);
							if(includesLeft)
							{
								Blocks.Remove(left.Vector);
								break;
							}
						}
					}
				}
				else 
				{
					if(index > 0)
					{
						// check all left blocks
						var leftRange = Enumerable.Range(0, index).Reverse().Select(i => Blocks.Values[i]);
						var includesLeft = false;
						foreach(var left in leftRange)
						{
							includesLeft = block.Includes(left);
							if(includesLeft)
							{
								Blocks.Remove(left.Vector);
								break;
							}
						}
					}
				}
			}
		}
	}
}
