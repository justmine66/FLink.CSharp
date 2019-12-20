namespace RealTimeDashboard
{
    public class SubOrderDetail
    {
        public long UserId { get; set; }
        public long OrderId { get; set; }
        public long SubOrderId { get; set; }
        public long SiteId { get; set; }
        public string SiteName { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
        public long WarehouseId { get; set; }
        public long MerchandiseId { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public int OrderStatus { get; set; }
        public int IsNewOrder { get; set; }
        public long Timestamp { get; set; }
    }
}
