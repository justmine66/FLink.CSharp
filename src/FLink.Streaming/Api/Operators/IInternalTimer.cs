namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Internal interface for in-flight timers.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys to which timers are scoped.</typeparam>
    /// <typeparam name="TNamespace">Type of the namespace to which timers are scoped.</typeparam>

    public interface IInternalTimer<out TKey, out TNamespace>
    {
        /// <summary>
        /// Returns the timestamp of the timer. This value determines the point in time when the timer will fire.
        /// </summary>
        long Timestamp { get; }

        /// <summary>
        /// Returns the key that is bound to this timer.
        /// </summary>
        TKey Key { get; }

        /// <summary>
        /// Returns the namespace that is bound to this timer.
        /// </summary>
        TNamespace Namespace { get; }
    }
}
