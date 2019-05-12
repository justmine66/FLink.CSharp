using System;

namespace FLink.Core.FS
{
    public abstract class OutputStream
    {
        /// <summary>
        /// Flushes this output stream and forces any buffered outbytes to be written out file.
        /// </summary>
        public abstract void Flush();

        /// <summary>
        /// Closes this output stream and releases any stream resource associated with this stream.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Writes bytes to this output stream.
        /// </summary>
        /// <param name="bytes">to be written</param>
        public abstract void Write(byte[] bytes);

        /// <summary>
        ///  Writes len bytes from the sepified to this output stream.
        /// </summary>
        /// <param name="bytes"></param>
        public abstract void Write(ArraySegment<byte> bytes);
    }
}
