namespace FLink.Core.Api.Common.Accumulators
{
    public class DoubleCounter : ISimpleAccumulator<double>
    {
        private double _localValue = 0;

        public DoubleCounter() { }

        public DoubleCounter(double value)
        {
            _localValue = value;
        }

        public void Add(double value)
        {
            _localValue += value;
        }

        public double GetLocalValue()
        {
            return _localValue;
        }

        public void ResetLocal()
        {
            _localValue = 0;
        }

        public void Merge(IAccumulator<double, double> other)
        {
            _localValue += other.GetLocalValue();
        }

        public IAccumulator<double, double> Clone()
        {
            return new DoubleCounter { _localValue = _localValue };
        }
    }
}
