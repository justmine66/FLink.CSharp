using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Util;
using System;

namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// Type information for primitive types (int, long, double, byte, ...), String, Date, Void, BigInteger, and BigDecimal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicTypeInfo<T> : TypeInformation<T>, IAtomicType<T>
    {
        public static readonly BasicTypeInfo<string> StringTypeInfo = new BasicTypeInfo<string>(typeof(string), new Type[] { }, StringSerializer.Instance, StringComparator.Instance);

        public static readonly BasicTypeInfo<bool> BoolTypeInfo = new BasicTypeInfo<bool>(typeof(bool), new Type[] { }, BoolSerializer.Instance, BoolComparator.Instance);

        public static readonly BasicTypeInfo<byte> ByteTypeInfo = new IntTypeInfo<byte>(typeof(byte), new Type[] { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(char) }, ByteSerializer.Instance, ByteComparator.Instance);

        public static readonly BasicTypeInfo<short> ShortTypeInfo = new IntTypeInfo<short>(typeof(short), new Type[] { typeof(int), typeof(long), typeof(float), typeof(double), typeof(char) }, ShortSerializer.Instance, ShortComparator.Instance);

        public static readonly BasicTypeInfo<int> IntTypeInfo = new IntTypeInfo<int>(typeof(short), new Type[] { typeof(long), typeof(float), typeof(double), typeof(char) }, IntSerializer.Instance, IntComparator.Instance);

        public static readonly BasicTypeInfo<long> LongTypeInfo = new IntTypeInfo<long>(typeof(short), new Type[] { typeof(float), typeof(double), typeof(char) }, LongSerializer.Instance, LongComparator.Instance);

        public static readonly BasicTypeInfo<float> FloatTypeInfo = new FractionalTypeInfo<float>(typeof(short),
            new Type[] { typeof(double) }, FloatSerializer.Instance,
            FloatComparator.Instance);

        public static readonly BasicTypeInfo<double> DoubleTypeInfo = new FractionalTypeInfo<double>(typeof(short),
            new Type[] { }, DoubleSerializer.Instance,
            DoubleComparator.Instance);

        public static readonly BasicTypeInfo<char> CharTypeInfo = new BasicTypeInfo<char>(typeof(short),
            new Type[] { }, CharSerializer.Instance,
            CharComparator.Instance);

        public static readonly BasicTypeInfo<DateTime> DateTimeTypeInfo = new BasicTypeInfo<DateTime>(typeof(short),
            new Type[] { }, DateTimeSerializer.Instance,
            DateTimeComparator.Instance);

        public override bool IsBasicType => true;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }
        public override bool IsKeyType => true;

        private readonly Type[] _possibleCastTargetTypes;
        private readonly TypeSerializer<T> _serializer;
        private readonly TypeComparator<T> _comparatorClass;

        public BasicTypeInfo(
            Type clazz,
            Type[] possibleCastTargetTypes,
            TypeSerializer<T> serializer,
            TypeComparator<T> comparatorClass)
        {
            TypeClass = Preconditions.CheckNotNull(clazz);
            _possibleCastTargetTypes = Preconditions.CheckNotNull(possibleCastTargetTypes);
            _serializer = Preconditions.CheckNotNull(serializer);
            _comparatorClass = comparatorClass;
        }

        public override TypeSerializer<T> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public TypeComparator<T> CreateComparator(bool sortOrderAscending, ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
