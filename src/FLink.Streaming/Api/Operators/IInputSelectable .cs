using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for stream operators that can select the input to get <see cref="StreamRecord{T}"/>
    /// </summary>
    public interface IInputSelectable
    {
        /// <summary>
        /// Returns the next InputSelection that wants to get the record.
        /// This method is guaranteed to not be called concurrently with other methods of the operator.
        /// </summary>
        InputSelection NextSelection { get; }
    }
}
