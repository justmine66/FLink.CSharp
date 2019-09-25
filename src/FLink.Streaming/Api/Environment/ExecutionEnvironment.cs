using FLink.Core.Api.Common;

namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// The ExecutionEnvironment is the context in which a program is executed.
    /// The environment provides methods to control the job execution (such as setting the parallelism) and to interact with the outside world (data access).
    /// </summary>
    public abstract class ExecutionEnvironment
    {
        /// <summary>
        /// Result from the latest execution, to make it retrievable when using eager execution methods.
        /// </summary>
        protected JobExecutionResult LastJobExecutionResult { get; set; }

        /// <summary>
        /// Creates a new Execution Environment.
        /// </summary>
        protected ExecutionEnvironment() { }

        /// <summary>
        /// Creates an execution environment that represents the context in which the program is currently executed.
        /// </summary>
        /// <returns></returns>
        public static ExecutionEnvironment GetExecutionEnvironment()
        {
            return null;
        }

        /// <summary>
        /// Creates a <see cref="LocalStreamEnvironment"/>.
        /// The local execution environment will run the program in a multi-threaded fashion in the same CLR as the environment was created in. It will use the parallelism specified in the parameter.
        /// </summary>
        /// <param name="parallelism">The parallelism for the local environment.</param>
        /// <returns>A local execution environment with the specified parallelism.</returns>
        public static LocalStreamEnvironment CreateLocalEnvironment(int parallelism)
        {
            return null;
        }
    }
}
