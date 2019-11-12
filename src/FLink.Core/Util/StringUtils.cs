using System;
using System.Text;
using FLink.Core.Exceptions;

namespace FLink.Core.Util
{
    /// <summary>
    /// Utility class to convert objects into strings in vice-versa.
    /// </summary>
    public sealed class StringUtils
    {
        private static readonly char[] HexChars =
            {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};

        /// <summary>
        /// Given an array of bytes it will convert the bytes to a hex string representation of the bytes.
        /// </summary>
        /// <param name="bytes">the bytes to convert in a hex string</param>
        /// <param name="start">start index, inclusively</param>
        /// <param name="end">end index, exclusively</param>
        /// <returns>hex string representation of the byte array</returns>
        public static string ByteToHexString(byte[] bytes, int start, int end)
        {
            if (bytes == null)
            {
                throw new IllegalArgumentException("bytes == null");
            }

            var length = end - start;
            var output = new char[length * 2];

            for (int i = start, j = 0; i < end; i++)
            {
                output[j++] = HexChars[(0xF0 & bytes[i]) >> 4];
                output[j++] = HexChars[0x0F & bytes[i]];
            }

            return string.Create(output.Length, output, (chars, buf) =>
            {
                for (var i = 0; i < chars.Length; i++) chars[i] = buf[i];
            });
        }

        /// <summary>
        /// Given an array of bytes it will convert the bytes to a hex string representation of the bytes.
        /// </summary>
        /// <param name="bytes">the bytes to convert in a hex string</param>
        /// <returns>hex string representation of the byte array</returns>
        public static string ByteToHexString(byte[] bytes) => ByteToHexString(bytes, 0, bytes.Length);

        public static byte[] HexStringToByte(string hex)
        {
            var bts = new byte[hex.Length / 2];
            for (var i = 0; i < bts.Length; i++)
            {
                bts[i] = (byte) int.Parse(hex.Substring(2 * i, 2 * i + 2));
            }

            return bts;
        }

        /// <summary>
        /// Creates a random string with a length within the given interval. The string contains only characters that can be represented as a single code point.
        /// </summary>
        /// <param name="rnd">The random used to create the strings.</param>
        /// <param name="minLength">The minimum string length.</param>
        /// <param name="maxLength">The maximum string length (inclusive).</param>
        /// <returns>A random String.</returns>
        public static string GetRandomString(Random rnd, int minLength, int maxLength)
        {
            var len = rnd.Next(maxLength - minLength + 1) + minLength;

            var data = new char[len];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (char) (rnd.Next(0x7fff) + 1);
            }

            return string.Create(data.Length, data, (chars, buf) =>
            {
                for (var i = 0; i < chars.Length; i++) chars[i] = buf[i];
            });
        }

        /// <summary>
        /// Creates a random string with a length within the given interval. The string contains only characters that can be represented as a single code point.
        /// </summary>
        /// <param name="rnd">The random used to create the strings.</param>
        /// <param name="minLength">The minimum string length.</param>
        /// <param name="maxLength">The maximum string length (inclusive).</param>
        /// <param name="minValue">The minimum character value to occur.</param>
        /// <param name="maxValue">The maximum character value to occur.</param>
        /// <returns>A random String.</returns>
        public static string GetRandomString(Random rnd, int minLength, int maxLength, char minValue, char maxValue)
        {
            var len = rnd.Next(maxLength - minLength + 1) + minLength;

            var data = new char[len];
            var diff = maxValue - minValue + 1;

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (char) (rnd.Next(diff) + minValue);
            }

            return string.Create(data.Length, data, (chars, buf) =>
            {
                for (var i = 0; i < chars.Length; i++) chars[i] = buf[i];
            });
        }

        /// <summary>
        /// Creates a random alphanumeric string of given length.
        /// </summary>
        /// <param name="rnd">The random number generator to use.</param>
        /// <param name="length">The number of alphanumeric characters to append.</param>
        /// <returns></returns>
        public static string GetRandomAlphanumericString(Random rnd, int length)
        {
            Preconditions.CheckNotNull(rnd);
            Preconditions.CheckArgument(length >= 0);

            var buffer = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                buffer.Append(NextAlphanumericChar(rnd));
            }
            return buffer.ToString();
        }

        private static char NextAlphanumericChar(Random rnd)
        {
            var which = rnd.Next(62);
            char c;
            if (which < 10)
            {
                c = (char)('0' + which);
            }
            else if (which < 36)
            {
                c = (char)('A' - 10 + which);
            }
            else
            {
                c = (char)('a' - 36 + which);
            }
            return c;
        }
    }
}
