using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class PojoTypeInfo<T> : CompositeType<T>
    {
        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity { get; }
        public override int TotalFields { get; }
        public override Type TypeClass { get; }

        public override TypeSerializer<T> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
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
