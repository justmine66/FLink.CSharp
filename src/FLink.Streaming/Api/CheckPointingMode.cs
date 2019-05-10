namespace FLink.Streaming.Api
{
    /// <summary>
    /// The check pointing mode defines what consistency guarantees the system gives in the presence of failures.
    /// </summary>
    public enum CheckPointingMode
    {
        ExactlyOnce,
        AtLeastOnce
    }
}
