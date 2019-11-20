using FLink.Core.Api.Common;
using FLink.Core.Configurations;
using FLink.Extensions.CSharp;
using FLink.Streaming.Api.Graph;

namespace FLink.Streaming.Api.Environment
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
        {
            if (!ExecutionEnvironment.AreExplicitEnvironmentsAllowed)
            {
                throw new InvalidProgramException(
                    "The LocalStreamEnvironment cannot be used when submitting a program through a client, " +
                    "or running in a TestEnvironment context.");
            }

            _configuration = configuration;
            SetParallelism(1);
        }

        /// <summary>
        /// Executes the JobGraph of the on a mini cluster of ClusterUtil with a user specified name.
        /// </summary>
        /// <param name="streamGraph"></param>
        /// <returns>The result of the job execution, containing elapsed time and accumulators.</returns>
        public override JobExecutionResult Execute(StreamGraph streamGraph)
        {
            throw new System.NotImplementedException();
        }
    }
}
