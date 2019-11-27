using System;
using FLink.Core.Exceptions;
using FLink.Core.Memory;
using FLink.Core.Util;

namespace FLink.Core.Types
{
    public class StringValue : INormalizableKey<StringValue>, IResettableValue<StringValue>, ICopyableValue<StringValue>
    {
        private static readonly char[] EmptyString = new char[0];

        private const int HighBit = 0x1 << 7;

        private const int HighBit2 = 0x1 << 13;

        private const int HighBit2Mask = 0x3 << 6;

        /// <summary>
        /// length of the string value
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// cache for the hashCode
        /// </summary>
        public int HashCode { get; private set; }

        #region [ Constructors ]

        /// <summary>
        /// Initializes the encapsulated String object with an empty string.	
        /// </summary>
        public StringValue()
        {
            CharArray = EmptyString;
        }

        /// <summary>
        /// Initializes this StringValue to a copy the given StringValue.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public StringValue(StringValue value)
        {
            CharArray = EmptyString;
            SetValue(value);
        }

        /// <summary>
        /// Initializes the StringValue to a sub-string of the given StringValue. 
        /// </summary>
        /// <param name="value">The string containing the substring.</param>
        /// <param name="offset">The offset of the substring.</param>
        /// <param name="length">The length of the substring.</param>
        public StringValue(StringValue value, int offset, int length)
        {
            CharArray = EmptyString;
            SetValue(value.CharArray, offset, length);
        }

        #endregion

        /// <summary>
        /// Sets a new length for the string.
        /// </summary>
        /// <param name="len">The new length.</param>
        public void SetLength(int len)
        {
            if (len < 0 || len > Length)
            {
                throw new IllegalArgumentException("Length must be between 0 and the current length.");
            }

            Length = len;
        }

        /// <summary>
        /// Sets the value of the StringValue to a substring of the given value.
        /// </summary>
        /// <param name="chars">The new string value (as a character array).</param>
        /// <param name="offset">The position to start the substring.</param>
        /// <param name="length">The length of the substring.</param>
        public void SetValue(char[] chars, int offset, int length)
        {
            Preconditions.CheckNotNull(chars);
            if (offset < 0 || length < 0 || offset > chars.Length - length)
            {
                throw new IndexOutOfRangeException();
            }

            EnsureSize(length);
            Array.Copy(chars, offset, CharArray, 0, length);

            Length = length;
            HashCode = 0;
        }

        /// <summary>
        /// Returns this StringValue's internal character data. The array might be larger than the string which is currently stored in the StringValue.
        /// </summary>
        public char[] CharArray { get; private set; }

        public virtual int CompareTo(StringValue other)
        {
            var len1 = Length;
            var len2 = other.Length;
            var n = Math.Min(len1, len2);
            var v1 = CharArray;
            var v2 = other.CharArray;

            for (var k = 0; k < n; k++)
            {
                var c1 = v1[k];
                var c2 = v2[k];
                if (c1 != c2)
                {
                    return c1 - c2;
                }
            }

            return len1 - len2;
        }

        public int MaxNormalizedKeyLength => int.MaxValue;

        public void CopyNormalizedKey(MemorySegment memory, int offset, int length)
        {
            throw new System.NotImplementedException();
        }

        public void SetValue(StringValue value)
        {
            Preconditions.CheckNotNull(value);
            SetValue(value.CharArray, 0, value.Length);
        }

        public void Write(IDataOutputView output)
        {
            throw new System.NotImplementedException();
        }

        public void Read(IDataInputView input)
        {
            throw new System.NotImplementedException();
        }

        public int BinaryLength => -1;

        public void CopyTo(StringValue target)
        {
            target.Length = Length;
            target.HashCode = HashCode;
            target.EnsureSize(Length);
            Array.Copy(CharArray, 0, target.CharArray, 0, Length);
        }

        public StringValue Copy() => new StringValue(this);

        public void Copy(IDataInputView source, IDataOutputView target)
        {
            var len = source.ReadUnsignedByte();
            target.WriteByte(len);

            if (len >= HighBit)
            {
                var shift = 7;
                int curr;
                len = len & 0x7f;
                while ((curr = source.ReadUnsignedByte()) >= HighBit)
                {
                    len |= (curr & 0x7f) << shift;
                    shift += 7;
                    target.WriteByte(curr);
                }
                len |= curr << shift;
                target.WriteByte(curr);
            }

            for (var i = 0; i < len; i++)
            {
                var c = source.ReadUnsignedByte();
                target.WriteByte(c);
                while (c >= HighBit)
                {
                    c = source.ReadUnsignedByte();
                    target.WriteByte(c);
                }
            }
        }

        public override string ToString() => StringUtils.Create(CharArray);

        #region [ Utilities ]

        private void EnsureSize(int size)
        {
            if (CharArray.Length < size)
            {
                CharArray = new char[size];
            }
        }

        #endregion
    }
}
