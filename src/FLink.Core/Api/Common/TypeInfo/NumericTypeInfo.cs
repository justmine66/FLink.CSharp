using System;
using System.Linq;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;

namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// Type information for numeric primitive types: int, long, double, byte, short, float, char.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NumericTypeInfo<T> : BasicTypeInfo<T>
    {
        public static readonly Type[] NumericalTypes = {
                typeof(int),
                typeof(long),
                typeof(double),
                typeof(byte),
                typeof(short),
                typeof(float),
                typeof(char)
            };

        protected NumericTypeInfo(Type clazz, Type[] possibleCastTargetTypes, TypeSerializer<T> serializer, TypeComparator<T> comparatorClass)
            : base(clazz, possibleCastTargetTypes, serializer, comparatorClass)
        {
            Preconditions.CheckArgument(NumericalTypes.Contains(clazz), $"The given class {clazz.Name} is not a numerical type");
        }
    }
}
