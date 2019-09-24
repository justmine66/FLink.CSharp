using FLink.Core.Api.Common;

namespace FLink.Streaming.Api.Environment
{
    /// <summary>
    /// The ExecutionEnvironment is the context in which a program is executed.
    /// </summary>
    public abstract class ExecutionEnvironment
    {
        /// <summary>
        /// Result from the latest execution, to make it retrievable when using eager execution methods.
        /// </summary>
        protected JobExecutionResult JobExecutionResult { get; }

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
    }
}
