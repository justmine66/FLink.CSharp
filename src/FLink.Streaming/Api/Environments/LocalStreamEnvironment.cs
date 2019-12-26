using FLink.Core.Configurations;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.CSharp;

namespace FLink.Streaming.Api.Environments
{
    /// <summary>
    /// The LocalStreamEnvironment is a StreamExecutionEnvironment that runs the program locally, multi-threaded, in the CLR where the environment is instantiated. It spawns an embedded Flink cluster in the background and executes the program on that cluster.
    /// </summary>
    public class LocalStreamEnvironment : StreamExecutionEnvironment
    {
        private readonly Configuration _configuration;

        public LocalStreamEnvironment()
            : this(new Configuration()) { }

        public LocalStreamEnvironment(Configuration configuration) 
            : base(ValidateAndGetConfiguration(configuration))
        {
            if (!ExecutionEnvironment.AreExplicitEnvironmentsAllowed)
            {
                throw new InvalidProgramException("The LocalStreamEnvironment cannot be used when submitting a program through a client, or running in a TestEnvironment context.");
            }

            _configuration = configuration;
            SetParallelism(1);
        }

        private static Configuration ValidateAndGetConfiguration(Configuration configuration)
        {
            if (!ExecutionEnvironment.AreExplicitEnvironmentsAllowed)
            {
                throw new InvalidProgramException("The LocalStreamEnvironment cannot be used when submitting a program through a client, or running in a TestEnvironment context.");
            }

            var effectiveConfiguration = new Configuration(Preconditions.CheckNotNull(configuration));

            effectiveConfiguration.Set(DeploymentOptions.Target, "local");
            effectiveConfiguration.Set(DeploymentOptions.Attached, true);

            return effectiveConfiguration;
        }
    }
}
