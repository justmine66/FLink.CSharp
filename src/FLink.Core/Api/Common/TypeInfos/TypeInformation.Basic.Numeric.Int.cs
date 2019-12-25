using System;
using System.Linq;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// Type information for numeric integer primitive types: int, long, byte, short, char.
    /// </summary>
    public class IntTypeInfo<T> : NumericTypeInfo<T>
    {
        public static readonly Type[] IntTypes = {
            typeof(int),
            typeof(long),
            typeof(byte),
            typeof(short),
            typeof(char)
        };

        public IntTypeInfo(
            Type clazz, 
            Type[] possibleCastTargetTypes, 
            TypeSerializer<T> serializer, 
            TypeComparator<T> comparatorClass)
            : base(clazz, possibleCastTargetTypes, serializer, comparatorClass)
        {
            Preconditions.CheckArgument(IntTypes.Contains(clazz), $"The given class {clazz.Name} is not a numerical type");
        }
    }
}
