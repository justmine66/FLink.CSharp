using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Util;

namespace WordCount
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
