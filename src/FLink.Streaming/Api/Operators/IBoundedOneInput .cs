namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for the one-input operators that can process EndOfInput event.
    /// </summary>
    public interface IBoundedOneInput
    {
        /// <summary>
        /// It is notified that no more data will arrive on the input.
        /// </summary>
        void EndInput();
    }
}
