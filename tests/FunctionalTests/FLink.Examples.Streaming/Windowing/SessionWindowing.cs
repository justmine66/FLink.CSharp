using System;
using System.Collections.Generic;
using FLink.Streaming.Api;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Api.Windowing.Assigners;

namespace FLink.Examples.Streaming.Windowing
{
    /// <summary>
    /// An example of session windowing that keys events by ID and groups and counts them in session with gaps of 3 milliseconds.
    /// </summary>
    public class SessionWindowing
    {
        public static void Run()
        {
            var env = StreamExecutionEnvironment.GetExecutionEnvironment()
                .SetStreamTimeCharacteristic(TimeCharacteristic.EventTime)
                .SetParallelism(1);

            var source = env.AddSource(new DataSourceFunctor());

            var aggregated = source
                .KeyBy(0)
                .Window(EventTimeSessionWindowAssigner<SessionElement>.WithGap(TimeSpan.FromMilliseconds(3)))
                .Sum(2);

            aggregated.Print();

            env.Execute("SessionWindowing");
        }

        private static IEnumerable<SessionElement> GetSource()
        {
            yield return new SessionElement("a", 1L, 1);
            yield return new SessionElement("b", 1L, 1);
            yield return new SessionElement("b", 3L, 1);
            yield return new SessionElement("b", 5L, 1);
            yield return new SessionElement("a", 6L, 1);
        }

        public class SessionElement
        {
            public SessionElement(string name, long timestamp, int count)
            {
                Name = name;
                Timestamp = timestamp;
                Count = count;
            }

            public string Name { get; private set; }
            public long Timestamp { get; private set; }
            public int Count { get; private set; }
        }

        public class DataSourceFunctor : ISourceFunction<SessionElement>
        {
            public void Run(ISourceContext<SessionElement> ctx)
            {
                foreach (var element in GetSource())
                {
                    ctx.CollectWithTimestamp(element, element.Timestamp);
                    ctx.EmitWatermark(new Watermark(element.Timestamp - 1));
                }

                ctx.EmitWatermark(new Watermark(long.MaxValue));
            }

            public void Cancel() { }
        }
    }
}
