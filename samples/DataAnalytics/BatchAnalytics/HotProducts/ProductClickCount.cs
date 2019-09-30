namespace HotProducts
{
    /// <summary>
    /// 商品点击量(窗口操作的输出类型)
    /// </summary>
    public class ProductClickCount
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 窗口结束时间戳
        /// </summary>
        public long WindowEnd { get; set; }
        /// <summary>
        /// 商品的点击量
        /// </summary>
        public long ViewCount { get; set; }
    }
}
