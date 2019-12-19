using System.Threading.Tasks;
using FLink.Core.Api.Dag;
using FLink.Core.Configurations;

namespace FLink.Core.Execution
{
    /// <summary>
    /// The entity responsible for executing a <see cref="IPipeline"/>, i.e. a user job.
    /// </summary>
    public interface IExecutor
    {
        /// <summary>
        /// Executes a <see cref="IPipeline"/> based on the provided configuration and returns a <see cref="IJobClient"/> which allows to interact with the job being executed, e.g.cancel it or take a savepoint.
        /// ATTENTION: The caller is responsible for managing the lifecycle of the returned <see cref="IJobClient"/>.
        /// This means that e.g. close() should be called explicitly at the call-site.
        /// </summary>
        /// <param name="pipeline">to execute</param>
        /// <param name="configuration">with the required execution parameters</param>
        /// <returns>a <see cref="TaskCompletionSource{TResult}"/> with the <see cref="IJobClient"/> corresponding to the pipeline.</returns>
        TaskCompletionSource<IJobClient> Execute(IPipeline pipeline, Configuration configuration);
    }
}
