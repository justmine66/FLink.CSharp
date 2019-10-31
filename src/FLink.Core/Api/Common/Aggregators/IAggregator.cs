namespace FLink.Core.Api.Common.Aggregators
{
    /// <summary>
    /// Aggregators are a means of aggregating values across parallel instances of a function.
    /// </summary>
    /// <typeparam name="TValue">The type of the aggregated value.</typeparam>
    public interface IAggregator<TValue>
    {
        /// <summary>
        /// Gets the aggregator's current aggregate.
        /// </summary>
        /// <returns>The aggregator's current aggregate.</returns>
        TValue GetAggregate();

        /// <summary>
        /// Aggregates the given element. In the case of a <i>sum</i> aggregator, this method adds the given value to the sum.
        /// </summary>
        /// <param name="element">The element to aggregate.</param>
        void Aggregate(TValue element);

        /// <summary>
        /// Resets the internal state of the aggregator. This must bring the aggregator into the same state as if it was newly initialized.
        /// </summary>
        void Reset();
    }
}
