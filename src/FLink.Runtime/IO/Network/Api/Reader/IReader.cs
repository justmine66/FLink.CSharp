using FLink.Core.IO;

namespace FLink.Runtime.IO.Network.Api.Reader
{
    /// <summary>
    /// A record-oriented reader for immutable record types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReader<out T> : IReaderBase where T : IIOReadableWritable
    {
        bool HasNext { get; }

        T Next { get; }

        void ClearBuffers();
    }
}
