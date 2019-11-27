using System;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.TypeInfo
{
    public class PrimitiveArrayTypeInfo<T> : TypeInformation<T>, IAtomicType<T>
    {
        public override bool IsBasicType { get; }
        public override bool IsTupleType { get; }
        public override int Arity { get; }
        public override int TotalFields { get; }
        public override Type TypeClass { get; }
        public override bool IsKeyType { get; }

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
