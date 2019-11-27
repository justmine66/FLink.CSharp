using System;
using FLink.Core.Memory;

namespace FLink.Core.Types
{
    /// <summary>
    /// The base interface for normalizable keys. Normalizable keys can create a binary representation of themselves that is byte-wise comparable. The byte-wise comparison of two normalized keys proceeds until all bytes are compared or two bytes at the corresponding positions are not equal. If two corresponding byte values are not equal, the lower byte value indicates the lower key. If both normalized keys are byte-wise identical, the actual key may have to be looked at to determine which one is actually lower.
    /// The latter depends on whether the normalized key covers the entire key or is just a prefix of the key. A normalized key is considered a prefix, if its length is less than the maximal normalized key length.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INormalizableKey<in T> : IComparable<T>
    {
        /// <summary>
        /// Gets the maximal length of normalized keys that the data type would produce to determine the order of instances solely by the normalized key.
        /// A value of <see cref="int.MaxValue"/> is interpreted as infinite. 
        /// </summary>
        int MaxNormalizedKeyLength { get; }

        /// <summary>
        /// Writes a normalized key for the given record into the target byte array, starting at the specified position an writing exactly the given number of bytes. 
        /// </summary>
        /// <param name="memory">The memory segment to put the normalized key bytes into.</param>
        /// <param name="offset">The offset in the byte array where the normalized key's bytes should start.</param>
        /// <param name="length">The number of bytes to put.</param>
        void CopyNormalizedKey(MemorySegment memory, int offset, int length);
    }
}
