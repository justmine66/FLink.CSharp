namespace FLink.Streaming.Api.Environment
{
    public static class StreamExecutionEnvironmentExtension
    {
        /// <summary>
        /// Creates an execution environment that represents the context in which the program is currently executed.
        /// If the program is invoked standalone, this method returns a local execution environment, as returned by <see cref="CreateLocalEnvironment"/>
        /// </summary>
        /// <returns>The execution environment of the context in which the program is</returns>
        public static StreamExecutionEnvironment GetExecutionEnvironment(this StreamExecutionEnvironment env)
        {
            return null;
        }

        /// <summary>
        /// Creates a <see cref="LocalStreamEnvironment"/>.  
        /// The local execution environment will run the program in a multi-threaded fashion in the same CLR as the  environment was created in. The default parallelism of the local environment is the number of hardware contexts(CPU cores / threads), unless it was specified differently by  <see cref="StreamExecutionEnvironment.SetParallelism"/>.
        /// </summary>
        /// <returns>A local execution environment.</returns>
        public static LocalStreamEnvironment CreateLocalEnvironment(this StreamExecutionEnvironment env)
        {
            return null;
        }

        internal static StreamExecutionEnvironment CreateStreamExecutionEnvironment(this StreamExecutionEnvironment env)
        {   // because the streaming project depends on "flink-clients" (and not the other way around)
            // we currently need to intercept the data set environment and create a dependent stream env.
            // this should be fixed once we rework the project dependencies
            return null;
        }
    }
}
