using FLink.Core.Configurations;
using FLink.Core.Util;
using FLink.Core.Api.Common;
using FLink.Core.Exceptions;

namespace FLink.CSharp
{
    /// <summary>
    /// An <see cref="ExecutionEnvironment"/> that runs the program locally, multi-threaded, in the CLR where the environment is instantiated.
    /// When this environment is instantiated, it uses a default parallelism of 1. The default parallelism can be set via <see cref="LocalEnvironment.Parallelism"/>.
    /// Local environments can also be instantiated through <see cref="ExecutionEnvironment.CreateLocalEnvironment()"/>. The former version will pick a default parallelism equal to the number of hardware contexts in the local machine. 
    /// </summary>
    public class LocalEnvironment : ExecutionEnvironment
    {
        private readonly Configuration _configuration;

        /// <summary>
        /// Creates a new local environment.
        /// </summary>
        public LocalEnvironment()
            : this(new Configuration())
        {
        }

        /// <summary>
        /// Creates a new local environment that configures its local executor with the given configuration. 
        /// </summary>
        /// <param name="configuration">The configuration used to configure the local executor.</param>
        public LocalEnvironment(Configuration configuration)
        {
            if (!ExecutionEnvironment.AreExplicitEnvironmentsAllowed)
            {
                throw new InvalidProgramException(
                    "The LocalEnvironment cannot be instantiated when running in a pre-defined context " +
                    "(such as Command Line Client, Scala Shell, or TestEnvironment)");
            }

            _configuration = Preconditions.CheckNotNull(configuration);
        }

        public override JobExecutionResult Execute(string jobName)
        {
            return null;
        }

        public override string ToString() => "Local Environment (parallelism = " + (Parallelism == ExecutionConfig.DefaultParallelism ? "default" : Parallelism.ToString()) + ").";
    }
}
