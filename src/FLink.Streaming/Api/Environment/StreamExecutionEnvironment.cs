﻿using System;
using System.Collections.Generic;
using System.Text;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Streaming.Api.DataStreams;
using FLink.Streaming.Api.Functions.Source;
using FLink.Streaming.Api.Graph;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Transformations;

namespace FLink.Streaming.Api.Environment
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

        // The default buffer timeout (max delay of records in the network stack).
        private const long DefaultNetworkBufferTimeout = 100L;

        // The environment of the context (local by default, cluster if invoked through command line).
        private static IStreamExecutionEnvironmentFactory _contextEnvironmentFactory;

        // The default parallelism used when creating a local environment.
        private static int _defaultLocalParallelism = System.Environment.ProcessorCount;

        // The execution configuration for this environment.
        private static readonly ExecutionConfig Config = new ExecutionConfig();

        private readonly List<StreamTransformation<object>> _transformations = new List<StreamTransformation<object>>();

        private long _bufferTimeout = DefaultNetworkBufferTimeout;

        protected bool IsChainingEnabled = true;

        public TimeCharacteristic TimeCharacteristic;

        public int Parallelism { get; } = Config.GetParallelism();

        public T Clean<T>(T t)
        {
            return t;
        }

        /// <summary>
        /// Gets the config object. 
        /// </summary>
        /// <returns></returns>
        public ExecutionConfig GetConfig()
        {
            return Config;
        }

        public StreamExecutionEnvironment SetParallelism(int parallelism)
        {
            Config.SetParallelism(parallelism);
            return this;
        }

        /// <summary>
        /// Creates an execution environment that represents the context in which the program is currently executed.
        /// If the program is invoked standalone, this method returns a local execution environment, as returned by <see cref="CreateLocalEnvironment"/>
        /// </summary>
        /// <returns>The execution environment of the context in which the program is</returns>
        public static StreamExecutionEnvironment GetExecutionEnvironment()
        {
            return null;
        }

        /// <summary>
        /// Creates a <see cref="LocalStreamEnvironment"/>.  
        /// The local execution environment will run the program in a multi-threaded fashion in the same CLR as the  environment was created in. The default parallelism of the local environment is the number of hardware contexts(CPU cores / threads), unless it was specified differently by  <see cref="StreamExecutionEnvironment.SetParallelism"/>.
        /// </summary>
        /// <returns>A local execution environment.</returns>
        public static LocalStreamEnvironment CreateLocalEnvironment()
        {
            return null;
        }

        internal static StreamExecutionEnvironment CreateStreamExecutionEnvironment()
        {
            // because the streaming project depends on "flink-clients" (and not the other way around)
            // we currently need to intercept the data set environment and create a dependent stream env.
            // this should be fixed once we rework the project dependencies
            return null;
        }

        public JobExecutionResult Execute()
        {
            return Execute(DefaultJobName);
        }

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

        public StreamGraph GetStreamGraph(string jobName)
        {
            return null;
        }

        public StreamExecutionEnvironment SetStreamTimeCharacteristic(TimeCharacteristic characteristic)
        {
            TimeCharacteristic = Preconditions.CheckNotNull(characteristic);
            GetConfig().SetAutoWatermarkInterval(characteristic == TimeCharacteristic.ProcessingTime ? 0 : 200);
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
            var isParallel = function is IParallelSourceFunction<TOut>;

            Clean(function);

            var sourceOperator = new StreamSource<TOut, ISourceFunction<TOut>>(function);
            return new DataStreamSource<TOut>(this, typeInfo, sourceOperator, isParallel, sourceName);
        }

        #endregion

        #endregion
    }
}
