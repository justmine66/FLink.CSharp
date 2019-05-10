using System;

namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that finds the minimum int value.
    /// </summary>
    public class DoubleMinimum : ISimpleAccumulator<double>
    {
        private double _min = double.MaxValue;

        public DoubleMinimum() { }

        public DoubleMinimum(double value)
        {
            _min = value;
        }

        public void Add(double value)
        {
            _min = Math.Min(_min, value);
        }

        public double GetLocalValue()
        {
            return _min;
        }

        public void ResetLocal()
        {
            _min = double.MaxValue;
        }

        public void Merge(IAccumulator<double, double> other)
        {
            _min = Math.Min(_min, other.GetLocalValue());
        }

        public IAccumulator<double, double> Clone()
        {
            return new DoubleMinimum() { _min = _min };
        }
    }
}
