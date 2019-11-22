namespace FLink.Core.IO
{
    /// <summary>
    /// This interface must be implemented by all kind of input splits that can be assigned to input formats.
    /// Input splits are transferred in serialized form via the messages, so they need to be serializable.
    /// </summary>
    public interface IInputSplit
    {
        /// <summary>
        /// Gets the number of this input split.
        /// </summary>
        int SplitNumber { get; }
    }
}
