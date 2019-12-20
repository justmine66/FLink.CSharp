using System.Collections.Generic;

namespace RealTimeDashboard
{
    public class OrderAccumulator
    {
        public HashSet<long> OrderIds { get; set; } = new HashSet<long>();

        public long SiteId { get; set; }
        public string SiteName { get; set; }
        public long QuantitySum { get; set; }
        public long SubOrderSum { get; set; }
        public long Gmv { get; set; }

        public void AddOrderId(long orderId) => OrderIds.Add(orderId);
        public void AddOrderIds(HashSet<long> orderIds)
        {
            foreach (var orderId in orderIds)
                AddOrderId(orderId);
        }
    }
}
