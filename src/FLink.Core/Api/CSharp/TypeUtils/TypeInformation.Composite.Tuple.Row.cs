using System;
using System.Collections.Generic;
using System.Linq;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Types;
using FLink.Core.Util;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class RowTypeInfo : TupleTypeInfoBase<Row>
    {
        public RowTypeInfo(params TypeInformation<object>[] types)
            : base(typeof(Row), types)
        {
            var length = types.Length;
            FieldNames = new string[length];

            for (var i = 0; i < length; i++)
            {
                FieldNames[i] = "f" + i;
            }
        }

        public RowTypeInfo(TypeInformation<object>[] types, string[] fieldNames)
            : base(typeof(Row), types)
        {
            Preconditions.CheckNotNull(fieldNames, "FieldNames should not be null.");
            Preconditions.CheckArgument(types.Length == fieldNames.Length, "Number of field types and names is different.");
            Preconditions.CheckArgument(!HasDuplicateFieldNames(fieldNames), "Field names are not unique.");

            Array.Copy(fieldNames, 0, FieldNames, 0, fieldNames.Length);
        }

        public override TypeSerializer<Row> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        protected override ITypeComparatorBuilder<Row> CreateTypeComparatorBuilder()
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

        private bool HasDuplicateFieldNames(string[] fieldNames)
        {
            var names = new HashSet<string>();

            return fieldNames.Any(field => !names.Add(field));
        }
    }
}
