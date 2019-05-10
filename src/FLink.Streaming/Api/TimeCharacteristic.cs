namespace FLink.Streaming.Api
{
    /// <summary>
    /// The time characteristic defines how the system determines time for time-dependent order and operations that depend on time(such as time windows).
    /// </summary>
    public enum TimeCharacteristic
    {
        /// <summary>
        /// Processing time for operators means that the operator uses the system clock of the machine to determine the current time of the data stream.
        /// Processing-time windows trigger based on wall-clock time and include whatever elements happen to have arrived at the operator at that point in time.
        /// </summary>
        /// <remarks>
        ///  Using processing time for window operations results in general in quite non-deterministic results, because the contents of the windows depends on the speed in which elements arrive.It is, however, the cheapest method of forming windows and the method that introduces the least latency.
        /// </remarks>
        ProcessingTime,
        /// <summary>
        /// Ingestion time means that the time of each individual element in the stream is determined when the element enters the FLink streaming data flow. Operations like windows group the elements based on that time, meaning that processing speed within the streaming data flow does not affect windowing, but only the speed at which sources receive elements.
        /// </summary>
        /// <remarks>
        /// Ingestion time is often a good compromise between processing time and event time. It does not need any special manual form of watermark generation, and events are typically not too much out-or-order when they arrive at operators; in fact, out-of-orderless can only be introduced by streaming shuffles or split/join/union operations. The fact that elements are not very much out-of-order means that the latency increase is moderate, compared to event time.
        /// </remarks>
        IngestionTime,
        /// <summary>
        /// Event time means that the time of each individual element in the stream (also called event) is determined by the event's individual custom timestamp. These timestamps either exist in the elements from before they entered the FLink streaming dataflow, or are user-assigned at the sources. The big implication sources and in all operators out of order, meaning that elements with earlier timestamps may arrive after elements with later timestamps.
        /// </summary>
        /// <remarks>
        /// Operators that window or order data with respect to event time must buffer data until they can be sure that all timestamps for a certain time interval have been received. This is handled by the so called "time watermarks".
        /// </remarks>
        EventTime
    }
}
