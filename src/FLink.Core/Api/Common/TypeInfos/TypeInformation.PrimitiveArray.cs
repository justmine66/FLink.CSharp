using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Api.Common.TypeUtils.Base.Array;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using System;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// A <see cref="TypeInformation{TType}"/> for arrays of primitive types (int, long, double, ...).
    /// Supports the creation of dedicated efficient serializers for these types.
    /// </summary>
    /// <typeparam name="TElement">The type represented by this type information, e.g., int[], double[], long[]</typeparam>
    public class PrimitiveArrayTypeInfo<TElement> :
        TypeInformation<TElement[]>,
        IAtomicType<TElement[]>,
        IEquatable<PrimitiveArrayTypeInfo<TElement>>
    {
        public PrimitiveArrayTypeInfo(
            Type arrayClass,
            TypeSerializer<TElement[]> serializer,
            PrimitiveArrayComparator<TElement, BasicTypeComparator<TElement>> comparatorClass)
        {
            TypeClass = Preconditions.CheckNotNull(arrayClass);
            Serializer = Preconditions.CheckNotNull(serializer);
            ComparatorClass = Preconditions.CheckNotNull(comparatorClass);

            Preconditions.CheckArgument(arrayClass.IsArray && arrayClass.GetElementType().IsPrimitive, "Class must represent an array of primitives");
        }

        public TypeSerializer<TElement[]> Serializer { get; }
        public PrimitiveArrayComparator<TElement, BasicTypeComparator<TElement>> ComparatorClass { get; }
        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        /// <summary>
        /// The class of the array (such as int[].class). 
        /// </summary>
        public override Type TypeClass { get; }
        public override bool IsKeyType => true;

        public override TypeSerializer<TElement[]> CreateSerializer(ExecutionConfig config) => Serializer;

        public override string ToString() => typeof(TElement).Name + "[]";

        public TypeComparator<TElement[]> CreateComparator(bool sortOrderAscending, ExecutionConfig executionConfig) => ComparatorClass;

        public override bool Equals(object obj) => obj is PrimitiveArrayTypeInfo<TElement> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Serializer != null ? Serializer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ComparatorClass != null ? ComparatorClass.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeClass != null ? TypeClass.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(PrimitiveArrayTypeInfo<TElement> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(Serializer, other.Serializer) && Equals(ComparatorClass, other.ComparatorClass) && TypeClass == other.TypeClass;
        }
    }

    public class PrimitiveArrayTypeInfo
    {
        public static readonly PrimitiveArrayTypeInfo<bool> BoolPrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<bool>(
                typeof(bool),
                BoolPrimitiveArraySerializer.Instance,
                BoolPrimitiveArrayComparator.Instance);
        public static readonly PrimitiveArrayTypeInfo<char> CharPrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<char>(
                typeof(char),
                CharPrimitiveArraySerializer.Instance,
                CharPrimitiveArrayComparator.Instance);
        public static readonly PrimitiveArrayTypeInfo<int> IntPrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<int>(
                typeof(int),
                IntPrimitiveArraySerializer.Instance,
                IntPrimitiveArrayComparator.Instance);
        public static readonly PrimitiveArrayTypeInfo<long> LongPrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<long>(
                typeof(bool),
                LongPrimitiveArraySerializer.Instance,
                LongPrimitiveArrayComparator.Instance);
        public static readonly PrimitiveArrayTypeInfo<short> ShortPrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<short>(
                typeof(bool),
                ShortPrimitiveArraySerializer.Instance,
                ShortPrimitiveArrayComparator.Instance);
        public static readonly PrimitiveArrayTypeInfo<float> FloatPrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<float>(
                typeof(float),
                FloatPrimitiveArraySerializer.Instance,
                FloatPrimitiveArrayComparator.Instance);
        public static readonly PrimitiveArrayTypeInfo<double> DoublePrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<double>(
                typeof(double),
                DoublePrimitiveArraySerializer.Instance,
                DoublePrimitiveArrayComparator.Instance);
        public static readonly PrimitiveArrayTypeInfo<byte> BytePrimitiveArrayTypeInfo =
            new PrimitiveArrayTypeInfo<byte>(
                typeof(byte),
                BytePrimitiveArraySerializer.Instance,
                BytePrimitiveArrayComparator.Instance);

        /// <summary>
        /// Tries to get the PrimitiveArrayTypeInfo for an array. Returns null, if the type is an array, but the component type is not a primitive type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The class of the array.</param>
        /// <returns>The corresponding PrimitiveArrayTypeInfo, or null, if the array is not an array of primitives.</returns>
        /// <exception cref="InvalidTypesException">Thrown, if the given class does not represent an array.</exception>
        public static dynamic GetInfoFor(Type type)
        {
            if (!type.IsArray)
            {
                throw new InvalidTypesException("The given class is no array.");
            }

            // basic type arrays
            if (type == typeof(bool[]))
            {
                return BoolPrimitiveArrayTypeInfo;
            }

            if (type == typeof(int[]))
            {
                return IntPrimitiveArrayTypeInfo;
            }

            if (type == typeof(long[]))
            {
                return LongPrimitiveArrayTypeInfo;
            }

            if (type == typeof(short[]))
            {
                return ShortPrimitiveArrayTypeInfo;
            }

            if (type == typeof(char[]))
            {
                return CharPrimitiveArrayTypeInfo;
            }

            if (type == typeof(float[]))
            {
                return FloatPrimitiveArrayTypeInfo;
            }

            if (type == typeof(double[]))
            {
                return DoublePrimitiveArrayTypeInfo;
            }

            if (type == typeof(byte[]))
            {
                return BytePrimitiveArrayTypeInfo;
            }

            return null;
        }
    }
}
