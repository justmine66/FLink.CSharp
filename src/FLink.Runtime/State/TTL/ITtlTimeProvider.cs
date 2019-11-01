namespace FLink.Runtime.State.TTL
{
    /// <summary>
    /// Provides time to TTL logic to judge about state expiration.
    /// </summary>
    public interface ITtlTimeProvider
    {
        long CurrentTimestamp { get; }
    }
}
