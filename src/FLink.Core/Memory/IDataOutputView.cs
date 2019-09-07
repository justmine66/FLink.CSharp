namespace FLink.Core.Memory
{
    /// <summary>
    /// This interface defines a view over some memory that can be used to sequentially write contents to the memory.
    /// The view is typically backed by one or more <see cref="MemorySegment"/>.
    /// </summary>
    public interface IDataOutputView
    {
        void SkipBytesToWrite(int numBytes);
        void Write(IDataOutputView source, int numBytes);
    }
}
