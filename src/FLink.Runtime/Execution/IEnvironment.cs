using System;
using FLink.Core.Api.Common;
using FLink.Runtime.Query;
using FLink.Runtime.State.Internal;

namespace FLink.Runtime.Execution
{
    /// <summary>
    /// The Environment gives the code executed in a task access to the task's properties(such as name, parallelism), the configurations, the data stream readers and writers,as well as the various components that are provided by the TaskManager, such as memory manager, I/O manager, ...
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Returns the job specific execution configuration associated with the current job.
        /// </summary>
        ExecutionConfig ExecutionConfig { get; }

        /// <summary>
        /// Returns the ID of the job that the task belongs to.
        /// the ID of the job from the original job graph
        /// </summary>
        JobId JobId { get; }

        /// <summary>
        /// Returns the user code class type
        /// </summary>
        Type UserClassType { get; }

        /// <summary>
        /// Returns the registry for <see cref="IInternalKvState{TKey,TNamespace,TValue}"/> instances.
        /// </summary>
        TaskKvStateRegistry TaskKvStateRegistry { get; }
    }
}
