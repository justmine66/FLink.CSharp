using System;

namespace FLink.Core.Api.Common.Accumulators
{
    public class AverageAccumulator : ISimpleAccumulator<double>
    {
        private long _count;

        private double _sum;

        public void Add(double value)
        {
            _count++;
            _sum += value;
        }

        public double GetLocalValue()
        {
            if (_count == 0) return 0.0;

            return _sum / _count;
        }

        public void ResetLocal()
        {
            _count = 0;
            _sum = 0;
        }

        public void Merge(IAccumulator<double, double> other)
        {
            if (other is AverageAccumulator avg)
            {
                _count += avg._count;
                _sum += avg._sum;
            }
            else
            {
                throw new NotSupportedException("The merged accumulator must be AverageAccumulator.");
            }
        }

        public IAccumulator<double, double> Clone()
        {
            return new AverageAccumulator { _count = _count, _sum = _sum };
        }

        public override string ToString()
        {
            return "AverageAccumulator " + GetLocalValue() + " for " + _count + " elements";
        }
    }
}
