namespace FLink.Streaming.Api.Functions.Windowing.Delta
{
    /// <summary>
    /// This interface allows the implementation of a function which calculates the delta between two data points. Delta functions might be used in delta policies and allow flexible adaptive windowing based on the arriving data points.
    /// </summary>
    /// <typeparam name="TData">The type of input data which can be compared using this function.</typeparam>
    public interface IDeltaFunction<in TData>
    {
        /// <summary>
        /// Calculates the delta between two given data points.
        /// </summary>
        /// <param name="oldDataPoint">the old data point.</param>
        /// <param name="newDataPoint">the new data point.</param>
        /// <returns>the delta between the two given points.</returns>
        double GetDelta(TData oldDataPoint, TData newDataPoint);
    }
}
