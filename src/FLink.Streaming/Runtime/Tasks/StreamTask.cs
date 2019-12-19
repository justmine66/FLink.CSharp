using System;
using FLink.Core.Api.CSharp.Threading;
using FLink.Extensions.DependencyInjection;
using FLink.Runtime.Execution;
using FLink.Runtime.IO.Network.Api.Writer;
using FLink.Runtime.JobGraphs.Tasks;
using FLink.Runtime.Pluggable;
using FLink.Runtime.State;
using FLink.Streaming.Api;
using FLink.Streaming.Api.Graphs;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Runtime.IO;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.Tasks.Mailbox;
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

        private readonly SynchronizedStreamTaskActionExecutor _actionExecutor;

        public IStreamInputProcessor InputProcessor { get; }

        public TOperator HeadOperator { get; }

        public OperatorChain<TOutput, TOperator> OperatorChain { get; }

        /// <summary>
        /// The configuration of this streaming task.
        /// </summary>
        public StreamConfig Configuration { get; }

        /// <summary>
        /// Our state backend. We use this to create checkpoint streams and a keyed state backend.
        /// </summary>
        public IStateBackend StateBackend { get; }

        /// <summary>
        /// The external storage where checkpoint data is persisted.
        /// </summary>
        private ICheckpointStorageWorkerView CheckpointStorage { get; }

        public ITimerService TimerService { get; }

        private readonly IUnCaughtExceptionHandler _unCaughtExceptionHandler;

        /// <summary>
        /// Flag to mark the task "in operation", in which case check needs to be initialized to true, so that early cancel() before invoke() behaves correctly.
        /// </summary>
        private volatile bool _isRunning;

        /// <summary>
        /// Flag to mark this task as canceled.
        /// </summary>
        private volatile bool _canceled;

        private readonly IRecordWriterDelegate<SerializationDelegate<StreamRecord<TOutput>>> _recordWriter;

        /// <summary>
        /// Constructor for initialization, possibly with initial state (recovery / savepoint / etc).   
        /// </summary>
        /// <param name="environment">The task environment for this task.</param>
        /// <param name="timerService">A specific timer service to use.</param>
        /// <param name="exceptionHandler">To handle uncaught exceptions in the async operations thread pool</param>
        /// <param name="actionExecutor">A mean to wrap all actions performed by this task thread. Currently, only SynchronizedActionExecutor can be used to preserve locking semantics.</param>
        protected StreamTask(
            IEnvironment environment,
            ITimerService timerService = null,
            IUnCaughtExceptionHandler exceptionHandler = null,
            SynchronizedStreamTaskActionExecutor actionExecutor = null)
            : this(environment, timerService, exceptionHandler, actionExecutor, null)
        { }

        /// <summary>
        /// Constructor for initialization, possibly with initial state (recovery / savepoint / etc).
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="timerService"></param>
        /// <param name="exceptionHandler"></param>
        /// <param name="actionExecutor"></param>
        /// <param name="mailbox"></param>
        protected StreamTask(
            IEnvironment environment,
            ITimerService timerService,
            IUnCaughtExceptionHandler exceptionHandler,
            SynchronizedStreamTaskActionExecutor actionExecutor,
            ITaskMailbox mailbox)
            : base(environment)
        {
            TimerService = timerService;
            _unCaughtExceptionHandler = exceptionHandler;
            Configuration = new StreamConfig(TaskConfiguration);
        }

        public void HandleAsyncException(string message, Exception exception)
        {

        }
    }
}
