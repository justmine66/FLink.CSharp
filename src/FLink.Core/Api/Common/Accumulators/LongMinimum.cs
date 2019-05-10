using System;

namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that finds the minimum long value.
    /// </summary>
    public class LongMinimum : ISimpleAccumulator<long>
    {
        private long _min = long.MaxValue;

        public LongMinimum() { }

        public LongMinimum(long value)
        {
            _min = value;
        }

        public void Add(long value)
        {
            _min = Math.Min(_min, value);
        }

        public long GetLocalValue()
        {
            return _min;
        }

        public void ResetLocal()
        {
            _min = long.MaxValue;
        }

        public void Merge(IAccumulator<long, long> other)
        {
            _min = Math.Min(_min, other.GetLocalValue());
        }

        public IAccumulator<long, long> Clone()
        {
            return new LongMinimum() { _min = _min };
        }
    }
}
