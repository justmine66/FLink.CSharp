using System;
using FLink.Extensions.DependencyInjection;
using FLink.Runtime.Execution;
using FLink.Runtime.JobGraphs.Tasks;
using FLink.Streaming.Api.Operators;
using Microsoft.Extensions.Logging;

namespace FLink.Streaming.Runtime.Tasks
{
    /// <summary>
    /// Base class for all streaming tasks.
    /// A task is the unit of local processing that is deployed and executed by the TaskManagers.
    /// Each task runs one or more <see cref="IStreamOperator{TOutput}"/>s which form the Task's operator chain.
    /// Operators that are chained together execute synchronously in the same thread and hence on the same stream partition.
    /// A common case for these chains are successive map/flatmap/filter tasks.
    /// 
    /// The task chain contains one "head" operator and multiple chained operators.
    /// The StreamTask is specialized for the type of the head operator: one-input and two-input tasks, as well as for sources, iteration heads and iteration tails.
    /// 
    /// The Task class deals with the setup of the streams read by the head operator, and the streams produced by the operators at the ends of the operator chain. Note that the chain may fork and thus have multiple ends.
    ///
    /// The life cycle of the task is set up as follows:
    /// -- setInitialState -> provides state of all operators in the chain
    /// -- invoke()
    ///      +----> Create basic utils (config, etc) and load the chain of operators
    ///      +----> operators.setup()
    ///      +----> task specific init()
    ///      +----> initialize-operator -states()
    ///      +----> open-operators()
    ///      +----> run()
    ///      +----> close-operators()
    ///      +----> dispose-operators()
    ///      +----> common cleanup
    ///      +----> task specific cleanup()
    ///
    /// The <see cref="StreamTask{TOutput,TOperator}"/> has a lock object. All calls to methods on a <see cref="IStreamOperator{TOutput}"/> must be synchronized on this lock object to ensure that no method are called concurrently.
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    /// <typeparam name="TOperator"></typeparam>
    public abstract class StreamTask<TOutput, TOperator> : AbstractInvokable, IAsyncExceptionHandler
            where TOperator : IStreamOperator<TOutput>
    {
        /// <summary>
        /// The logger used by the StreamTask and its subclasses.
        /// </summary>
        public static readonly ILogger Logger = ServiceLocator.GetService<ILogger<StreamTask<TOutput, TOperator>>>();

        protected StreamTask(IEnvironment environment) : base(environment)
        {

        }

        public void HandleAsyncException(string message, Exception exception)
        {

        }
    }
}
