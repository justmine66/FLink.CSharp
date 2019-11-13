namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Interface for <see cref="StreamPartitioner{T}"/> which have to be configured with the maximum parallelism of the stream transformation.
    /// The configure method is called by the StreamGraph when adding internal edges.
    /// This interface is required since the stream partitioners are instantiated eagerly. Due to that the maximum parallelism might not have been determined and needs to be set at a stage when the maximum parallelism could have been determined.
    /// </summary>
    public interface IConfigurableStreamPartitioner
    {
        /// <summary>
        /// Configure the <see cref="StreamPartitioner{T}"/> with the maximum parallelism of the down stream operator.
        /// </summary>
        /// <param name="maxParallelism">Maximum parallelism of the down stream operator.</param>
        void Configure(int maxParallelism);
    }
}
