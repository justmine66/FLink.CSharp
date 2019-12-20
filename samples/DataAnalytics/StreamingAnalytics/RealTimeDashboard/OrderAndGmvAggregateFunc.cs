using FLink.Core.Api.Common.Functions;

namespace RealTimeDashboard
{
    public class OrderAndGmvAggregateFunc : IAggregateFunction<SubOrderDetail, OrderAccumulator, OrderAccumulator>
    {
        public OrderAccumulator Add(SubOrderDetail value, OrderAccumulator acc)
        {
            if (acc.SiteId == 0)
            {
                acc.SiteId = value.SiteId;
                acc.SiteName = value.SiteName;
            }

            acc.AddOrderId(value.OrderId);
            acc.QuantitySum += value.Quantity;
            acc.SubOrderSum++;
            acc.Gmv += value.Price * value.Quantity;

            return acc;
        }

        public OrderAccumulator CreateAccumulator() => new OrderAccumulator();

        public OrderAccumulator GetResult(OrderAccumulator accumulator) => accumulator;

        public OrderAccumulator Merge(OrderAccumulator acc1, OrderAccumulator acc2)
        {
            if (acc1.SiteId == 0)
            {
                acc1.SiteId = acc2.SiteId;
                acc1.SiteName = acc2.SiteName;
            }

            acc1.AddOrderIds(acc2.OrderIds);
            acc1.QuantitySum += acc2.QuantitySum;
            acc1.SubOrderSum += acc2.SubOrderSum;
            acc1.Gmv += acc2.Gmv;

            return acc1;
        }
    }
}
