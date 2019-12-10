namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// This class gives access to the type information of the most common types for which Flink has built-in serializers and comparators.
    /// </summary>
    public static class Types
    {
        /// <summary>
        /// Returns type information for <see cref="string"/>. Supports a null value.
        /// </summary>
        public static readonly TypeInformation<string> String = BasicTypeInfo.StringTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="byte"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<byte> Byte = BasicTypeInfo.ByteTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="bool"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<bool> Bool = BasicTypeInfo.BoolTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="short"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<short> Short = BasicTypeInfo.ShortTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <code>int</code> and <see cref="int"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<int> Int = BasicTypeInfo.IntTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="long"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<long> Long = BasicTypeInfo.LongTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="float"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<float> Float = BasicTypeInfo.FloatTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="double"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<double> Double = BasicTypeInfo.DoubleTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="char"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<char> Char = BasicTypeInfo.CharTypeInfo;

        /// <summary>
        /// Returns type information for both a primitive <see cref="decimal"/>. Does not support a null value.
        /// </summary>
        public static readonly TypeInformation<decimal> Decimal = BasicTypeInfo.DecimalTypeInfo;

        public static TypeInformation<T> Poco<T>()
        {
            return null;
        }
    }
}
