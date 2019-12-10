using System;
using FLink.Core.Api.Common.Functions;

namespace FLink.Streaming.Api.Functions.Co
{
    /// <summary>
    /// A CoMapFunction implements a map() transformation over two connected streams.
    /// The same instance of the transformation function is used to transform both of the connected streams. That way, the stream transformations can share state.
    /// </summary>
    /// <typeparam name="TInput1">Type of the first input.</typeparam>
    /// <typeparam name="TInput2">Type of the second input.</typeparam>
    /// <typeparam name="TOutput">Output type.</typeparam>
    public interface ICoMapFunction<in TInput1, in TInput2, out TOutput> : IFunction
    {
        /// <summary>
        /// This method is called for each element in the first of the connected streams.
        /// </summary>
        /// <param name="value">The stream element</param>
        /// <returns>The resulting element</returns>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program to fail and go into recovery.</exception>
        TOutput Map1(TInput1 value);

        /// <summary>
        /// This method is called for each element in the second of the connected streams.
        /// </summary>
        /// <param name="value">The stream element</param>
        /// <returns>The resulting element</returns>
        /// <exception cref="Exception">The function may throw exceptions which cause the streaming program to fail and go into recovery.</exception>
        TOutput Map2(TInput2 value);
    }
}
