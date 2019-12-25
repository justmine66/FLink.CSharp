﻿using System;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// Type information for arrays boxed primitive types.
    /// </summary>
    /// <typeparam name="TComponent">The type (class) of the array component.</typeparam>
    public class BasicArrayTypeInfo<TComponent> : TypeInformation<TComponent[]>
    {
        public static readonly BasicArrayTypeInfo<string> StringArrayTypeInfo = new BasicArrayTypeInfo<string>(default, BasicTypeInfo.StringTypeInfo);

        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }
        public override bool IsKeyType { get; }

        private readonly Type _arrayClass;
        private readonly BasicTypeInfo<TComponent> _componentInfo;

        private BasicArrayTypeInfo(Type arrayClass, BasicTypeInfo<TComponent> componentInfo)
        {
            _arrayClass = Preconditions.CheckNotNull(arrayClass);
            _componentInfo = Preconditions.CheckNotNull(componentInfo);
        }

        public override TypeSerializer<TComponent[]> CreateSerializer(ExecutionConfig config)
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
