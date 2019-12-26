using System;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// Type information for arrays boxed primitive types.
    /// </summary>
    /// <typeparam name="TComponent">The type (class) of the array component.</typeparam>
    public class BasicArrayTypeInfo<TComponent> : TypeInformation<TComponent[]>
    {
        public override bool IsBasicType => false;
        public override bool IsTupleType => false;
        public override int Arity => 1;
        public override int TotalFields => 1;
        public override Type TypeClass { get; }
        public override bool IsKeyType => false;

        public BasicTypeInfo<TComponent> ComponentInfo { get; }

        public BasicArrayTypeInfo(Type arrayClass, BasicTypeInfo<TComponent> componentInfo)
        {
            TypeClass = Preconditions.CheckNotNull(arrayClass);
            ComponentInfo = Preconditions.CheckNotNull(componentInfo);
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

    public class BasicArrayTypeInfo
    {
        public static readonly BasicArrayTypeInfo<string> StringArrayTypeInfo = new BasicArrayTypeInfo<string>(default, BasicTypeInfo.StringTypeInfo);

        public static dynamic GetInfoFor(Type type)
        {
            if (!type.IsArray)
            {
                throw new InvalidTypesException("The given class is no array.");
            }

            // basic type arrays
            if (type == typeof(string))
            {
                return StringArrayTypeInfo;
            }

            return null;
        }
    }
}
