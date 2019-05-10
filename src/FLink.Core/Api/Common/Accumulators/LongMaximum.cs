using System;

namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that finds the minimum long value.
    /// </summary>
    public class LongMaximum : ISimpleAccumulator<long>
    {
        private long _max = long.MinValue;

        public LongMaximum() { }

        public LongMaximum(long value)
        {
            _max = value;
        }

        public void Add(long value)
        {
            _max = Math.Max(_max, value);
        }

        public long GetLocalValue()
        {
            return _max;
        }

        public void ResetLocal()
        {
            _max = long.MaxValue;
        }

        public void Merge(IAccumulator<long, long> other)
        {
            _max = Math.Max(_max, other.GetLocalValue());
        }

        public IAccumulator<long, long> Clone()
        {
            return new LongMaximum() { _max = _max };
        }
    }
}
