using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FLink.Core.Api.Common.Operators;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public abstract class TupleTypeInfoBase<T> : CompositeType<T>, IEquatable<TupleTypeInfoBase<T>>
    {
        private static readonly string RegexField = "(f?)([0-9]+)";
        private static readonly string RegexNestedFields = "(" + RegexField + ")(\\.(.+))?";
        private static readonly string RegexNestedFieldsWildcard =
            $"{RegexNestedFields}|\\{Keys<T>.ExpressionKeys<T>.SelectAllChar}";

        protected TupleTypeInfoBase(Type typeClass, params TypeInformation<object>[] types)
            : base(typeClass)
        {
            Types = types;
            TotalFields = types.Sum(type => type.TotalFields);
        }

        public TypeInformation<object>[] Types { get; }

        public override bool IsBasicType => false;

        public override bool IsTupleType => true;

        public override int TotalFields { get; }

        public override int Arity => Types.Length;

        public override void GetFlatFields(string fieldExpression, int offset, IList<FlatFieldDescriptor> result)
        {
            throw new NotImplementedException();
        }

        public override TypeInformation<TX> GetTypeAt<TX>(int pos)
        {
            if (pos < 0 || pos >= Types.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return Types[pos] as TypeInformation<TX>;
        }

        public override TypeInformation<TX> GetTypeAt<TX>(string fieldExpression)
        {
            throw new NotImplementedException();
        }

        public bool Equals(TupleTypeInfoBase<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return TotalFields == other.TotalFields && Types.SequenceEqual(other.Types);
        }

        public override bool Equals(object obj) => obj is TupleTypeInfoBase<T> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ HashCodeHelper.GetHashCode(Types);
                hashCode = (hashCode * 397) ^ TotalFields;
                return hashCode;
            }
        }

        public override string ToString()
        {
            var length = Types.Length;
            var bld = new StringBuilder("Tuple");

            bld.Append(length);

            if (length <= 0) return bld.ToString();

            bld.Append('<').Append(Types[0]);

            for (var i = 1; i < length; i++)
            {
                bld.Append(", ").Append(Types[i]);
            }

            bld.Append('>');

            return bld.ToString();
        }

        public override bool HasDeterministicFieldOrder => true;
    }
}
