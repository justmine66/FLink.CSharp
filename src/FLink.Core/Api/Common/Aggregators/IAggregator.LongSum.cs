using FLink.Core.Types;

namespace FLink.Core.Api.Common.Aggregators
{
    /// <summary>
    /// An <see cref="IAggregator{TValue}"/> that sums up long values.
    /// </summary>
    public class LongSumAggregator : IAggregator<LongValue>
    {
        private long _sum;

        public LongValue GetAggregate() => new LongValue(_sum);

        public void Aggregate(long value) => _sum += value;
        public void Aggregate(LongValue element) => _sum += element.Value;

        public void Reset() => _sum = 0;
    }
}
