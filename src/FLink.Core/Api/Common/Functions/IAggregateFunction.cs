namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// The <see cref="IAggregateFunction{TIn,TAcc,TOut}"/> is a flexible aggregation function, characterized by the following features:
    /// </summary>
    /// <typeparam name="TIn">The type of the values that are aggregated (input values)</typeparam>
    /// <typeparam name="TAcc">The type of the accumulator (intermediate aggregate state)</typeparam>
    /// <typeparam name="TOut">The type of the aggregated result</typeparam>
    public interface IAggregateFunction<in TIn, TAcc, out TOut> : IFunction
    {
        /// <summary>
        /// Creates a new accumulator, starting a new aggregate.
        /// </summary>
        /// <returns></returns>
        TAcc CreateAccumulator();

        /// <summary>
        /// Adds the given input value to the given accumulator, returning the new accumulator value.
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <param name="accumulator">The accumulator to add the value to</param>
        /// <returns></returns>
        TAcc Add(TIn value, TAcc accumulator);

        /// <summary>
        /// Gets the result of the aggregation from the accumulator.
        /// </summary>
        /// <param name="accumulator">The accumulator of the aggregation</param>
        /// <returns>The final aggregation result.</returns>
        TOut GetResult(TAcc accumulator);

        /// <summary>
        /// Merges two accumulators, returning an accumulator with the merged state.
        /// </summary>
        /// <param name="a">An accumulator to merge</param>
        /// <param name="b">Another accumulator to merge</param>
        /// <returns></returns>
        TAcc Merge(TAcc a, TAcc b);
    }
}
