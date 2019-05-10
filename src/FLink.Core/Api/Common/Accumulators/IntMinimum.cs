using System;

namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that finds the minimum int value.
    /// </summary>
    public class IntMinimum : ISimpleAccumulator<int>
    {
        private int _min = int.MaxValue;

        public IntMinimum() { }

        public IntMinimum(int value)
        {
            _min = value;
        }

        public void Add(int value)
        {
            _min = Math.Min(_min, value);
        }

        public int GetLocalValue()
        {
            return _min;
        }

        public void ResetLocal()
        {
            _min = int.MaxValue;
        }

        public void Merge(IAccumulator<int, int> other)
        {
            _min = Math.Min(_min, other.GetLocalValue());
        }

        public IAccumulator<int, int> Clone()
        {
            return new IntMinimum() { _min = _min };
        }
    }
}
