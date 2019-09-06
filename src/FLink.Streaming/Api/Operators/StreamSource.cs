using FLink.Streaming.Api.Functions.Source;

namespace FLink.Streaming.Api.Operators
{
    public class StreamSource<TOut, TSrc> : AbstractUdfStreamOperator<TOut, TSrc>, IStreamOperator<TOut>
        where TSrc : ISourceFunction<TOut>
    {
        public StreamSource(TSrc sourceFunction)
            : base(sourceFunction)
        {
             
        }
    }
}
