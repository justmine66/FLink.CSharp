using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Runtime.StreamStatuses;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// <see cref="IStreamOperator{TOutput}"/> for streaming sources.
    /// </summary>
    /// <typeparam name="TOutput">Type of the output elements</typeparam>
    /// <typeparam name="TFunction">Type of the source function of this stream source operator</typeparam>
    public class StreamSource<TOutput, TFunction> : AbstractUdfStreamOperator<TOutput, TFunction>
        where TFunction : ISourceFunction<TOutput>
    {
        private readonly ISourceFunctionContext<TOutput> _context;

        private volatile bool _canceledOrStopped = false;
        private volatile bool _hasSentMaxWatermark = false;

        public StreamSource(TFunction sourceFunction)
            : base(sourceFunction)
        {
            ChainingStrategy = ChainingStrategy.Head;
        }

        public void Run(object lockingObject,
          IStreamStatusMaintainer streamStatusMaintainer,
          OperatorChain<object, IStreamOperator<object>> operatorChain)
        {

        }
    }
}
