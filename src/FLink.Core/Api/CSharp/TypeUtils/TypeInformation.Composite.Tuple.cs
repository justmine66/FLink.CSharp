using System;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class TupleTypeInfo<T> : TupleTypeInfoBase<T>  
    {
        public TupleTypeInfo(Type typeClass, params TypeInformation<object>[] types)
            : base(typeClass, types)
        {
            var length = types.Length;

            FieldNames = new string[length];
            for (var i = 0; i < length; i++)
            {
                FieldNames[i] = "f" + i;
            }
        }

        public override TypeSerializer<T> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        protected override ITypeComparatorBuilder<T> CreateTypeComparatorBuilder()
        {
            throw new NotImplementedException();
        }

        public sealed override string[] FieldNames { get; }

        public override int GetFieldIndex(string fieldName)
        {
            for (var i = 0; i < FieldNames.Length; i++)
            {
                if (FieldNames[i].Equals(fieldName))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
