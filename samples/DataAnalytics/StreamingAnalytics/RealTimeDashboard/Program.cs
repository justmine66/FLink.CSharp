using System;
using System.Text.Json;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Streaming.Api;
using FLink.Streaming.Api.Environments;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace RealTimeDashboard
{
    /// <summary>
    /// 实时大屏(实时统计每个站点成交额)
    /// </summary>
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

            // 1. 读取kafka创建源数据
            var sourceStream = env.AddSource<string>(null)
                .SetParallelism(partitionCount)
                .SetName("source_kafka_" + orderExtTopicName)
                .SetUId("source_kafka_" + orderExtTopicName);

            // 2. 创建SubOrderDetail数据流
            var orderStream = sourceStream.Map(new SubOrderDetailMapper())
                .SetName("map_sub_order_detail")
                .SetUId("map_sub_order_detail");

            // 3. 统计每天的数据，每秒持续输出。
            var siteDayWindowStream = orderStream
                .KeyBy("siteId")
                .Window(TumblingProcessingTimeWindowAssigner<SubOrderDetail>.Of(TimeSpan.FromDays(1), TimeSpan.FromHours(-8)))
                .Trigger(ContinuousProcessingTimeTrigger<SubOrderDetail, TimeWindow>.Of(TimeSpan.FromSeconds(1)));

            // 5. 计算站点聚合指标
            var siteAggStream = siteDayWindowStream
                .Aggregate(new OrderAndGmvAggregateFunc())
                .SetName("aggregate_site_order_gmv")
                .SetUId("aggregate_site_order_gmv");

            // 6. 只输出变化的聚合指标
            var siteResultStream = siteAggStream
                .KeyBy(0)
                .Process(new OutputOrderGmvProcessFunc(), TypeInformation.Of<(long, string)>())
                .SetName("process_site_gmv_changed")
                .SetUId("process_site_gmv_changed");

            // todo:
            // 6. Sink到Redis，利用有序集合完成TopN排序。
            // 显示端直接从Redis中查询结果。

            Console.WriteLine("Hello World!");
        }

        public class SubOrderDetailMapper : IMapFunction<string, SubOrderDetail>
        {
            public SubOrderDetail Map(string value) => JsonSerializer.Deserialize<SubOrderDetail>(value);
        }
    }
}
