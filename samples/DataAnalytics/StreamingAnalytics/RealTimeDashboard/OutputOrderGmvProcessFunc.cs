using System.Text.Json;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Configurations;
using FLink.Streaming.Api.Functions;

namespace RealTimeDashboard
{
    public class OutputOrderGmvProcessFunc : KeyedProcessFunction<int, OrderAccumulator, (long, string)>
    {
        private IMapState<long, OrderAccumulator> _state;

        public override void Open(Configuration parameters)
        {
            var descriptor = new MapStateDescriptor<long, OrderAccumulator>("state_site_order_gmv",
                TypeInformation<long>.Of(), TypeInformation<OrderAccumulator>.Of());

            _state = RuntimeContext.GetMapState(descriptor);
        }

        public override void OnTimer(long timestamp, OnTimerContext context, FLink.Core.Util.ICollector<(long, string)> output)
        {
            throw new System.NotImplementedException();
        }

        public override void ProcessElement(OrderAccumulator value, Context context, FLink.Core.Util.ICollector<(long, string)> output)
        {
            var key = value.SiteId;
            var cachedValue = _state.Get(key);

            if (cachedValue == null || value.SubOrderSum != cachedValue.SubOrderSum)
            {
                var result = JsonSerializer.Serialize(value);
                output.Collect((key, result));
                _state.Put(key, value);
            }
        }

        public override void Close()
        {
            _state.Clear();
            base.Close();
        }
    }
}
