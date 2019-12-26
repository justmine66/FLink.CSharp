using System.IO;
using System.Text;

namespace FLink.Core.Util
{
    /// <summary>
    /// Collection of simple mathematical routines.
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Pseudo-randomly maps a long (64-bit) to an integer (32-bit) using some bit-mixing for better distribution.
        /// </summary>
        /// <param name="input">the long (64-bit)input.</param>
        /// <returns>the bit-mixed int (32-bit) output</returns>
        public static int LongToIntWithBitMixing(ulong input)
        {
            input = (input ^ (input >> 30)) * 0xbf58476d1ce4e5b9L;
            input = (input ^ (input >> 27)) * 0x94d049bb133111ebL;
            input = input ^ (input >> 31);
            return (int)input;
        }

        /// <summary>
        /// This function hashes an integer value.
        /// It is crucial to use different hash functions to partition data across machines and the internal partitioning of data structures. This hash function is intended for partitioning across machines.
        /// </summary>
        /// <param name="uniqueString"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static int MurmurHash(string uniqueString, Encoding encoding)
        {
            var input = encoding.GetBytes(uniqueString);
            using var stream = new MemoryStream(input);
            return MurMurHash3.Hash(stream);
        }

        public static int MurmurHash(string uniqueString)
        {
            var input = Encoding.UTF8.GetBytes(uniqueString);
            using var stream = new MemoryStream(input);
            return MurMurHash3.Hash(stream);
        }
    }
}
