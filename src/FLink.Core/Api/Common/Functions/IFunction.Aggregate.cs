namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// The <see cref="IAggregateFunction{TInput,TAccumulator,TOutput}"/> is a flexible aggregation function.
    /// </summary>
    /// <typeparam name="TInput">The type of the values that are aggregated (input values)</typeparam>
    /// <typeparam name="TAccumulator">The type of the accumulator (intermediate aggregate state)</typeparam>
    /// <typeparam name="TOutput">The type of the aggregated result</typeparam>
    public interface IAggregateFunction<in TInput, TAccumulator, out TOutput> : IFunction
    {
        /// <summary>
        /// Creates a initial accumulator, starting a new aggregate.
        /// </summary>
        /// <returns>a new accumulator</returns>
        TAccumulator CreateAccumulator();

        /// <summary>
        /// Adds the given input value to the given accumulator, returning the new accumulator value.
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <param name="accumulator">The accumulator to add the value to</param>
        /// <returns>returning the new accumulator value.</returns>
        TAccumulator Add(TInput value, TAccumulator accumulator);

        /// <summary>
        /// Gets the result of the aggregation from the accumulator.
        /// </summary>
        /// <param name="accumulator">The accumulator of the aggregation</param>
        /// <returns>The final aggregation result.</returns>
        TOutput GetResult(TAccumulator accumulator);

        /// <summary>
        /// Merges two accumulators.
        /// </summary>
        /// <param name="acc1">An accumulator to merge</param>
        /// <param name="acc2">Another accumulator to merge</param>
        /// <returns>The accumulator with the merged state</returns>
        TAccumulator Merge(TAccumulator acc1, TAccumulator acc2);
    }
}
