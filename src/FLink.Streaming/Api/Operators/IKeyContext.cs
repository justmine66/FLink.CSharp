namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Inteface for setting and querying the current key of keyed operations.
    /// </summary>
    /// <remarks>
    /// This is mainly used by the timer system to query the key when creating timers and to set the correct key context when firing a timer.
    /// </remarks>
    public interface IKeyContext
    {
        void SetCurrentKey(object key);
        object GetCurrentKey();
    }
}
