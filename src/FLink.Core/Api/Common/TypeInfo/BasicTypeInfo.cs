using System;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// Type information for primitive types (int, long, double, byte, ...), String, Date, Void, BigInteger, and BigDecimal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicTypeInfo<T> : TypeInformation<T>, IAtomicType<T>
    {
        public static readonly BasicTypeInfo<string> StringTypeInfo = new BasicTypeInfo<string>(typeof(string), new Type[]{}, StringSerializer.Instance, StringComparator.Instance);

        public override bool IsBasicType => true;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }

        private readonly Type _clazz;
        private readonly Type[] _possibleCastTargetTypes;
        private readonly TypeSerializer<T> _serializer;
        private readonly TypeComparator<T> _comparatorClass;

        protected BasicTypeInfo(Type clazz, Type[] possibleCastTargetTypes, TypeSerializer<T> serializer, TypeComparator<T> comparatorClass)
        {
            _clazz = Preconditions.CheckNotNull(clazz);
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
