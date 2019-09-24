using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;
using FLink.Streaming.Api.Environment;

namespace WordCount
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = ExecutionEnvironment.GetExecutionEnvironment();

            Console.Read();
        }
    }

    class Splitter : IFlatMapFunction<string, Tuple<string, int>>
    {
        public void FlatMap(string sentence, ICollector<Tuple<string, int>> output)
        {
            var words = sentence.Split(" ");
            foreach (var word in words)
            {
                output.Collect(new Tuple<string, int>(word, 1));
            }
        }
    }
}
