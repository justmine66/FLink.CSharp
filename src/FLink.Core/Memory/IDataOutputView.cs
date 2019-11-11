using System.IO;
using FLink.Core.IO;

namespace FLink.Core.Memory
{
    /// <summary>
    /// This interface defines a view over some memory that can be used to sequentially write contents to the memory.
    /// The view is typically backed by one or more <see cref="MemorySegment"/>.
    /// </summary>
    public interface IDataOutputView : IDataOutput
    {
        /// <summary>
        /// Skips <paramref name="numBytes"/> bytes memory. If some program reads the memory that was skipped over, the results are undefined.
        /// </summary>
        /// <param name="numBytes">The number of bytes to skip.</param>
        /// <exception cref="IOException">Thrown, if any I/O related problem occurred such that the view could not be advanced to the desired position.</exception>
        void SkipBytesToWrite(int numBytes);

        /// <summary>
        /// Copies <paramref name="numBytes"/> bytes from the source to this view.
        /// </summary>
        /// <param name="source">The source to copy the bytes from.</param>
        /// <param name="numBytes">The number of bytes to copy.</param>
        /// <exception cref="IOException">Thrown, if any I/O related problem occurred such that the view could not be advanced to the desired position.</exception>
        void Write(IDataInputView source, int numBytes);
    }
}
