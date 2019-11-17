namespace FLink.Streaming.Api
{
    /// <summary>
    /// Specifies whether a firing timer is based on event time or processing time.
    /// </summary>
    public enum TimeDomain
    {
        EventTime,
        ProcessingTime
    }
}
