using System;
using FLink.Core.Exceptions;

namespace FLink.Core.Util
{
    /// <summary>
    /// A statistically unique identification number.
    /// </summary>
    public class AbstractId : IComparable<AbstractId>, IEquatable<AbstractId>
    {
        private static readonly Random Rnd = new Random();

        /// <summary>
        /// The size of a long in bytes. 
        /// </summary>
        private static readonly int SizeOfLong = sizeof(long);

        /// <summary>
        /// The size of the ID in byte.
        /// </summary>
        public static readonly int Size = 2 * SizeOfLong;

        /// <summary>
        /// Gets the upper 64 bits of the ID.
        /// </summary>
        public long UpperPart;

        /// <summary>
        /// Gets the lower 64 bits of the ID.
        /// </summary>
        public long LowerPart;

        /// <summary>
        /// Constructs a new ID with a specific bytes value.
        /// </summary>
        /// <param name="bytes"></param>
        public AbstractId(byte[] bytes)
        {
            if (bytes == null || bytes.Length != Size)
            {
                throw new IllegalArgumentException("Argument bytes must by an array of " + Size + " bytes");
            }

            LowerPart = ByteArrayToLong(bytes, 0);
            UpperPart = ByteArrayToLong(bytes, SizeOfLong);
        }

        /// <summary>
        /// Constructs a new abstract ID.
        /// </summary>
        /// <param name="lowerPart">the lower bytes of the ID</param>
        /// <param name="upperPart">the higher bytes of the ID</param>
        public AbstractId(long lowerPart, long upperPart)
        {
            LowerPart = lowerPart;
            UpperPart = upperPart;
        }

        /// <summary>
        /// Copy constructor: Creates a new abstract ID from the given one.
        /// </summary>
        /// <param name="id">the abstract ID to copy</param>
        public AbstractId(AbstractId id)
        {
            if (id == null)
            {
                throw new IllegalArgumentException("Id must not be null.");
            }
            LowerPart = id.LowerPart;
            UpperPart = id.UpperPart;
        }

        public AbstractId()
        {
            LowerPart = Rnd.Next();
            UpperPart = Rnd.Next();
        }

        public byte[] GetBytes()
        {
            var bytes = new byte[Size];
            LongToByteArray(LowerPart, bytes, 0);
            LongToByteArray(UpperPart, bytes, SizeOfLong);
            return bytes;
        }

        public string ToHexString()
        {
            var ba = new byte[Size];
            LongToByteArray(LowerPart, ba, 0);
            LongToByteArray(UpperPart, ba, SizeOfLong);

            return StringUtils.ByteToHexString(ba);
        }

        public int CompareTo(AbstractId other)
        {
            var diff1 = UpperPart - other.UpperPart;
            var diff2 = LowerPart - other.LowerPart;
            return diff1 == 0 ? (int)diff2 : (int)diff1;
        }

        #region [ Conversion Utilities ]

        /// <summary>
        /// Converts the given byte array to a long.
        /// </summary>
        /// <param name="bytes">the byte array to be converted</param>
        /// <param name="offset">the offset indicating at which byte inside the array the conversion shall begin</param>
        /// <returns>the long variable</returns>
        private static long ByteArrayToLong(byte[] bytes, int offset)
        {
            var l = 0L;

            for (var i = 0; i < SizeOfLong; ++i)
            {
                l |= (bytes[offset + SizeOfLong - 1 - i] & 0xffL) << (i << 3);
            }

            return l;
        }

        /// <summary>
        /// Converts a long to a byte array.
        /// </summary>
        /// <param name="l">the long variable to be converted</param>
        /// <param name="bytes">the byte array to store the result the of the conversion</param>
        /// <param name="offset">offset indicating at what position inside the byte array the result of the conversion shall be stored</param>
        private static void LongToByteArray(long l, byte[] bytes, int offset)
        {
            for (var i = 0; i < SizeOfLong; ++i)
            {
                var shift = i << 3; // i * 8
                bytes[offset + SizeOfLong - 1 - i] = (byte)((l & (0xffL << shift)) >> shift);
            }
        }

        #endregion

        public bool Equals(AbstractId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return UpperPart == other.UpperPart && LowerPart == other.LowerPart;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AbstractId)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = UpperPart.GetHashCode();
                hashCode = (hashCode * 397) ^ LowerPart.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString() => ToHexString();
    }
}
