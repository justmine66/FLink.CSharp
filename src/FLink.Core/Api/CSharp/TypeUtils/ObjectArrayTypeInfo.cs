using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;
using System;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public class ObjectArrayTypeInfo<TElement> : TypeInformation<TElement[]>
    {
        public ObjectArrayTypeInfo(Type arrayType, TypeInformation<TElement> componentInfo)
        {
            TypeClass = Preconditions.CheckNotNull(arrayType);
            ComponentInfo = Preconditions.CheckNotNull(componentInfo);
        }

        public TypeInformation<TElement> ComponentInfo { get; }
        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }
        public override bool IsKeyType => false;

        public override TypeSerializer<TElement[]> CreateSerializer(ExecutionConfig config)
        {
            throw new NotImplementedException();
        }

        public override string ToString()=> $"{GetType().Name}<{ComponentInfo}>";

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectArrayTypeInfo
    {
        public static dynamic GetInfoFor<T>(Type arrayClass, TypeInformation<T> componentInfo)
        {
            Preconditions.CheckNotNull(arrayClass);
            Preconditions.CheckNotNull(componentInfo);
            Preconditions.CheckArgument(arrayClass.IsArray, "Class " + arrayClass + " must be an array.");

            return new ObjectArrayTypeInfo<T>(arrayClass, componentInfo);
        }
    }
}
