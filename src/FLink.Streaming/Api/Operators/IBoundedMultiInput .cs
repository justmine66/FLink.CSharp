namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for the multi-input operators that can process EndOfInput event.
    /// </summary>
    public interface IBoundedMultiInput
    {
        /// <summary>
        /// It is notified that no more data will arrive on the input identified by the <paramref name="inputId"/>.
        /// The <paramref name="inputId"/> is numbered starting from 1, and `1` indicates the first input.
        /// </summary>
        /// <param name="inputId"></param>
        void EndInput(int inputId);
    }
}
