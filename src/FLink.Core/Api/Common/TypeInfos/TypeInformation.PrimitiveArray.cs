using System;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Api.Common.TypeUtils.Base.Array;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// A <see cref="TypeInformation{TType}"/> for arrays of primitive types (int, long, double, ...).
    /// Supports the creation of dedicated efficient serializers for these types.
    /// </summary>
    /// <typeparam name="TElement">The type represented by this type information, e.g., int[], double[], long[]</typeparam>
    public class PrimitiveArrayTypeInfo<TElement> :
        TypeInformation<TElement[]>, IAtomicType<TElement[]>,
        IEquatable<PrimitiveArrayTypeInfo<TElement>>
    {
        private PrimitiveArrayTypeInfo(Type arrayClass, TypeSerializer<TElement[]> serializer, PrimitiveArrayComparator<TElement, BasicTypeComparator<TElement>> comparatorClass)
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
}
