using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Util;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class ListTypeInfo<T> : TypeInformation<IList<T>>
    {
        public ListTypeInfo(TypeInformation<T> elementTypeInfo)
        {
            ElementTypeInfo = Preconditions.CheckNotNull(elementTypeInfo);
        }

        /// <summary>
        /// Gets the type information for the elements contained in the list.
        /// </summary>
        public TypeInformation<T> ElementTypeInfo { get; }

        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 0;
        public override int TotalFields => 1;
        public override Type TypeClass => typeof(IList<T>);
        public override bool IsKeyType => false;
        public override TypeSerializer<IList<T>> CreateSerializer(ExecutionConfig config)
        {
            var elementTypeSerializer = ElementTypeInfo.CreateSerializer(config);
            return new ListSerializer<T>(elementTypeSerializer);
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
