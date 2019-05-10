using System.Collections.Generic;

namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// This accumulator stores a collection of objects.
    /// </summary>
    /// <typeparam name="T">The type of the accumulated objects.</typeparam>
    public class ListAccumulator<T> : IAccumulator<T, List<T>>
    {
        private List<T> _localValue = new List<T>();

        public void Add(T value)
        {
            _localValue.Add(value);
        }

        public List<T> GetLocalValue()
        {
            return _localValue;
        }

        public void ResetLocal()
        {
            _localValue.Clear();
        }

        public void Merge(IAccumulator<T, List<T>> other)
        {
            _localValue.AddRange(other.GetLocalValue());
        }

        public IAccumulator<T, List<T>> Clone()
        {
            return new ListAccumulator<T> { _localValue = new List<T>(_localValue) };
        }
    }
}
