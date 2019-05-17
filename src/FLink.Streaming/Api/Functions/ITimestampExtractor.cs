using FLink.Core.Api.Common.Functions;

namespace FLink.Streaming.Api.Functions
{
    public interface ITimestampExtractor<in T> : IFunction
    {
        /// <summary>
        /// Extracts a timestamp from an element.
        /// </summary>
        /// <param name="element">The element that the timestamp is extracted from.</param>
        /// <param name="currentTimestamp">The current internal timestamp of the element.</param>
        /// <returns></returns>
        long ExtractTimestamp(T element, long currentTimestamp);

        /// <summary>
        /// Asks the extractor if it wants to emit a watermark now that it has seen the given element.
        /// </summary>
        /// <param name="element">The element that we last saw.</param>
        /// <param name="currentTimestamp">The current timestamp of the element that we last saw.</param>
        /// <returns><see cref="long.MinValue"/> if no watermark should be emitted, positive value for emitting this value as a watermark.</returns>
        long ExtractWatermark(T element, long currentTimestamp);

        /// <summary>
        /// Returns the current watermark.
        /// This is periodically called by the system to determine the current watermark and forward it.
        /// </summary>
        long CurrentWatermark { get; }
    }
}
