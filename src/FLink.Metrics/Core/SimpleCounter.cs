namespace FLink.Metrics.Core
{
    /// <summary>
    /// A simple low-overhead <see cref="ICounter"/> that is not thread-safe.
    /// </summary>
    public class SimpleCounter : ICounter
    {
        public void Increment() => Count++;

        public void Increment(long value) => Count += value;

        public void Decrement() => Count--;

        public void Decrement(long value) => Count -= value;

        public long Count { get; private set; }
    }
}
