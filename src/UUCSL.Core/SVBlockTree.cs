using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UUCSL.Core
{
	public class SVBlockTree
	{
		private readonly SortedDictionary<SVVector, SVBlockTree> _subblocks = new SortedDictionary<SVVector, SVBlockTree>();

		public SVBlock Block { get; }

		public bool IsRoot => Block is null;

		public ICollection<SVBlockTree> Children => _subblocks.Values;

		private SVBlockTree(IEnumerable<SVBlockTree> children)
		{
			if (children is null)
			{
				throw new ArgumentNullException(nameof(children));
			}

			AddChildren(children);

			if (Children.Count > 0)
			{
				Block = CreateRoot(Children.First().Block.Vector.Length);
			}
		}

		public SVBlockTree(SVBlock block)
		{
			if (block is null)
			{
				throw new ArgumentNullException(nameof(block));
			}

			Block = block;
		}

		public SVBlockTree(int vectorLength)
		{
			Block = CreateRoot(vectorLength);
		}

		public SVBlockTree Add(SVBlock block)
		{
			if (block is null)
			{
				throw new ArgumentNullException(nameof(block));
			}

			var tree = Includes(block);
			if (tree == null)
			{
				return new SVBlockTree(new[] { this, new SVBlockTree(block) });
			}

			var childTree = ChildrenIncludes(tree, block);
			if (childTree == null)
			{
				tree.AddChild(block);
				return tree;
			}

			return childTree.Add(block);
		}

		public SVBlockTree Includes(SVBlock block)
		{
			bool hasChildren = Children.Count > 0;
			bool includes = Block?.Includes(block) ?? false;
			var current = includes ? this : null;

			if (includes && !hasChildren)
			{
				return this;
			}

			if (hasChildren)
			{
				return ChildrenIncludes(this, block) ?? current;
			}

			return current;
		}

		private static SVBlockTree ChildrenIncludes(SVBlockTree parent, SVBlock block)
		{
			foreach (var child in parent.Children.Reverse())
			{
				var tree = child.Includes(block);
				if (tree != null)
				{
					return tree;
				}
				if (block.Includes(child.Block))
				{
					var newChild = new SVBlockTree(block);
					parent.Replace(child, newChild);

					return newChild;
				}
			}

			return null;
		}

		private void Replace(SVBlockTree oldChild, SVBlockTree newChild)
		{
			_subblocks.Remove(oldChild.Block.Vector);
			_subblocks.Add(newChild.Block.Vector, newChild);
		}

		private void AddChild(SVBlock block)
		{
			_subblocks.Add(block.Vector, new SVBlockTree(block));
		}

		private void AddChildren(IEnumerable<SVBlockTree> children)
		{
			foreach (var child in children)
			{
				_subblocks.Add(child.Block.Vector, child);
			}
		}

		private static SVBlock CreateRoot(int vectorLength)
		{
			var vector = new StringBuilder();
			foreach (var nine in Enumerable.Range(1, vectorLength).Select(_ => '9'))
			{
				vector.Append(nine);
			}
			var svVector = SVVector.FromSV(vector.ToString());
			return new SVBlock(svVector, "ROOT");
		}
	}
}
