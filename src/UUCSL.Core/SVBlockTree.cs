using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class SVBlockTree
	{
		private SortedDictionary<SVVector, SVBlockTree> _subblocks;

		public SVBlock Block { get; private set; }

		public SVBlockTree(SVBlock block, SortedDictionary<SVVector, SVBlockTree> children = null)
		{
			Block = block;
			if(children != null)
			{
				_subblocks = children;

				if(_subblocks.Any())
				{
					MaxBlock = _subblocks.Values.Last().Block;
				}
			}
			else
			{
				_subblocks = new SortedDictionary<SVVector, SVBlockTree>();
			}
		}

		public SVBlockTree Add(SVBlock block)
		{
			if(block.Includes(Block))
			{
				return new SVBlockTree(block, new SortedDictionary<SVVector, SVBlockTree> { { block.Vector, this } });
			}

			if(Block.Includes(block))
			{
				SVBlockTree childTree = null;
				foreach(var child in Children)
				{
					if(child.Block.Includes(block))
					{
						childTree = child;
						break;
					}
				}

				if(childTree == null)
				{
					_subblocks.Add(block.Vector, new SVBlockTree(block));
					MaxBlock = Children.Select(t => t.Block).Last();
				}
				else
				{
					childTree.Add(block);
				}
			}
			else
			{
				var subblocks = new SortedDictionary<SVVector, SVBlockTree> 
				{ 
					{ block.Vector, new SVBlockTree(block) },
					{ Block.Vector, this },
				};
				return new SVBlockTree(null, subblocks);
			}

			return this;
		}

		public ICollection<SVBlockTree> Children => _subblocks.Values;

		public SVBlock MaxBlock { get; private set; }
	}
}