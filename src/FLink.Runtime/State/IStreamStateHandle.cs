using FLink.Core.FS;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A <see cref="IStateObject"/> that represents state that was written to a stream. 
    /// </summary>
    public interface IStreamStateHandle
    {
        FSDataInputStream OpenInputStream();
    }
}
