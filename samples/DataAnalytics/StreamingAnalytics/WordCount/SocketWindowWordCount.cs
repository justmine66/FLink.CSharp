using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;

namespace WordCount
{
    public class SocketWindowWordCount
    {
        public static void Run()
        {
            // get the execution environment
            var env = StreamExecutionEnvironment.GetExecutionEnvironment();

            // get input data by connecting to the socket
            var text = env.SocketTextStream("localhost", 5000, "\n");
            var windowCounts = text
                .FlatMap(new Splitter())
                .KeyBy("word")
                .TimeWindow(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1))
                .Reduce(new Reducer());

            windowCounts.Print().SetParallelism(1);

            env.Execute("Socket Window WordCount");
        }

        public class Reducer : IReduceFunction<WordWithCount>
        {
            public WordWithCount Reduce(WordWithCount value1, WordWithCount value2)
            {
                return new WordWithCount(value1.Word, value1.Count + value2.Count);
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
