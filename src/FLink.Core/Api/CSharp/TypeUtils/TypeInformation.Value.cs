using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Types;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class ValueTypeInfo<T>: TypeInformation<T>, IAtomicType<T> where T : IValue
    {
        public ValueTypeInfo(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass => typeof(T);
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
