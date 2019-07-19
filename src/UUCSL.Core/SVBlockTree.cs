using System;
using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class SVBlockTree
	{
		private readonly SortedDictionary<SVVector, SVBlockTree> _subblocks = new SortedDictionary<SVVector, SVBlockTree>();

		public SVBlock Block { get;  private set; } = null;
		public SVBlockTree Parent { get; private set; } = null;

		public bool IsRoot => Block is null;
		public ICollection<SVBlockTree> Children => _subblocks.Values;
		public int Count => _subblocks.Count;
		public bool HasChildren => Count > 0;

		private SVBlockTree(IEnumerable<SVBlockTree> children, SVBlockTree parent = null)
		{
			if (children is null)
			{
				throw new ArgumentNullException(nameof(children));
			}

			Parent = parent;
			AddChildren(children);
		}

		public SVBlockTree()
		{
		}

		public SVBlockTree(SVBlock block)
		{
			if (block is null)
			{
				throw new ArgumentNullException(nameof(block));
			}

			Block = block;
		}

		public override string ToString()
		{
			string block = IsRoot ? "NULL" : Block.ToString();
			return $"{block}({Count})";
		}

		public SVBlockTree Merge(SVBlockTree tree)
		{
			if (IsRoot)
			{
				if (!HasChildren)
				{
					Block = tree.Block;
					AddChildren(tree.Children);
					return this;
				}
				if (tree.IsRoot)
				{
					return new SVBlockTree(Children.Union(tree.Children));
				}

				var added = SearchChildren(tree);
				if(!added)
				{
					AddChild(tree);
				}
				return this;
			}

			if (tree.IsRoot)
			{
				if(!tree.HasChildren)
				{
					return this;
				}

				SVBlockTree newTree = Merge(tree.Children.Last());
				foreach(var child in tree.Children)
				{
					newTree = newTree.Merge(child);
				}

				return newTree;
			}

			var block = tree.Block;
			if(block == null)
			{
				throw new InvalidOperationException();
			}
			int comparasion = Block.CompareTo(block);

			if (comparasion == 0)
			{
				return this;
			}
			if (comparasion < 0)
			{
				return tree.Merge(this);
			}

			// Block > block
			bool includes = Block.Includes(block);
			if (includes)
			{
				bool added = SearchChildren(tree);
				if (!added)
				{
					AddChild(tree);
				}

				return this;
			}

			if (Parent == null)
			{
				return new SVBlockTree(new[] { tree, this });
			}

			Parent.AddChild(tree);

			return Parent;
		}

		private bool SearchChildren(SVBlockTree tree)
		{
			// Block includes block (>)
			var firstIncludes = Children.Reverse().FirstOrDefault(t => t.Block.Includes(tree.Block));
			if (firstIncludes != null)
			{
				if (!firstIncludes.HasChildren)
				{
					firstIncludes.AddChild(tree);
					return true;
				}

				bool added = firstIncludes.SearchChildren(tree);
				if (!added)
				{
					firstIncludes.AddChild(tree);
					return true;
				}
			}

			return false;
		}

		private void AddChild(SVBlockTree tree)
		{
			_subblocks.TryAdd(tree.Block.Vector, tree);
			tree.Parent = this;
		}

		private void AddChildren(IEnumerable<SVBlockTree> children)
		{
			foreach (var child in children)
			{
				AddChild(child);
			}
		}
	}
}
