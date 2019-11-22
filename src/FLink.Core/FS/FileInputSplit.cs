using System;
using FLink.Core.IO;

namespace FLink.Core.FS
{
    /// <summary>
    /// A file input split provides information on a particular part of a file, possibly hosted on a distributed file system and replicated among several hosts.
    /// </summary>
    public class FileInputSplit : LocatableInputSplit, IEquatable<FileInputSplit>
    {
        /// <summary>
        /// The path of the file this file split refers to.
        /// </summary>
        public string File { get; }

        /// <summary>
        /// The position of the first byte in the file to process.
        /// </summary>
        public long Start;

        /// <summary>
        /// The number of bytes in the file to process.
        /// </summary>
        public long Length;

        /// <summary>
        /// Constructs a split with host information.
        /// </summary>
        /// <param name="splitNumber">the number of this input split</param>
        /// <param name="file">the file name</param>
        /// <param name="start">the position of the first byte in the file to process</param>
        /// <param name="length">the number of bytes in the file to process (-1 is flag for "read whole file")</param>
        /// <param name="hostnames">the list of hosts containing the block, possibly <code>null</code></param>
        public FileInputSplit(int splitNumber, string file, long start, long length, params string[] hostnames)
            : base(splitNumber, hostnames)
        {
            File = file;
            Start = start;
            Length = length;
        }

        public bool Equals(FileInputSplit other) => other != null && Start == other.Start && Length == other.Length && File == other.File;

        public override bool Equals(object obj) => obj is FileInputSplit other && Equals(other);

        public override int GetHashCode() => SplitNumber ^ (File == null ? 0 : File.GetHashCode());
    }
}
