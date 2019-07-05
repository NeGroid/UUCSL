using System;
using System.Collections.Generic;
using System.Linq;

namespace UUCSL.Core
{
	public class Block : IEquatable<Block>
	{
		public SortedList<Word, WordCount> Words { get; private set; } = new SortedList<Word, WordCount>();

		public Block(IEnumerable<Word> words)
		{
			var allWords = words
				.GroupBy(t => t)
				.Select(t => (t.Key, WordCount.Create(t.Count())))
				.OrderBy(t => t.Key.Value);

			foreach(var word in allWords)
			{
				Words[word.Key] = word.Item2;
			}
		}

		public Block(IEnumerable<string> words)
			: this(words.Select(Word.Create))
		{
		}

		public static Block Create(string block) =>
			new Block(block.Split(' ').ToArray());

		public bool Equals(Block other) =>
			Words.All(t => other.Words.ContainsKey(t.Key) && other.Words[t.Key].Count <= t.Value.Count);
	}
}
