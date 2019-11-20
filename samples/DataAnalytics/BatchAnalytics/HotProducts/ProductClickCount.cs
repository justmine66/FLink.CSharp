namespace HotProducts
{
    /// <summary>
    /// 商品点击量(窗口操作的输出类型)
    /// </summary>
    public class ProductClickCount
    {
        public ProductClickCount(long productId, long windowEnd, long viewCount)
        {
            ProductId = productId;
            WindowEnd = windowEnd;
            ViewCount = viewCount;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; }
        /// <summary>
        /// 窗口结束时间戳
        /// </summary>
        public long WindowEnd { get; }
        /// <summary>
        /// 商品的点击量
        /// </summary>
        public long ViewCount { get; }
    }
}
