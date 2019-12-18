using FLink.Core.Api.Common.IO;
using FLink.Core.IO;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Input format source operator factory.
    /// </summary>
    /// <typeparam name="TOutput">The output type of the operator</typeparam>
    public interface IInputFormatOperatorFactory<TOutput> : IStreamOperatorFactory<TOutput>
    {
        /// <summary>
        /// input format of this operator factory.
        /// </summary>
        IInputFormat<TOutput, IInputSplit> InputFormat { get; }
    }
}
