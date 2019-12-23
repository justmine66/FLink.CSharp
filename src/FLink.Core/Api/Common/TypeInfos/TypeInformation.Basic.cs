using System;
using System.Linq;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// Type information for primitive types (int, long, double, byte, ...), String, Date, Void, BigInteger, and BigDecimal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicTypeInfo<T> : TypeInformation<T>, IAtomicType<T>, IEquatable<BasicTypeInfo<T>>
    {
        public override bool IsBasicType => true;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }
        public override bool IsKeyType => true;

        private readonly Type[] _possibleCastTargetTypes;
        private readonly TypeSerializer<T> _serializer;
        private readonly TypeComparator<T> _comparator;

        public BasicTypeInfo(
            Type clazz,
            Type[] possibleCastTargetTypes,
            TypeSerializer<T> serializer,
            TypeComparator<T> comparatorClass)
        {
            TypeClass = Preconditions.CheckNotNull(clazz);
            _possibleCastTargetTypes = Preconditions.CheckNotNull(possibleCastTargetTypes);
            _serializer = Preconditions.CheckNotNull(serializer);
            _comparator = comparatorClass;
        }

        public bool ShouldAutoCastTo<TTo>(BasicTypeInfo<TTo> to)
        {
            foreach (var from in _possibleCastTargetTypes)
                return @from == to.TypeClass;

            return false;
        }

        public override TypeSerializer<T> CreateSerializer(ExecutionConfig config) => _serializer;

        public override string ToString() => TypeClass.Name;

        public virtual TypeComparator<T> CreateComparator(bool sortOrderAscending, ExecutionConfig executionConfig) =>
            _comparator;

        public override bool Equals(object obj) => obj is BasicTypeInfo<T> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_possibleCastTargetTypes != null ? HashCodeHelper.GetHashCode(_possibleCastTargetTypes) : 0);
                hashCode = (hashCode * 397) ^ (_serializer != null ? _serializer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_comparator != null ? _comparator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeClass != null ? TypeClass.GetHashCode() : 0);

                return hashCode;
            }
        }

        public bool Equals(BasicTypeInfo<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(_serializer, other._serializer) &&
                   Equals(_comparator, other._comparator) &&
                   Equals(TypeClass, other.TypeClass) &&
                   _possibleCastTargetTypes.SequenceEqual(other._possibleCastTargetTypes);
        }
    }

    public class BasicTypeInfo
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

        public static readonly BasicTypeInfo<decimal> DecimalTypeInfo = new BasicTypeInfo<decimal>(typeof(decimal), new Type[] { }, DecimalSerializer.Instance, DecimalComparator.Instance);
    }
}
