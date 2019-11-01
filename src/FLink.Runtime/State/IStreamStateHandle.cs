using FLink.Core.FS;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A <see cref="IStateObject"/> that represents state that was written to a stream. 
    /// </summary>
    public interface IStreamStateHandle : IStateObject
    {
        /// <summary>
        /// Returns an <see cref="FSDataInputStream"/> that can be used to read back the data that was previously written to the stream.
        /// </summary>
        FSDataInputStream OpenInputStream();
    }
}
