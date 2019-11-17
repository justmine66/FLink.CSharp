using FLink.Core.Api.Common.Functions;

namespace FLink.Streaming.Api.Functions.Sink
{
    /// <summary>
    /// Interface for implementing user defined sink functionality.
    /// </summary>
    /// <typeparam name="TInput">Input type parameter.</typeparam>
    public interface ISinkFunction<TInput> : IFunction
    {
        /// <summary>
        /// Writes the given value to the sink. This function is called for every record.
        /// </summary>
        /// <param name="value">The input record.</param>
        /// <param name="context">Additional context about the input record.</param>
        /// <exception cref="System.Exception">This method may throw exceptions. Throwing an exception will cause the operation to fail and may trigger recovery.</exception>
        void Invoke(TInput value, ISinkContext<TInput> context);
    }

    public interface ISinkContext<T>
    {

        /// <summary>
        /// Gets the current processing time.
        /// </summary>
        long CurrentProcessingTime { get; }

        /// <summary>
        /// Gets the current event-time watermark.
        /// </summary>
        long CurrentWatermark { get; }

        /// <summary>
        /// Gets the timestamp of the current input record or {@code null} if the element does not have an assigned timestamp.
        /// </summary>
        long Timestamp { get; }
    }
}
