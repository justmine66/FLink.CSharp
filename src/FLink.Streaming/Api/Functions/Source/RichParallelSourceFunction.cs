using FLink.Core.Api.Common.Functions;

namespace FLink.Streaming.Api.Functions.Source
{
    /// <summary>
    /// Base class for implementing a parallel data source. Upon execution, the runtime will execute as many parallel instances of this function as configured parallelism of the source.
    /// The data source has access to context information (such as the number of parallel instances of the source, and which parallel instance the current instance is).
    /// </summary>
    public abstract class RichParallelSourceFunction<TOutput> : AbstractRichFunction, IParallelSourceFunction<TOutput>
    {
        public abstract void Run(ISourceContext<TOutput> ctx);

        public abstract void Cancel();
    }
}
