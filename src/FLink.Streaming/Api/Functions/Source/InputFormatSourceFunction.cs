using FLink.Core.Api.Common.IO;
using FLink.Core.Api.Common.TypeInfos;
using FLink.Core.IO;

namespace FLink.Streaming.Api.Functions.Source
{
    /// <summary>
    /// A <see cref="ISourceFunction{T}"/> that reads data using an <see cref="IInputFormat{TRecord,TInputSplit}"/>.
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public class InputFormatSourceFunction<TOutput> : RichParallelSourceFunction<TOutput>
    {
        private readonly TypeInformation<TOutput> _typeInfo;
        private IInputFormat<TOutput, IInputSplit> _format;

        public InputFormatSourceFunction(IInputFormat<TOutput, IInputSplit> format, TypeInformation<TOutput> typeInfo)
        {
            _format = format;
            _typeInfo = typeInfo;
        }

        public override void Run(ISourceFunctionContext<TOutput> ctx)
        {
            throw new System.NotImplementedException();
        }

        public override void Cancel()
        {
            throw new System.NotImplementedException();
        }
    }
}
