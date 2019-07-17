using System;
using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class SVBlockTree
	{
		private readonly SortedDictionary<SVVector, SVBlockTree> _subblocks = new SortedDictionary<SVVector, SVBlockTree>();

		public SVBlock Block { get; }

		public bool IsRoot => Block is null;

		public ICollection<SVBlockTree> Children => _subblocks.Values;

		private SVBlockTree(IEnumerable<SVBlockTree> children, SVBlock block = null)
		{
			if (children is null)
			{
				throw new ArgumentNullException(nameof(children));
			}

			Block = block;
			AddChildren(children);
		}

		public SVBlockTree(SVBlock block)
		{
			if (block is null)
			{
				throw new ArgumentNullException(nameof(block));
			}

			Block = block;
		}

		public SVBlockTree()
		{
		}

		public SVBlockTree Add(SVBlock block)
		{
			if (block is null)
			{
				throw new ArgumentNullException(nameof(block));
			}

			return AddInner(new SVBlockTree(block));
		}

		private SVBlockTree AddInner(SVBlockTree tree)
		{
			if (!IsRoot && !tree.IsRoot)
			{
				var block = tree.Block;
				if (block.Includes(Block))
				{
					return tree.AddInner(this);
				}

				if (Block.Includes(block))
				{
					AddChild(tree);
					return this;
				}
			}

			var notRootTree = !IsRoot ? (this, tree) : !tree.IsRoot ? (tree, this) : (null, null);
			if(notRootTree.Item1 != null)
			{
				//notRootTree.Item1.ad
			}

			foreach (var child in Children)
			{
				tree = tree.AddInner(child);
			}

			var subblocks = new SVBlockTree[] { tree, this };
			return new SVBlockTree(subblocks);
		}

		private void AddChild(SVBlockTree tree)
		{
			if (_subblocks.Count == 0)
			{
				_subblocks.Add(tree.Block.Vector, tree);
				return;
			}

			var maxBlock = Children.Last().Block;
			int comparasion = maxBlock.CompareTo(tree.Block);
			if (comparasion == 0)
			{
				return;
			}
			if (comparasion < 0)
			{
				_subblocks.Add(tree.Block.Vector, tree);
				return;
			}

			var childIncludes = Children.Reverse().FirstOrDefault(t => t.Block.Includes(tree.Block));
			if (childIncludes != null)
			{
				childIncludes.AddChild(tree);
			}
			else
			{
				_subblocks.Add(tree.Block.Vector, tree);
			}
		}

		private void AddChildren(IEnumerable<SVBlockTree> children)
		{
			foreach (var child in children)
			{
				_subblocks.Add(child.Block.Vector, child);
			}
		}
	}
}
