namespace FLink.Streaming.Api.Windowing.Assigners
{
    /// <summary>
    /// A <see cref="ISessionWindowTimeGapExtractor{T}"/> extracts session time gaps for Dynamic Session Window Assigners.
    /// </summary>
    /// <typeparam name="T">The type of elements that this <see cref="ISessionWindowTimeGapExtractor{T}"/> can extract session time gaps from.</typeparam>
    public interface ISessionWindowTimeGapExtractor<in T>
    {
        /// <summary>
        /// Extracts the session time gap.
        /// </summary>
        /// <param name="element">The input element.</param>
        /// <returns>The session time gap in milliseconds.</returns>
        long Extract(T element);
    }
}
