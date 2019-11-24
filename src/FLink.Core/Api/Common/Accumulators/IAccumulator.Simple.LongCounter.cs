namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that sums up long values.
    /// </summary>
    public class LongCounter : ISimpleAccumulator<long>
    {
        private long _localValue = 0;

        public LongCounter() { }

        public LongCounter(long value)
        {
            _localValue = value;
        }

        public void Add(long value)
        {
            _localValue += value;
        }

        public long GetLocalValue()
        {
            return _localValue;
        }

        public void ResetLocal()
        {
            _localValue = 0;
        }

        public void Merge(IAccumulator<long, long> other)
        {
            _localValue += other.GetLocalValue();
        }

        public IAccumulator<long, long> Clone()
        {
            return new LongCounter { _localValue = _localValue };
        }
    }
}
