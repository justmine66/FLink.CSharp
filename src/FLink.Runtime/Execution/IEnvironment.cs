using System;
using FLink.Core.Api.Common;
using FLink.Core.Configurations;
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
        /// Gets the job specific execution configuration associated with the current job.
        /// </summary>
        ExecutionConfig ExecutionConfig { get; }

        /// <summary>
        /// Gets the ID of the job that the task belongs to.
        /// the ID of the job from the original job graph
        /// </summary>
        JobId JobId { get; }

        /// <summary>
        /// Gets the user code class type
        /// </summary>
        Type UserClassType { get; }

        /// <summary>
        /// Gets the registry for <see cref="IInternalKvState{TKey,TNamespace,TValue}"/> instances.
        /// </summary>
        TaskKvStateRegistry TaskKvStateRegistry { get; }

        /// <summary>
        /// Gets the <see cref="TaskInfo"/> object associated with this subtask.
        /// </summary>
        TaskInfo TaskInfo { get; }

        /// <summary>
        /// Gets the task-wide configuration object, originally attached to the job vertex.
        /// </summary>
        Configuration TaskConfiguration { get; }

        /// <summary>
        /// Gets the job-wide configuration object that was attached to the JobGraph.
        /// </summary>
        Configuration JobConfiguration { get; }
    }
}
