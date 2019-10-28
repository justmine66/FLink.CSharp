using FLink.Core.Api.Common.Functions;

namespace FLink.Streaming.Api.Functions
{
    /// <summary>
    /// A <see cref="ITimestampAssigner{TElement}"/> assigns event time timestamps to elements. These timestamps are used by all functions that operate on event time, for example event time windows.
    /// </summary>
    /// <remarks>Timestamps are represented in milliseconds since the Epoch (midnight, January 1, 1970 UTC).</remarks>
    /// <typeparam name="TElement">The type of the elements to which this assigner assigns timestamps.</typeparam>
    public interface ITimestampAssigner<in TElement> : IFunction
    {
        /// <summary>
        /// Assigns a timestamp to an element, in milliseconds since the Epoch.
        /// </summary>
        /// <remarks>
        /// The method is passed the previously assigned timestamp of the element. That previous timestamp may have been assigned from a previous assigner, by ingestion time.If the element did not carry a timestamp before, this value is <see cref="long.MinValue"/>.
        /// </remarks>
        /// <param name="element">The element that the timestamp will be assigned to.</param>
        /// <param name="previousElementTimestamp">The previous internal timestamp of the element, or a negative value, if no timestamp has been assigned yet.</param>
        /// <returns></returns>
        long ExtractTimestamp(TElement element, long previousElementTimestamp);
    }
}
