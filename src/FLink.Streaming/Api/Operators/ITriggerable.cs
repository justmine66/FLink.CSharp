namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for things that can be called by <see cref="IInternalTimerService{TNamespace}"/>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TNamespace"></typeparam>
    public interface ITriggerable<TKey, TNamespace>
    {
        /// <summary>
        /// Invoked when an event-time timer fires.
        /// </summary>
        /// <param name="timer"></param>
        void OnEventTime(IInternalTimer<TKey, TNamespace> timer);

        /// <summary>
        /// Invoked when a processing-time timer fires.
        /// </summary>
        /// <param name="timer"></param>
        void OnProcessingTime(IInternalTimer<TKey, TNamespace> timer);
    }
}
