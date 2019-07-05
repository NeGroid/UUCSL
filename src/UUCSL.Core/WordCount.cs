namespace UUCSL.Core
{
	public struct WordCount
	{
		public byte Count { get; private set; }

		public WordCount(int count)
		{
			Count = (byte)count;
		}

		public static WordCount Create(int count) => new WordCount(count);
	}
}
