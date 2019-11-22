using FLink.Core.Api.Common.TypeInfo;

namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// Base type information class for Tuple and Pojo types.
    /// The class is taking care of serialization and comparators for Tuples as well.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CompositeType<T> : TypeInformation<T>
    {
         
    }
}
