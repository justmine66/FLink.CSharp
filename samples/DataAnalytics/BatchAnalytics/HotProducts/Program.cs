using System;
using System.Collections.Generic;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Configurations;
using FLink.Core.Util;
using FLink.Streaming.Api;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Functions.Timestamps;
using FLink.Streaming.Api.Functions.Windowing;
using FLink.Streaming.Api.Windowing.Windows;

namespace HotProducts
{
    /// <summary>
    /// 一个统计实时热门商品例子
    /// 每隔5分钟统计过去一小时内点击量最多的前N个商品
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var env = StreamExecutionEnvironment.GetExecutionEnvironment()
                .SetStreamTimeCharacteristic(TimeCharacteristic.EventTime)
                .SetParallelism(1);

            var stat = env.ReadCsvFile<UserBehavior>("")// 创建数据源，得到UserBehavior类型的DataStream
                    .AssignTimestampsAndWatermarks(new UserBehaviorAscendingTimestampExtractor())// 抽取出时间和生成watermark
                    .Filter(new UserBehaviorFilter())// 过滤出只有点击的数据
                    .KeyBy("itemId")// 按商品分区统计
                    .TimeWindow(TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(5))// 窗口大小是一小时，每隔5分钟滑动一次
                    .Aggregate(new CountAggregator(), new WindowResultFunction())
                    .KeyBy("windowEnd")
                    .Process(new TopNHotProducts(3));

            stat.Print();

            env.Execute("Hot Products Job");
        }
    }

    public class UserBehaviorAscendingTimestampExtractor : AscendingTimestampExtractor<UserBehavior>
    {
        public override long ExtractAscendingTimestamp(UserBehavior element)
        {
            // 原始数据单位秒，将其转成毫秒
            return element.Timestamp * 1000;
        }
    }

    public class UserBehaviorFilter : IFilterFunction<UserBehavior>
    {
        public bool Filter(UserBehavior value)
        {
            // 过滤出只有点击的数据
            return value.Behavior.Equals("pv");
        }
    }

    public class WindowResultFunction : IWindowFunction<long, ProductClickCount, object, TimeWindow>
    {
        public void Apply(object key, TimeWindow window, IEnumerable<long> input, ICollector<ProductClickCount> output)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// COUNT统计的聚合函数实现，每出现一条记录加一.
    /// </summary>
    public class CountAggregator : IAggregateFunction<UserBehavior, long, long>
    {
        public long Add(UserBehavior value, long accumulator)
        {
            throw new NotImplementedException();
        }

        public long CreateAccumulator()
        {
            throw new NotImplementedException();
        }

        public long GetResult(long accumulator)
        {
            throw new NotImplementedException();
        }

        public long Merge(long a, long b)
        {
            throw new NotImplementedException();
        }
    }

    public class TopNHotProducts : KeyedProcessFunction<object, ProductClickCount, string>
    {
        private static int _topSize;

        public TopNHotProducts(int topSize)
        {
            _topSize = topSize;
        }

        public override void ProcessElement(ProductClickCount value, Context ctx, ICollector<string> output)
        {
            throw new NotImplementedException();
        }
    }
}
