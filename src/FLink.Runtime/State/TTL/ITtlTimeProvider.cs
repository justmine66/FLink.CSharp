using System;

namespace FLink.Runtime.State.TTL
{
    /// <summary>
    /// Provides time to TTL logic to judge about state expiration.
    /// </summary>
    public interface ITtlTimeProvider
    {
        long CurrentTimestamp { get; }
    }

    public class TtlTimeProvider : ITtlTimeProvider
    {
        public static ITtlTimeProvider Default = new TtlTimeProvider();

        public long CurrentTimestamp { get; } = DateTime.Now.Millisecond;
    }
}
