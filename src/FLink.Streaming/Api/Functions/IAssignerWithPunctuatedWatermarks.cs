using FLink.Streaming.Api.Watermarks;

namespace FLink.Streaming.Api.Functions
{
    /// <summary>
    /// The <see cref="IAssignerWithPunctuatedWatermarks{T}"/> assigns event time timestamps to elements, and generates low watermarks that signal event time progress within the stream. These timestamps and watermarks are used by functions and operators that operate on event time, for example event time windows.
    /// </summary>
    /// <typeparam name="T">The type of the elements to which this assigner assigns timestamps.</typeparam>
    public interface IAssignerWithPunctuatedWatermarks<in T> : ITimestampAssigner<T>
    {
        /// <summary>
        ///  Asks this implementation if it wants to emit a watermark. This method is called right after the <see cref="ITimestampAssigner{T}.ExtractTimestamp"/> method.
        /// </summary>
        /// <param name="lastElement"></param>
        /// <param name="extractedTimestamp"></param>
        /// <returns></returns>
        Watermark CheckAndGetNextWatermark(T lastElement, long extractedTimestamp);
    }
}
