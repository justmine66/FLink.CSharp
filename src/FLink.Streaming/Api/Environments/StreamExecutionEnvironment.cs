using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FLink.Clients.Program;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Cache;
using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.IO;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Api.Dag;
using FLink.Core.Configurations;
using FLink.Core.Exceptions;
using FLink.Core.IO;
using FLink.Core.Util;
using FLink.CSharp;
using FLink.Runtime.State;
using FLink.Streaming.Api.Checkpoint;
using FLink.Streaming.Api.DataStreams;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Graphs;
using FLink.Streaming.Api.Operators;

namespace FLink.Streaming.Api.Environments
{
    /// <summary>
    /// The StreamExecutionEnvironment is the context in which a streaming program is executed. 
    /// </summary>
    /// <remarks>
    /// The environment provides methods to control the job execution (such as setting the parallelism or the fault tolerance/check pointing parameters) and to interact with the outside world(data access).
    /// </remarks>
    public abstract class StreamExecutionEnvironment
    {
        // The default name to use for a streaming job if no other name has been specified.
        public const string DefaultJobName = "Flink Streaming Job";

        // The time characteristic that is used if none other is set.
        private const TimeCharacteristic DefaultTimeCharacteristic = TimeCharacteristic.ProcessingTime;

        // The default buffer timeout 100ms (max delay of records in the network stack).
        private const long DefaultNetworkBufferTimeout = 100L;

        // The environment of the context (local by default, cluster if invoked through command line).
        private static readonly IStreamExecutionEnvironmentFactory ContextEnvironmentFactory;
        // The ThreadLocal used to store IStreamExecutionEnvironmentFactory.
        private static readonly ThreadLocal<IStreamExecutionEnvironmentFactory> ThreadLocalContextEnvironmentFactory = new ThreadLocal<IStreamExecutionEnvironmentFactory>();

        // The default parallelism used when creating a local environment.
        private static int _defaultLocalParallelism = System.Environment.ProcessorCount;

        // The execution configuration for this environment.
        private static readonly ExecutionConfig Config = new ExecutionConfig();

        private readonly List<Transformation<object>> _transformations = new List<Transformation<object>>();

        /// <summary>
        /// Gets whether operator chaining is enabled.
        /// true if chaining is enabled, false otherwise.
        /// </summary>
        public bool IsChainingEnabled = true;

        /// <summary>
        /// Get the list of cached files that were registered for distribution among the task managers.
        /// </summary>
        public IList<(string, DistributedCacheEntry)> CachedFiles = new List<(string, DistributedCacheEntry)>();

        public TimeCharacteristic TimeCharacteristic;

        /// <summary>
        /// A maximum wait time for the buffers to fill up in the network stack. The default buffer timeout 100ms.
        /// </summary>
        public long BufferTimeout = DefaultNetworkBufferTimeout;

        /// <summary>
        /// Gets the parallelism with which operation are executed by default. Operations can individually override this value to use a specific parallelism.
        /// </summary>
        public int Parallelism { get; } = Config.Parallelism;

        public IList<Transformation<dynamic>> Transformations = new List<Transformation<dynamic>>();

        /// <summary>
        /// Returns a "closure-cleaned" version of the given function. Cleans only if closure cleaning is not disabled in the <see cref="ExecutionConfig"/>.
        /// </summary>
        /// <typeparam name="TFunction"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public TFunction Clean<TFunction>(TFunction function)
        {
            return function;
        }

        /// <summary>
        /// Gets the config object. 
        /// </summary>
        public ExecutionConfig ExecutionConfig => Config;

        /// <summary>
        /// Sets the parallelism for operations executed through this environment. Setting a parallelism of x here will cause all operators (such as map, batchReduce) to run with x parallel instances. This method overrides the default parallelism for this environment. The <see cref="LocalStreamEnvironment"/> uses by default a value equal to the number of hardware contexts(CPU cores / threads). 
        /// </summary>
        /// <param name="parallelism">The parallelism</param>
        /// <returns>The configured <see cref="StreamExecutionEnvironment"/>.</returns>
        public StreamExecutionEnvironment SetParallelism(int parallelism)
        {
            Config.SetParallelism(parallelism);
            return this;
        }

        /// <summary>
        /// Disables operator chaining for streaming operators.
        /// Operator chaining allows non-shuffle operations to be co-located in the same thread fully avoiding serialization and de-serialization.
        /// </summary>
        /// <returns>StreamExecutionEnvironment with chaining disabled.</returns>
        public StreamExecutionEnvironment DisableOperatorChaining()
        {
            IsChainingEnabled = false;

            return this;
        }

        /// <summary>
        /// Registers a file at the distributed cache under the given name.
        /// The file will be accessible from any user-defined function in the (distributed) runtime under a local path.
        /// Files may be local files (which will be distributed via BlobServer), or files in a distributed file system.
        /// The runtime will copy the files temporarily to a local cache, if needed.
        /// </summary>
        /// <param name="filePath">The path of the file, as a URI (e.g. "file:///some/path" or "hdfs://host:port/and/path")</param>
        /// <param name="name">The name under which the file is registered.</param>
        /// <param name="executable">Flag indicating whether the file should be executable</param>
        public void RegisterCachedFile(string filePath, string name, bool executable)
        {
            CachedFiles.Add((name, new DistributedCacheEntry(filePath, executable)));
        }

        /// <summary>
        /// Creates an execution environment that represents the context in which the program is currently executed.
        /// If the program is invoked standalone, this method returns a local execution environment, as returned by <see cref="CreateLocalEnvironment()"/>
        /// </summary>
        /// <returns>The execution environment of the context in which the program is</returns>
        public static StreamExecutionEnvironment GetExecutionEnvironment()
        {
            var factory = Utils.ResolveFactory(ThreadLocalContextEnvironmentFactory, ContextEnvironmentFactory);
            var environment = factory?.CreateExecutionEnvironment() ?? CreateStreamExecutionEnvironment();

            return environment;
        }

        /// <summary>
        /// Creates a <see cref="LocalStreamEnvironment"/>.  
        /// The local execution environment will run the program in a multi-threaded fashion in the same CLR as the  environment was created in. The default parallelism of the local environment is the number of hardware contexts(CPU cores / threads), unless it was specified differently by  <see cref="StreamExecutionEnvironment.SetParallelism"/>.
        /// </summary>
        /// <returns>A local execution environment.</returns>
        public static LocalStreamEnvironment CreateLocalEnvironment() => CreateLocalEnvironment(_defaultLocalParallelism);

        /// <summary>
        /// Creates a <see cref="LocalStreamEnvironment"/>. The local execution environment will run the program in a multi-threaded fashion in the same CLR as the environment was created in. It will use the parallelism specified in the parameter.
        /// </summary>
        /// <param name="parallelism">The parallelism for the local environment.</param>
        /// <returns>A local execution environment with the specified parallelism.</returns>
        public static LocalStreamEnvironment CreateLocalEnvironment(int parallelism) => CreateLocalEnvironment(parallelism, new Configuration());

        /// <summary>
        /// Creates a <see cref="LocalStreamEnvironment"/>. The local execution environment will run the program in a multi-threaded fashion in the same CLR as the environment was created in. It will use the parallelism specified in the parameter.
        /// </summary>
        /// <param name="parallelism">The parallelism for the local environment.</param>
        /// <param name="configuration">Pass a custom configuration into the cluster</param>
        /// <returns>A local execution environment with the specified parallelism.</returns>
        public static LocalStreamEnvironment CreateLocalEnvironment(int parallelism, Configuration configuration)
        {
            var currentEnvironment = new LocalStreamEnvironment(configuration);
            currentEnvironment.SetParallelism(parallelism);

            return currentEnvironment;
        }

        private static StreamExecutionEnvironment CreateStreamExecutionEnvironment()
        {
            // because the streaming project depends on "FLink-clients" (and not the other way around)
            // we currently need to intercept the data set environment and create a dependent stream env.
            // this should be fixed once we rework the project dependencies

            var env = ExecutionEnvironment.GetExecutionEnvironment();

            switch (env)
            {
                case ContextEnvironment environment:
                    return new StreamContextEnvironment(environment);
                case OptimizerPlanEnvironment _:
                case PreviewPlanEnvironment _:
                    return new StreamPlanEnvironment(env);
                default:
                    return CreateLocalEnvironment();
            }
        }

        /// <summary>
        /// Getter of the <see cref="StreamGraph"/> of the streaming job.
        /// This call clears previously registered <see cref="Transformations"/>.
        /// </summary>
        /// <returns>The streamgraph representing the transformations</returns>
        public StreamGraph GetStreamGraph() => GetStreamGraph(DefaultJobName);

        /// <summary>
        /// Getter of the <see cref="StreamGraph"/> of the streaming job.
        /// This call clears previously registered <see cref="Transformations"/>.
        /// </summary>
        /// <param name="jobName">Desired name of the job</param>
        /// <returns>The streamgraph representing the transformations</returns>
        public StreamGraph GetStreamGraph(string jobName) => GetStreamGraph(jobName, true);

        /// <summary>
        /// Getter of the <see cref="StreamGraph"/> of the streaming job with the option to clear previously registered <see cref="Transformations"/>.
        /// Clearing the transformations allows, for example, to not re-execute the same operations when calling <see cref="Execute()"/> multiple times.
        /// </summary>
        /// <param name="jobName">Desired name of the job</param>
        /// <param name="clearTransformations">Whether or not to clear previously registered transformations</param>
        /// <returns>The streamgraph representing the transformations</returns>
        public StreamGraph GetStreamGraph(string jobName, bool clearTransformations)
        {
            var streamGraph = GetStreamGraphGenerator().SetJobName(jobName).Generate();
            if (clearTransformations)
            {
                Transformations.Clear();
            }

            return streamGraph;
        }

        private StreamGraphGenerator GetStreamGraphGenerator()
        {
            if (Transformations.Count <= 0)
            {
                throw new IllegalStateException("No operators defined in streaming topology. Cannot execute.");
            }

            return new StreamGraphGenerator(Transformations, ExecutionConfig, CheckpointConfig)
                .SetStateBackend(StateBackend)
                .SetChaining(IsChainingEnabled)
                .SetUserArtifacts(CachedFiles)
                .SetTimeCharacteristic(TimeCharacteristic)
                .SetDefaultBufferTimeout(BufferTimeout);
        }

        public StreamExecutionEnvironment SetStreamTimeCharacteristic(TimeCharacteristic characteristic)
        {
            TimeCharacteristic = Preconditions.CheckNotNull(characteristic);
            ExecutionConfig.SetAutoWatermarkInterval(characteristic == TimeCharacteristic.ProcessingTime ? 0 : 200);
            return this;
        }

        /// <summary>
        /// Sets the maximum time frequency (milliseconds) for the flushing of the output buffers. By default the output buffers flush frequently to provide low latency and to aid smooth developer experience. Setting the parameter can result in three logical modes:
        /// 1. A positive integer triggers flushing periodically by that integer.
        /// 2. 0 triggers flushing after every record thus minimizing latency. A buffer timeout of 0 should be avoided, because it can cause severe performance degradation.
        /// 3. -1 triggers flushing only when the output buffer is full thus maximizing throughput. it will remove the timeout and buffers will only be flushed when they are full.
        /// </summary>
        /// <param name="timeoutMillis">The maximum time between two output flushes.</param>
        /// <returns>The configured <see cref="StreamExecutionEnvironment"/>.</returns>
        public StreamExecutionEnvironment SetBufferTimeout(long timeoutMillis)
        {
            BufferTimeout = timeoutMillis;
            return this;
        }

        #region [ Source ]

        #region [ memory ]

        public DataStreamSource<TOut> FromCollection<TOut>(IEnumerable<TOut> data, TypeInformation<TOut> typeInfo)
        {
            Preconditions.CheckNotNull(data, "Collection must not be null");

            // must not have null elements and mixed elements
            FromElementsFunction<TOut>.CheckCollection(data, typeInfo.TypeClass);

            ISourceFunction<TOut> function;
            try
            {
                function = new FromElementsFunction<TOut>(typeInfo.CreateSerializer(Config), data);
            }
            catch (Exception e)
            {
                throw new RuntimeException(e.Message, e);
            }

            return AddSource(function, "Collection Source", typeInfo)
                .SetParallelism(1);
        }

        #endregion

        #region [ socket ]

        /// <summary>
        /// Creates a new data stream that contains the strings received infinitely from a socket. Received strings are decoded by the system's default character set. On the termination of the socket server connection retries can be initiated.
        /// </summary>
        /// <param name="hostname">The host name which a server socket binds</param>
        /// <param name="port">The port number which a server socket binds. A port number of 0 means that the port number is automatically allocated.</param>
        /// <returns>A data stream containing the strings received from the socket</returns>
        public DataStreamSource<string> SocketTextStream(string hostname, int port)
        {
            return SocketTextStream(hostname, port, "\n");
        }

        /// <summary>
        /// Creates a new data stream that contains the strings received infinitely from a socket. Received strings are decoded by the system's default character set. On the termination of the socket server connection retries can be initiated.
        /// </summary>
        /// <param name="hostname">The host name which a server socket binds</param>
        /// <param name="port">The port number which a server socket binds. A port number of 0 means that the port number is automatically allocated.</param>
        /// <param name="delimiter">A string which splits received strings into records</param>
        /// <returns>A data stream containing the strings received from the socket</returns>
        public DataStreamSource<string> SocketTextStream(string hostname, int port, string delimiter)
        {
            return SocketTextStream(hostname, port, delimiter, 0);
        }

        /// <summary>
        /// Creates a new data stream that contains the strings received infinitely from a socket. Received strings are decoded by the system's default character set. On the termination of the socket server connection retries can be initiated.
        /// </summary>
        /// <param name="hostname">The host name which a server socket binds</param>
        /// <param name="port">The port number which a server socket binds. A port number of 0 means that the port number is automatically allocated.</param>
        /// <param name="delimiter">A string which splits received strings into records</param>
        /// <param name="maxRetryIntervalSeconds">The maximal retry interval in seconds while the program waits for a socket that is temporarily down. Reconnection is initiated every second. A number of 0 means that the reader is immediately terminated, while a negative value ensures retrying forever.</param>
        /// <returns>A data stream containing the strings received from the socket</returns>
        public DataStreamSource<string> SocketTextStream(string hostname, int port, string delimiter,
            int maxRetryIntervalSeconds)
        {
            return AddSource(new SocketTextStreamFunction(hostname, port, delimiter, maxRetryIntervalSeconds),
                "Socket Stream");
        }

        #endregion

        #region [ file ]

        public DataStreamSource<TOut> ReadCsvFile<TOut>(string filePath)
        {
            return null;
        }

        /// <summary>
        /// Reads the given file line-by-line and creates a data stream that contains a string with the contents of each such line. The file will be read with the UTF-8 character set.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>The data stream that represents the data read from the given file as text lines.</returns>
        public DataStreamSource<string> ReadTextFile(string filePath) => ReadTextFile(filePath, Encoding.UTF8);

        /// <summary>
        /// Reads the given file line-by-line and creates a data stream that contains a string with the contents of each such line. The file will be read with the UTF-8 character set.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <param name="charset">The name of the character set used to read the file.</param>
        /// <returns>The data stream that represents the data read from the given file as text lines.</returns>
        public DataStreamSource<string> ReadTextFile(string filePath, Encoding charset)
        {
            Preconditions.CheckArgument(!string.IsNullOrWhiteSpace(filePath), "The file path must not be null or blank.");

            return null;
        }

        #endregion

        #region [ custom ]

        /// <summary>
        /// Adds a Data Source to the streaming topology.
        /// </summary>
        /// <typeparam name="TOut">type of the returned stream</typeparam>
        /// <param name="function">the user defined function</param>
        /// <returns>the data stream constructed</returns>
        public DataStreamSource<TOut> AddSource<TOut>(ISourceFunction<TOut> function) => AddSource(function, "Custom Source");

        /// <summary>
        /// Adds a data source with a custom type information thus opening a <see cref="DataStream{TElement}"/>. Only in very special cases does the user need to support type information. Otherwise use <see cref="ISourceFunction{T}"/>.
        /// </summary>
        /// <typeparam name="TOut">type of the returned stream</typeparam>
        /// <param name="function">the user defined function</param>
        /// <param name="sourceName">Name of the data source</param>
        /// <returns>the data stream constructed</returns>
        public DataStreamSource<TOut> AddSource<TOut>(ISourceFunction<TOut> function, string sourceName) => AddSource(function, sourceName, null);

        /// <summary>
        /// Ads a data source with a custom type information thus opening a <see cref="DataStream{TElement}"/>. Only in very special cases does the user need to support type information. Otherwise use <see cref="ISourceFunction{T}"/>.
        /// </summary>
        /// <typeparam name="TOut">type of the returned stream</typeparam>
        /// <param name="function">the user defined function</param>
        /// <param name="typeInfo">the user defined type information for the stream</param>
        /// <returns>the data stream constructed</returns>
        public DataStreamSource<TOut> AddSource<TOut>(ISourceFunction<TOut> function, TypeInformation<TOut> typeInfo) => AddSource(function, "Custom Source", typeInfo);

        /// <summary>
        /// Ads a data source with a custom type information thus opening a <see cref="DataStream{TElement}"/>. Only in very special cases does the user need to support type information. Otherwise use <see cref="ISourceFunction{T}"/>.
        /// </summary>
        /// <typeparam name="TOut">type of the returned stream</typeparam>
        /// <param name="function">the user defined function</param>
        /// <param name="sourceName">Name of the data source</param>
        /// <param name="typeInfo">the user defined type information for the stream</param>
        /// <returns>the data stream constructed</returns>
        public DataStreamSource<TOut> AddSource<TOut>(ISourceFunction<TOut> function, string sourceName, TypeInformation<TOut> typeInfo)
        {
            if (function is IResultTypeQueryable<TOut> queryable)
                typeInfo = queryable.ProducedType;

            if (typeInfo == null)
            {
                try
                {
                    typeInfo = TypeExtractor.CreateTypeInfo<object, object, TOut>(typeof(ISourceFunction<>), function.GetType(), 0, null, null);
                }
                catch (InvalidTypesException e)
                {
                    typeInfo = new MissingTypeInfo(sourceName, e) as TypeInformation<TOut>;
                }
            }

            var isParallel = function is IParallelSourceFunction<TOut>;

            Clean(function);

            var sourceOperator = new StreamSource<TOut, ISourceFunction<TOut>>(function);
            return new DataStreamSource<TOut>(this, typeInfo, sourceOperator, isParallel, sourceName);
        }

        /// <summary>
        /// Generic method to create an input data stream with <see cref="IInputFormat{TRecord,TInputSplit}"/>.
        /// Since all data streams need specific information about their types, this method needs to determine the type of the data produced by the input format. It will attempt to determine the data type by reflection, unless the input format implements <see cref="IResultTypeQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="TOutput">The type of the returned data stream</typeparam>
        /// <param name="inputFormat">The input format used to create the data stream</param>
        /// <returns>The data stream that represents the data created by the input format</returns>
        public DataStreamSource<TOutput> CreateInput<TOutput>(IInputFormat<TOutput, IInputSplit> inputFormat)
        {
            return CreateInput(inputFormat, TypeExtractor.GetInputFormatTypes(inputFormat));
        }

        /// <summary>
        /// Generic method to create an input data stream with <see cref="IInputFormat{TRecord,TInputSplit}"/>.
        /// Since all data streams need specific information about their types, this method needs to determine the type of the data produced by the input format. It will attempt to determine the data type by reflection, unless the input format implements <see cref="IResultTypeQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="TOutput">The type of the returned data stream</typeparam>
        /// <param name="inputFormat">The input format used to create the data stream</param>
        /// <param name="typeInfo">The information about the type of the output type</param>
        /// <returns>The data stream that represents the data created by the input format</returns>
        public DataStreamSource<TOutput> CreateInput<TOutput>(IInputFormat<TOutput, IInputSplit> inputFormat, TypeInformation<TOutput> typeInfo)
        {
            switch (inputFormat)
            {
                case FileInputFormat<TOutput> format:
                    return CreateFileInput(format, typeInfo, "Custom File source", FileProcessingMode.ProcessOnce, -1);
                default:
                    return CreateInput(inputFormat, typeInfo, "Custom Source");
            }
        }

        private DataStreamSource<TOutput> CreateInput<TOutput>(IInputFormat<TOutput, IInputSplit> inputFormat,
        TypeInformation<TOutput> typeInfo, string sourceName)
        {
            var function = new InputFormatSourceFunction<TOutput>(inputFormat, typeInfo);
            return AddSource(function, sourceName, typeInfo);
        }

        private DataStreamSource<OUT> CreateFileInput<OUT>(FileInputFormat<OUT> inputFormat,
            TypeInformation<OUT> typeInfo,
            String sourceName,
            FileProcessingMode monitoringMode,
            long interval)
        {
            Preconditions.CheckNotNull(inputFormat, "Unspecified file input format.");
            Preconditions.CheckNotNull(typeInfo, "Unspecified output type information.");
            Preconditions.CheckNotNull(sourceName, "Unspecified name for the source.");
            Preconditions.CheckNotNull(monitoringMode, "Unspecified monitoring mode.");

            return null;
        }

        #endregion

        #endregion

        #region [ State ]

        /// <summary>
        /// Gets the state backend that defines how to store and checkpoint state.
        /// </summary>
        public IStateBackend StateBackend { get; private set; }

        public CheckpointConfig CheckpointConfig = new CheckpointConfig();

        /// <summary>
        /// Sets the state backend that describes how to store and checkpoint operator state.
        /// It defines both which data structures hold state during execution(for example hash tables, RockDB, or other data stores) as well as where checkpointed data will be persisted.
        /// State managed by the state backend includes both keyed state that is accessible on <see cref="KeyedStream{TElement,TKey}"/>, as well as state maintained directly by the user code that implements <see cref="ICheckpointedFunction"/>
        /// </summary>
        /// <param name="backend"></param>
        /// <returns>This StreamExecutionEnvironment itself, to allow chaining of function calls.</returns>
        public StreamExecutionEnvironment SetStateBackend(IStateBackend backend)
        {
            StateBackend = Preconditions.CheckNotNull(backend);
            return this;
        }

        /// <summary>
        /// Enables checkpointing for the streaming job. The distributed state of the streaming dataflow will be periodically snapshotted.In case of a failure, the streaming will be restarted from the latest completed checkpoint. This method selects <see cref="CheckpointingMode.ExactlyOnce"/> guarantees.
        /// The job draws checkpoints periodically, in the given interval. The state will be stored in the configured state backend.
        /// </summary>
        /// <param name="interval">Time interval between state checkpoints in milliseconds.</param>
        /// <returns>This StreamExecutionEnvironment itself, to allow chaining of function calls.</returns>
        public StreamExecutionEnvironment EnableCheckpointing(long interval)
        {
            CheckpointConfig.CheckpointInterval = interval;
            return this;
        }

        /// <summary>
        /// Enables checkpointing for the streaming job. The distributed state of the streaming dataflow will be periodically snapshotted.In case of a failure, the streaming will be restarted from the latest completed checkpoint. This method selects <see cref="CheckpointingMode.ExactlyOnce"/> guarantees.
        /// The job draws checkpoints periodically, in the given interval. The state will be stored in the configured state backend.
        /// </summary>
        /// <param name="interval">Time interval between state checkpoints in milliseconds.</param>
        /// <param name="mode">The checkpointing mode, selecting between "exactly once" and "at least once" guaranteed.</param>
        /// <returns>This StreamExecutionEnvironment itself, to allow chaining of function calls.</returns>
        public StreamExecutionEnvironment EnableCheckpointing(long interval, CheckpointingMode mode)
        {
            CheckpointConfig.CheckpointingMode = mode;
            CheckpointConfig.CheckpointInterval = interval;
            return this;
        }

        #endregion

        /// <summary>
        /// Adds an operator to the list of operators that should be executed when calling <see cref="Execute()"/>.
        /// When calling <see cref="Execute()"/> only the operators that where previously added to the list are executed.
        /// This is not meant to be used by users. The API methods that create operators must call this method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transformation"></param>
        internal void AddOperator<T>(Transformation<T> transformation)
        {
            Preconditions.CheckNotNull(transformation, "transformation must not be null.");

            Transformations.Add(transformation as Transformation<dynamic>);
        }

        public JobExecutionResult Execute() => Execute(DefaultJobName);

        /// <summary>
        /// Triggers the program execution. The environment will execute all parts of the program that have resulted in a "sink" operation.Sink operations are for example printing results or forwarding them to a message queue.
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public JobExecutionResult Execute(string jobName)
        {
            Preconditions.CheckNotNull(jobName, "Streaming Job name should not be null.");

            return Execute(GetStreamGraph(jobName));
        }

        public abstract JobExecutionResult Execute(StreamGraph streamGraph);
    }
}
