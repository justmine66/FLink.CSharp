namespace FLink.Streaming.Runtime.IO
{
    /// <summary>
    /// One input state which might be currently available, not available or already finished.
    /// </summary>
    public enum InputStatus
    {
        /// <summary>
        /// Indicator that more data is available and the input can be called immediately again to emit more data.
        /// </summary>
        MoreAvailable,
        /// <summary>
        /// Indicator that no data is currently available, but more data will be available in the future again.
        /// </summary>
        NothingAvailable,
        /// <summary>
        /// Indicator that the input has reached the end of data.
        /// </summary>
        EndOfInput
    }
}
