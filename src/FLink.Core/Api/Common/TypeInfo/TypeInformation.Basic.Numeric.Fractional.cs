using System;
using System.Linq;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// Type information for numeric fractional primitive types (double, float).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FractionalTypeInfo<T> : NumericTypeInfo<T>
    {
        public static readonly Type[] FractionalTypes = {
            typeof(double),
            typeof(float)
        };

        public FractionalTypeInfo(
            Type clazz, 
            Type[] possibleCastTargetTypes, 
            TypeSerializer<T> serializer, 
            TypeComparator<T> comparatorClass) 
            : base(clazz, possibleCastTargetTypes, serializer, comparatorClass)
        {
            Preconditions.CheckArgument(FractionalTypes.Contains(clazz), $"The given class {clazz.Name} is not a fractional type");
        }
    }
}
