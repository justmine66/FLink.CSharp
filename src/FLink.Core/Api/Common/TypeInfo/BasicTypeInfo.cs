using System;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// Type information for primitive types (int, long, double, byte, ...), String, Date, Void, BigInteger, and BigDecimal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicTypeInfo<T> : TypeInformation<T>, IAtomicType<T>
    {
        public override bool IsBasicType => true;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }

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
