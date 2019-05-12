namespace FLink.Streaming.Api.Functions.Source
{
    /// <summary>
    /// A stream data source that is executed in parallel. Upon execution, the runtime will execute as many parallel instances of this function as configured parallelism of the source.
    /// This interface acts only as a marker to tell the system that this source may be executed in parallel. When different parallel instances are required to perform different tasks, use the <see cref="IRichParallelSourceFunction{TOutput}"/> to get access to the runtime context, which reveals information like the number of parallel tasks, and which parallel task the current instance is.
    /// </summary>
    /// <typeparam name="TOutput">The type of the records produced by this source.</typeparam>
    public interface IParallelSourceFunction<TOutput> : ISourceFunction<TOutput>
    {

    }
}
