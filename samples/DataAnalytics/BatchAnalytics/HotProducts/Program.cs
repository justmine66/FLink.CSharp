using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.CSharp.TypeUtils;
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
            // 创建 execution environment
            var env = StreamExecutionEnvironment.GetExecutionEnvironment()
                // 告诉系统按照 EventTime 处理
                .SetStreamTimeCharacteristic(TimeCharacteristic.EventTime)
                // 为了打印到控制台的结果不乱序，我们配置全局的并发为1，改变并发对结果正确性没有影响
                .SetParallelism(1);

            var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "UserBehavior.csv");

            var stream = env.ReadCsvFile<UserBehavior>("")// 创建数据源，得到UserBehavior类型的DataStream
                    .AssignTimestampsAndWatermarks(new UserBehaviorAscendingTimestampExtractor())// 抽取出时间和生成watermark
                    .Filter(new UserBehaviorFilter())// 过滤出只有点击的数据
                    .KeyBy("itemId")// 按商品分区统计
                    .TimeWindow(TimeSpan.FromMinutes(60), TimeSpan.FromMinutes(5))// 窗口大小是一小时，每隔5分钟滑动一次
                    .Aggregate(new CountAggregator(), new WindowResultFunction())// 获得每个窗口的点击量的数据流
                    .KeyBy("windowEnd") // 为了统计每个窗口下最热门的商品，再次按窗口进行分组.
                    .Process(new TopNHotProducts(3));// 计算点击量排名前3名的商品

            stream.Print();// 控制台打印输出

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

    /// <summary>
    /// COUNT统计的聚合函数实现，每出现一条记录加一.
    /// </summary>
    public class CountAggregator : IAggregateFunction<UserBehavior, long, long>
    {
        public long Add(UserBehavior value, long accumulator) => accumulator + 1;

        public long CreateAccumulator() => 0;

        public long GetResult(long accumulator) => accumulator;

        public long Merge(long a, long b) => a + b;
    }

    public class WindowResultFunction : IWindowFunction<long, ProductClickCount, string, TimeWindow>
    {
        public void Apply(string key, TimeWindow window, IEnumerable<long> aggregatedResult, ICollector<ProductClickCount> output)
        {
            var productId = int.Parse(key);
            var count = aggregatedResult.FirstOrDefault();

            output.Collect(new ProductClickCount(productId, window.End, count));
        }
    }

    public class TopNHotProducts : KeyedProcessFunction<string, ProductClickCount, string>
    {
        private readonly int _top;
        private IListState<ProductClickCount> _state;

        public TopNHotProducts(int top) => _top = top;

        public override void Open(Configuration parameters)
        {
            base.Open(parameters);

            var itemsStateDesc = new ListStateDescriptor<ProductClickCount>("itemState-state");
            _state = RuntimeContext.GetListState(itemsStateDesc);
        }

        public override void OnTimer(long timestamp, OnTimerContext ctx, ICollector<string> output)
        {
            // 获取收到的所有商品点击量
            var products = _state.Get().ToArray();
            // 提前清除状态中的数据，释放空间
            _state.Clear();
            // 按照点击量从大到小排序
            var sortedProducts = products.OrderByDescending(it => it.ViewCount);
            // 将排名信息格式化成String, 便于打印
            var result = new StringBuilder();
            result.Append("====================================\n");
            result.Append("时间: ").Append(DateTimeOffset.FromUnixTimeMilliseconds(timestamp - 1)).Append("\n");

            var index = 0;
            foreach (var product in sortedProducts)
            {
                // No1:  商品ID=12224  浏览量=2413
                result.Append("No").Append(++index).Append(":")
                    .Append("  商品ID=").Append(product.ProductId)
                    .Append("  浏览量=").Append(product.ViewCount)
                    .Append("\n");
                result.Append("====================================\n\n");

                if (index >= _top) break;
            }

            output.Collect(result.ToString());
        }

        public override void ProcessElement(ProductClickCount value, Context context, ICollector<string> output)
        {
            // 每条数据都保存到状态中
            _state.Add(value);
            // 注册windowEnd+1的EventTime Timer, 当触发时，说明收齐了属于windowEnd窗口的所有商品数据
            context.TimerService.RegisterEventTimeTimer(value.WindowEnd + 1);
        }
    }
}
