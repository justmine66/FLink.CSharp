using FLink.Core.FS;

namespace FLink.Runtime.State
{
    /// <summary>
    /// An output stream for checkpoint metadata.
    /// </summary>
    /// <remarks>This stream always creates a file, regardless of the amount of data written.</remarks>
    public abstract class CheckpointMetadataOutputStream : FSDataOutputStream
    {
        /// <summary>
        /// This method should close the stream, if has not been closed before.
        /// If this method actually closes the stream, it should delete/release the resource behind the stream, such as the file that the stream writes to.
        /// </summary>
        public override abstract void Close();
    }
}
