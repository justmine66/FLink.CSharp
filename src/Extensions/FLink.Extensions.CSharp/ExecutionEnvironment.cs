using System;
using System.Threading;

namespace FLink.Extensions.CSharp
{
    /// <summary>
    /// The ExecutionEnvironment is the context in which a program is executed.
    /// <see cref="LocalEnvironment"/> will cause execution in the current CLR, a <see cref="RemoteEnvironment"/> will cause execution on a remote setup.
    /// The environment provides methods to control the job execution (such as setting the parallelism) and to interact with the outside world (data access).
    /// </summary>
    public class ExecutionEnvironment
    {
        private static readonly IExecutionEnvironmentFactory ContextEnvironmentFactory = null;
        private static readonly ThreadLocal<IExecutionEnvironmentFactory> ThreadLocalContextEnvironmentFactory = new ThreadLocal<IExecutionEnvironmentFactory>();

        public static ExecutionEnvironment GetExecutionEnvironment()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the parallelism with which operation are executed by default.
        /// </summary>
        public int Parallelism { get; set; }

        /// <summary>
        /// Checks whether it is currently permitted to explicitly instantiate a LocalEnvironment or a RemoteEnvironment.
        /// </summary>
        /// <returns>True, if it is possible to explicitly instantiate a LocalEnvironment or a RemoteEnvironment, false otherwise.</returns>
        public static bool AreExplicitEnvironmentsAllowed => ContextEnvironmentFactory == null && ThreadLocalContextEnvironmentFactory.Value == null;
    }
}
