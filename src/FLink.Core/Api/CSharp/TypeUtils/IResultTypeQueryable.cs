using FLink.Core.Api.Common.TypeInfos;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    /// <summary>
    /// This interface can be implemented by functions and input formats to tell the framework about their produced data type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResultTypeQueryable<T>
    {
        /// <summary>
        /// Gets the data type (as a <see cref="TypeInformation{TType}"/>) produced by this function or input format.
        /// </summary>
        TypeInformation<T> ProducedType { get; }
    }
}
