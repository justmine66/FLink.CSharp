using FLink.Core.Api.Common.TypeInfo;

namespace FLink.Core.Util
{
    /// <summary>
    /// An <see cref="OutputTag{T}"/> is a typed and named tag to use for tagging side outputs of an operator.
    /// An <see cref="OutputTag{T}"/> must always be an anonymous inner class so that Flink can derive a <see cref="TypeInformation{TType}"/> for the generic type parameter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OutputTag<T>
    {
        public TypeInformation<T> TypeInfo { get; }
    }
}
