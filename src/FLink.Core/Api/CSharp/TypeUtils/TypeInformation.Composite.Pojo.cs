using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    /// <summary>
    /// TypeInformation for "Java Beans"-style types. Flink refers to them as POJOs, since the conditions are slightly different from Java Beans.
    /// A type is considered a FLink POJO type, if it fulfills the conditions below:
    /// 1. It is a public class, and standalone (not a non-static inner class).
    /// 2. It has a public no-argument constructor.
    /// 3. All fields are either public, or have public getters and setters.
    /// </summary>
    /// <typeparam name="T">The type represented by this type information.</typeparam>
    public class PojoTypeInfo<T> : CompositeType<T>, IEquatable<PojoTypeInfo<T>>
    {
        private readonly PojoField[] _fields;

        public PojoTypeInfo(Type type, IList<PojoField> fields)
            : base(type)
        {
            Preconditions.CheckArgument(type.IsPublic, $"POJO {type.Name} is not public");

            _fields = fields.OrderBy(it => it.Field.Name).ToArray();
            TotalFields = fields.Sum(field => field.Type.TotalFields);
        }

        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => _fields.Length;
        public override int TotalFields { get; }

        /// <summary>
        /// Support for sorting POJOs that implement Comparable is not implemented yet. Since the order of fields in a POJO type is not well defined, sorting on fields gives only some undefined order.
        /// </summary>
        public override bool IsSortKeyType => false;

        public override TypeSerializer<T> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        public override int GetFieldIndex(string fieldName)
        {
            for (var i = 0; i < _fields.Length; i++)
            {
                if (_fields[i].Field.Name.Equals(fieldName))
                {
                    return i;
                }
            }

            return -1;
        }

        public override void GetFlatFields(string fieldExpression, int offset, IList<FlatFieldDescriptor> result)
        {
            throw new NotImplementedException();
        }

        public override TypeInformation<TX> GetTypeAt<TX>(string fieldExpression)
        {
            throw new NotImplementedException();
        }

        public PojoField GetPojoFieldAt(int pos)
        {
            if (pos < 0 || pos >= _fields.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return _fields[pos];
        }

        public override TypeInformation<TX> GetTypeAt<TX>(int pos)
        {
            throw new NotImplementedException();
        }

        protected override ITypeComparatorBuilder<T> CreateTypeComparatorBuilder() => new PojoTypeComparatorBuilder();

        public override string[] FieldNames => _fields.Select(it => it.Field.Name).ToArray();

        public override bool Equals(object obj) => obj is PojoTypeInfo<T> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (_fields != null ? HashCodeHelper.GetHashCode(_fields) : 0);
                hashCode = (hashCode * 397) ^ TotalFields;
                return hashCode;
            }
        }

        public bool Equals(PojoTypeInfo<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return base.Equals(other) && _fields.SequenceEqual(other._fields) && TotalFields == other.TotalFields;
        }

        public override string ToString()
        {
            var fieldStrings = _fields.Select(field => field.Field.Name + ": " + field.Type);
            var flatFieldStrings = fieldStrings.Aggregate((x, y) => $"{x},{y}");

            return $"PojoType<{TypeClass.Name}, fields = [{flatFieldStrings}]>";
        }

        public class PojoTypeComparatorBuilder : ITypeComparatorBuilder<T>
        {
            private readonly IList<TypeComparator<T>> _fieldComparators;
            private readonly IList<FieldInfo> _keyFields;

            public PojoTypeComparatorBuilder()
            {
                _fieldComparators = new List<TypeComparator<T>>();
                _keyFields = new List<FieldInfo>();
            }

            public void InitializeTypeComparatorBuilder(int size)
            {
                throw new NotImplementedException();
            }

            public void AddComparatorField<TType>(int fieldId, TypeComparator<TType> comparator)
            {
                throw new NotImplementedException();
            }

            public TypeComparator<T> CreateTypeComparator(ExecutionConfig config)
            {
                throw new NotImplementedException();
            }
        }
    }
}
