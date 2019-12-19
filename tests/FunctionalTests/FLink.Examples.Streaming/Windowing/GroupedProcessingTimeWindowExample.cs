using System;
using FLink.Core.Api.Common.Functions;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Functions.Sink;
using FLink.Streaming.Api.Functions.Source;

namespace FLink.Examples.Streaming.Windowing
{
    /// <summary>
    /// An example of grouped stream windowing into sliding time windows.
    /// This example uses [[RichParallelSourceFunction]] to generate a list of key-value pairs.
    /// </summary>
    public class GroupedProcessingTimeWindowExample
    {
        public static void Run()
        {
            var env = StreamExecutionEnvironment.GetExecutionEnvironment()
                .SetParallelism(4);

            var source = env.AddSource(new DataSource());

            var steam = source.KeyBy(0)
                .TimeWindow(TimeSpan.FromMilliseconds(2500), TimeSpan.FromMilliseconds(500))
                .Reduce(new SummingReducer())
                .AddSink(new EmptySink());

            env.Execute();
        }

        public class Element
        {
            public Element(long key, long value)
            {
                Key = key;
                Value = value;
            }

            public long Key { get; private set; }
            public long Value { get; private set; }
        }

        private class DataSource : RichParallelSourceFunction<Element>
        {
            private volatile bool _running = true;

            public override void Run(ISourceContext<Element> ctx)
            {
                var startTime = DateTime.Now.Ticks;
                var numElements = 20000000;
                var numKeys = 10000;
                var val = 1L;
                var count = 0L;

                while (_running && count < numElements)
                {
                    count++;
                    ctx.Collect(new Element(1L, val++));

                    if (val > numKeys) val = 1L;
                }

                var endTime = DateTime.Now.Ticks;
                Console.WriteLine("Took " + TimeSpan.FromTicks(endTime - startTime).TotalMilliseconds + " msecs for " + numElements + " values");
            }

            public override void Cancel() => _running = false;
        }

        private class SummingReducer : IReduceFunction<Element>
        {
            public Element Reduce(Element value1, Element value2) => new Element(value1.Key, value1.Value + value2.Value);
        }

        private class EmptySink : ISinkFunction<Element>
        {
            public void Invoke(Element value, ISinkContext<Element> context) { }
        }
    }
}
