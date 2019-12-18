using FLink.Core.Api.Common.IO;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Interface for operator factories which create the sink operator containing an <see cref="IOutputFormat{TRecord}"/>.
    /// </summary>
    /// <typeparam name="TInput">The input type of the operator.</typeparam>
    public interface IOutputFormatOperatorFactory<in TInput> : IStreamOperatorFactory<object>
    {
        /// <summary>
        /// output format of the operator created by this factory.
        /// </summary>
        IOutputFormat<TInput> OutputFormat { get; }
    }
}
