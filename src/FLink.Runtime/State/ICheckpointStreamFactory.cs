using FLink.Core.FS;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A factory for checkpoint output streams, which are used to persist data for checkpoints.
    /// </summary>
    public interface ICheckpointStreamFactory
    {
        /// <summary>
        /// Creates an new <see cref="CheckpointStateOutputStream"/>. When the stream is closed, it returns a state handle that can retrieve the state back.
        /// </summary>
        /// <param name="scope">The state's scope, whether it is exclusive or shared.</param>
        /// <returns>An output stream that writes state for the given checkpoint.</returns>
        CheckpointStateOutputStream CreateCheckpointStateOutputStream(CheckpointedStateScope scope);
    }

    /// <summary>
    /// A dedicated output stream that produces a {@link StreamStateHandle} when closed.
    /// </summary>
    public abstract class CheckpointStateOutputStream : FSDataOutputStream
    {

    }
}
