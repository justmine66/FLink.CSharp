using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class GenericTypeInfo<T> : TypeInformation<T>, IAtomicType<T>, IEquatable<GenericTypeInfo<T>>
    {
        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }
        public override bool IsKeyType => typeof(IComparable<T>).IsAssignableFrom(TypeClass);

        public GenericTypeInfo(Type typeClass) => TypeClass = Preconditions.CheckNotNull(typeClass);

        public override TypeSerializer<T> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        public override string ToString() => "GenericType<" + TypeClass + ">";

        public TypeComparator<T> CreateComparator(bool sortOrderAscending, ExecutionConfig executionConfig)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj) => obj is GenericTypeInfo<T> other && Equals(other);

        public override int GetHashCode()
        {
            return (TypeClass != null ? TypeClass.GetHashCode() : 0);
        }

        public bool Equals(GenericTypeInfo<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(TypeClass, other.TypeClass);
        }
    }
}
