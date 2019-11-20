namespace HotProducts
{
    /// <summary>
    /// 用户行为数据结构
    /// </summary>
    public class UserBehavior
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 商品类目ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 用户行为, 包括("pv", "buy", "cart", "fav")
        /// </summary>
        public string Behavior { get; set; }
        /// <summary>
        /// 行为发生的时间戳，单位秒
        /// </summary>
        public long Timestamp { get; set; }
    }
}
