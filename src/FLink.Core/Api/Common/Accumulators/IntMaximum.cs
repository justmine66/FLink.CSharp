using System;

namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that finds the minimum int value.
    /// </summary>
    public class IntMaximum : ISimpleAccumulator<int>
    {
        private int _max = int.MinValue;

        public IntMaximum() { }

        public IntMaximum(int value)
        {
            _max = value;
        }

        public void Add(int value)
        {
            _max = Math.Max(_max, value);
        }

        public int GetLocalValue()
        {
            return _max;
        }

        public void ResetLocal()
        {
            _max = int.MaxValue;
        }

        public void Merge(IAccumulator<int, int> other)
        {
            _max = Math.Max(_max, other.GetLocalValue());
        }

        public IAccumulator<int, int> Clone()
        {
            return new IntMaximum() { _max = _max };
        }
    }
}
