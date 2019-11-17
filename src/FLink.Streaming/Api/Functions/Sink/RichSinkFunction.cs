using FLink.Core.Api.Common.Functions;

namespace FLink.Streaming.Api.Functions.Sink
{
    public abstract class RichSinkFunction<TInput> : AbstractRichFunction, ISinkFunction<TInput>
    {
        public abstract void Invoke(TInput value, ISinkContext<TInput> context);
    }
}
