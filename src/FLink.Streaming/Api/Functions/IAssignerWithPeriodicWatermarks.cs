namespace FLink.Streaming.Api.Functions
{
    /// <summary>
    /// Assigns event time timestamps to elements, and generates low watermarks that signal event time progress within the stream.
    /// These timestamps and watermarks are used by functions and operators that operate on event time, for example event time windows.
    /// </summary>
    /// <typeparam name="T">The type of the elements to which this assigner assigns timestamps.</typeparam>
    public interface IAssignerWithPeriodicWatermarks<in T> : ITimestampAssigner<T>
    {
        /// <summary>
        /// Returns the current watermark. This method is periodically called by the system to retrieve the current watermark. if no watermark should be emitted, or the next watermark to emit.
        /// </summary>
        Watermark.Watermark CurrentWatermark { get; }
    }
}
