using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.Cache
{
    /// <summary>
    /// DistributedCache provides static methods to write the registered cache files into job configuration or decode them from job configuration. It also provides user access to the file locally.
    /// </summary>
    public class DistributedCache
    {
        private readonly IDictionary<string, TaskCompletionSource<string>> _cacheCopyTasks;

        public DistributedCache(IDictionary<string, TaskCompletionSource<string>> cacheCopyTasks)
        {
            _cacheCopyTasks = cacheCopyTasks;
        }
    }

    /// <summary>
    /// Meta info about an entry in <see cref="DistributedCache"/>.
    /// </summary>
    public class DistributedCacheEntry : IEquatable<DistributedCacheEntry>
    {
        /// <summary>
        /// Client-side constructor used by the API for initial registration.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isExecutable"></param>
        public DistributedCacheEntry(string filePath, bool isExecutable)
            : this(filePath, isExecutable, null, false)
        { }

        /// <summary>
        /// Client-side constructor used during job-submission for zipped directory.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isExecutable"></param>
        /// <param name="isZipped"></param>
        public DistributedCacheEntry(string filePath, bool isExecutable, bool isZipped)
            : this(filePath, isExecutable, null, isZipped)
        { }

        /// <summary>
        /// Server-side constructor used during job-submission for zipped directories.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isExecutable"></param>
        /// <param name="blobKey"></param>
        /// <param name="isZipped"></param>
        public DistributedCacheEntry(string filePath, bool isExecutable, byte[] blobKey, bool isZipped)
        {
            FilePath = filePath;
            IsExecutable = isExecutable;
            BlobKey = blobKey;
            IsZipped = isZipped;
        }

        public string FilePath { get; }
        public bool IsExecutable { get; }
        public bool IsZipped { get; }

        public byte[] BlobKey { get; }

        public bool Equals(DistributedCacheEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(FilePath, other.FilePath) && IsExecutable == other.IsExecutable && IsZipped == other.IsZipped && BlobKey.SequenceEqual(other.BlobKey);
        }

        public override bool Equals(object obj) => obj is DistributedCacheEntry other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (FilePath != null ? FilePath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsExecutable.GetHashCode();
                hashCode = (hashCode * 397) ^ IsZipped.GetHashCode();
                hashCode = (hashCode * 397) ^ (BlobKey != null ? HashCodeHelper.GetHashCode(BlobKey) : 0);
                return hashCode;
            }
        }
    }
}
