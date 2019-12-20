using System;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Streaming.Api;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace RealTimeDashboard
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = StreamExecutionEnvironment.GetExecutionEnvironment()
                    .SetStreamTimeCharacteristic(TimeCharacteristic.ProcessingTime)
                    .EnableCheckpointing(60 * 1000, CheckpointingMode.ExactlyOnce);

            env.CheckpointConfig.CheckpointTimeout = 30 * 1000;

            var partitionCount = 2;
            var orderExtTopicName = "orders";

            var sourceStream = env.AddSource<string>(null)
                .SetParallelism(partitionCount)
                .SetName("source_kafka_" + orderExtTopicName)
                .SetUId("source_kafka_" + orderExtTopicName);

            var orderStream = sourceStream.Map(new SubOrderDetailMapper())
                .SetName("map_sub_order_detail")
                .SetUId("map_sub_order_detail");

            var siteDayWindowStream = orderStream
                .KeyBy("siteId")
                .Window(TumblingProcessingTimeWindowAssigner<SubOrderDetail>.Of(TimeSpan.FromDays(1), TimeSpan.FromHours(-8)))
                .Trigger(ContinuousProcessingTimeTrigger<SubOrderDetail, TimeWindow>.Of(TimeSpan.FromSeconds(1)));

            var siteAggStream = siteDayWindowStream
                .Aggregate(new OrderAndGmvAggregateFunc())
                .SetName("aggregate_site_order_gmv")
                .SetUId("aggregate_site_order_gmv");

            var siteResultStream = siteAggStream
                .KeyBy(0)
                .Process(new OutputOrderGmvProcessFunc(), TypeInformation<(long, string)>.Of())
                .SetName("process_site_gmv_changed")
                .SetUId("process_site_gmv_changed");

            Console.WriteLine("Hello World!");
        }

        public class SubOrderDetailMapper : IMapFunction<string, SubOrderDetail>
        {
            public SubOrderDetail Map(string value)
            {
                throw new NotImplementedException();
            }
        }
    }
}
