using FLink.Core.Api.Common.Functions;
using FLink.Streaming.Api;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions.Timestamps;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Evictors;
using FLink.Streaming.Api.Windowing.Windows;
using System;

namespace FLink.Examples.Streaming.Windowing
{
    public class TopSpeedWindowing
    {
        public static void Run()
        {
            var env = StreamExecutionEnvironment.GetExecutionEnvironment()
                .SetStreamTimeCharacteristic(TimeCharacteristic.EventTime)
                .SetParallelism(1);

            var stream = env.ReadTextFile("").Map(new CarDatumMapper());

            var transformation = stream.AssignTimestampsAndWatermarks(new CarTimestampExtractor())
                .KeyBy("Id")
                .Window(GlobalWindowAssigner<CarDatum>.Create())
                .Evictor(TimeWindowEvictor.Of<CarDatum, GlobalWindow>(TimeSpan.FromSeconds(10)))
                .MaxBy(1);

            transformation.Print();

            env.Execute("CarTopSpeedWindowingExample");
        }

        public class CarDatum
        {
            public int Id { get; set; }
            public int Speed { get; set; }
            public double Distance { get; set; }
            public long Timestamp { get; set; }
        }

        public class CarDatumMapper : IMapFunction<string, CarDatum>
        {
            public CarDatum Map(string value)
            {
                throw new System.NotImplementedException();
            }
        }

        public class CarTimestampExtractor : AscendingTimestampExtractor<CarDatum>
        {
            public override long ExtractAscendingTimestamp(CarDatum element) => element.Timestamp;
        }
    }
}
