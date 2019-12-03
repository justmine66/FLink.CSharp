using FLink.Core.Api.Common.Functions;

namespace FLink.Runtime.TaskExecutors
{
    /// <summary>
    /// This interface gives access to transient, named, global aggregates. This can be sued to share state amongst parallel tasks in a job.  It is not designed for high throughput updates and the aggregates do NOT survive a job failure.  Each call to the updateGlobalAggregate() method results in serialized RPC communication with the JobMaster so use with care.
    /// </summary>
    public interface IGlobalAggregateManager
    {
        /// <summary>
        /// Update the global aggregate and return the new value.
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TAccumulator"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="aggregateName">The name of the aggregate to update</param>
        /// <param name="aggregand">The value to add to the aggregate</param>
        /// <param name="aggregateFunction">The function to apply to the current aggregate and aggregand to obtain the new aggregate value</param>
        /// <returns>The updated aggregate</returns>
        TOutput UpdateGlobalAggregate<TInput, TAccumulator, TOutput>(string aggregateName, object aggregand, IAggregateFunction<TInput, TAccumulator, TOutput> aggregateFunction);
    }
}
