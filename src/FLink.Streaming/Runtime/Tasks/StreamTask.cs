using System;
using FLink.Runtime.Execution;
using FLink.Runtime.JobGraphs.Tasks;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// Base class for all streaming tasks. A task is the unit of local processing that is deployed and executed by the TaskManagers. Each task runs one or more <see cref="IStreamOperator{TOutput}"/>s which form the Task's operator chain. Operators that are chained together execute synchronously in the same thread and hence on the same stream partition. A common case for these chains are successive map/flatmap/filter tasks.
    /// The task chain contains one "head" operator and multiple chained operators.
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    /// <typeparam name="TOperator"></typeparam>
    public abstract class StreamTask<TOutput, TOperator> : AbstractInvokable, IAsyncExceptionHandler 
        where TOperator : IStreamOperator<TOutput>
    {
        protected StreamTask(IEnvironment environment) : base(environment)
        {
        }

        public void HandleAsyncException(string message, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
