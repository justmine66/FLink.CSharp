using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;

namespace WordCount
{
    /// <summary>
    /// 一个从Socket流中统计单词出现次数的例子.
    /// </summary>
    public class SocketWindowWordCount
    {
        public static void Run()
        {
            // get the execution environment
            var env = StreamExecutionEnvironment.GetExecutionEnvironment();

            // source: get input data by connecting to the socket.
            var stream = env.SocketTextStream("localhost", 5000, "\n");

            // transformation
            var stat = stream
                .FlatMap(new Splitter())// FlatMap算子对数据进行转换
                .KeyBy("word")// 按照指定key对数据进行分区，相同key的数据流向相同的SubTask实例。
                .TimeWindow(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1))
                .Reduce(new Reducer());

            // sink: 将数据输出到外部存储(控制台标准输出)。
            stat.Print().SetParallelism(1);

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
