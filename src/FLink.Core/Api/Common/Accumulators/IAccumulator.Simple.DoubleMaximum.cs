using System;

namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that finds the minimum double value.
    /// </summary>
    public class DoubleMaximum : ISimpleAccumulator<double>
    {
        private double _max = double.MinValue;

        public DoubleMaximum() { }

        public DoubleMaximum(double value)
        {
            _max = value;
        }

        public void Add(double value)
        {
            _max = Math.Max(_max, value);
        }

        public double GetLocalValue()
        {
            return _max;
        }

        public void ResetLocal()
        {
            _max = double.MaxValue;
        }

        public void Merge(IAccumulator<double, double> other)
        {
            _max = Math.Max(_max, other.GetLocalValue());
        }

        public IAccumulator<double, double> Clone()
        {
            return new DoubleMaximum() { _max = _max };
        }
    }
}
