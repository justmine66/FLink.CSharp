namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// An accumulator that sums up int values.
    /// </summary>
    public class IntCounter : ISimpleAccumulator<int>
    {
        private int _localValue = 0;

        public IntCounter() { }

        public IntCounter(int value)
        {
            _localValue = value;
        }

        public void Add(int value)
        {
            _localValue += value;
        }

        public int GetLocalValue()
        {
            return _localValue;
        }

        public void ResetLocal()
        {
            _localValue = 0;
        }

        public void Merge(IAccumulator<int, int> other)
        {
            _localValue += other.GetLocalValue();
        }

        public IAccumulator<int, int> Clone()
        {
            return new IntCounter { _localValue = _localValue };
        }
    }
}
