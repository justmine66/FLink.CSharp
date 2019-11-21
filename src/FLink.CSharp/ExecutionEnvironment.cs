using FLink.Core.Api.Common;
using FLink.Core.Configurations;
using System;
using System.Threading;

namespace FLink.CSharp
{
    /// <summary>
    /// The ExecutionEnvironment is the context in which a program is executed.
    /// <see cref="LocalEnvironment"/> will cause execution in the current CLR, a <see cref="RemoteEnvironment"/> will cause execution on a remote setup.
    /// The environment provides methods to control the job execution (such as setting the parallelism) and to interact with the outside world (data access).
    /// Please note that the execution environment needs strong type information for the input and return types of all operations that are executed. This means that the environments needs to know that the return value of an operation is for example a Tuple of String and Integer.
    /// </summary>
    public class ExecutionEnvironment
    {
        private static readonly IExecutionEnvironmentFactory ContextEnvironmentFactory = null;
        private static readonly ThreadLocal<IExecutionEnvironmentFactory> ThreadLocalContextEnvironmentFactory = new ThreadLocal<IExecutionEnvironmentFactory>();

        // The default parallelism used by local environments.
        private static int _defaultLocalParallelism = Environment.ProcessorCount;
        private static readonly string DefaultName = "Flink CSharp Job at " + DateTimeOffset.Now;

        public static ExecutionEnvironment GetExecutionEnvironment()
        {
            var factory = Utils.ResolveFactory(ThreadLocalContextEnvironmentFactory, ContextEnvironmentFactory);
            var environment = factory?.CreateExecutionEnvironment() ?? CreateLocalEnvironment();
            return environment;
        }

        /// <summary>
        /// Creates a <see cref="LocalEnvironment"/>.
        /// The local execution environment will run the program in a multi-threaded fashion in the same CLR as the environment was created in. The default parallelism of the local environment is the number of hardware contexts (CPU cores / threads), unless it was specified differently by <see cref="_defaultLocalParallelism"/>.
        /// </summary>
        /// <returns></returns>
        public static LocalEnvironment CreateLocalEnvironment() => CreateLocalEnvironment(_defaultLocalParallelism);

        /// <summary>
        /// Creates a <see cref="LocalEnvironment"/>.
        /// The local execution environment will run the program in a multi-threaded fashion in the same CLR as the environment was created in. It will use the parallelism specified in the parameter.
        /// </summary>
        /// <param name="parallelism">The parallelism for the local environment.</param>
        /// <returns>A local execution environment with the specified parallelism.</returns>
        public static LocalEnvironment CreateLocalEnvironment(int parallelism) =>
            CreateLocalEnvironment(new Configuration(), parallelism);

        /// <summary>
        /// Creates a <see cref="LocalEnvironment"/> which is used for executing Flink jobs.
        /// </summary>
        /// <param name="configuration">to start the <see cref="LocalEnvironment"/> with</param>
        /// <param name="defaultParallelism">to initialize the <see cref="LocalEnvironment"/> with</param>
        /// <returns>The <see cref="LocalEnvironment"/>.</returns>
        private static LocalEnvironment CreateLocalEnvironment(Configuration configuration, int defaultParallelism)
        {
            var localEnvironment = new LocalEnvironment(configuration);

            if (defaultParallelism > 0)
                localEnvironment.Parallelism = defaultParallelism;

            return localEnvironment;
        }

        /// <summary>
        /// Gets the parallelism with which operation are executed by default.
        /// </summary>
        public int Parallelism { get; set; }

        /// <summary>
        /// Gets and sets the default parallelism that will be used for the local execution environment.
        /// </summary>
        public static int LocalParallelism
        {
            get => _defaultLocalParallelism;
            set => _defaultLocalParallelism = value;
        }

        /// <summary>
        /// Checks whether it is currently permitted to explicitly instantiate a LocalEnvironment or a RemoteEnvironment.
        /// </summary>
        /// <returns>True, if it is possible to explicitly instantiate a LocalEnvironment or a RemoteEnvironment, false otherwise.</returns>
        public static bool AreExplicitEnvironmentsAllowed => ContextEnvironmentFactory == null && ThreadLocalContextEnvironmentFactory.Value == null;

        /// <summary>
        /// Triggers the program execution. The environment will execute all parts of the program that have resulted in a "sink" operation.
        /// The program execution will be logged and displayed with the given job name.
        /// </summary>
        /// <returns>The result of the job execution, containing elapsed time and accumulators.</returns>
        /// <exception cref="Exception">Exception Thrown, if the program executions fails.</exception>
        public JobExecutionResult Execute() => Execute(DefaultName);

        /// <summary>
        /// Triggers the program execution. The environment will execute all parts of the program that have resulted in a "sink" operation.
        /// The program execution will be logged and displayed with the given job name.
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns>The result of the job execution, containing elapsed time and accumulators.</returns>
        /// <exception cref="Exception">Exception Thrown, if the program executions fails.</exception>
        public virtual JobExecutionResult Execute(string jobName)
        {
            return null;
        }
    }
}
