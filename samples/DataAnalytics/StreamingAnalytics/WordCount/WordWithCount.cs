namespace WordCount
{
    /// <summary>
    /// Data type for words with count
    /// </summary>
    public class WordWithCount
    {
        public string Word { get; }
        public long Count { get; }

        public WordWithCount(string word, long count)
        {
            Word = word;
            Count = count;
        }

        public override string ToString()
        {
            return Word + " : " + Count;
        }
    }
}
