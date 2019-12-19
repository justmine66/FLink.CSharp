using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Environments;

namespace FLink.Examples.Streaming.Windowing
{
    /// <summary>
    /// Implements a windowed version of the streaming "WordCount" program.
    /// The input is a plain text file with lines separated by newline characters.
    /// </summary>
    public class WindowWordCount
    {
        public static void Run()
        {
            var env = StreamExecutionEnvironment.GetExecutionEnvironment();

            var steam = env.ReadTextFile("");

            var transformation = steam.FlatMap(new Splitter())
                .KeyBy(0)
                .CountWindow(10, 5)
                .Sum(1); // sum up tuple field "Count"

            transformation.Print();

            env.Execute("WindowWordCount");
        }

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

        public class Splitter : IFlatMapFunction<string, WordWithCount>
        {
            public void FlatMap(string sentence, ICollector<WordWithCount> output)
            {
                var words = sentence.Split("\\s");
                foreach (var word in words)
                    output.Collect(new WordWithCount(word, 1L));
            }
        }
    }
}
