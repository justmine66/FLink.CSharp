using System;
using FLink.Core.Api.Dag;
using FLink.Core.Configurations;

namespace FLink.Core.Execution
{
    /// <summary>
    /// An interface to be implemented by the entity responsible for finding the correct <see cref="IExecutor"/> to execute a given <see cref="IPipeline"/>.
    /// </summary>
    public interface IExecutorServiceLoader
    {
        /// <summary>
        /// Loads the <see cref="IExecutorFactory"/> which is compatible with the provided configuration.
        /// There can be at most one compatible factory among the available ones, otherwise an exception will be thrown.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>a compatible <see cref="IExecutorFactory"/>.</returns>
        /// <exception cref="Exception">if there is more than one compatible factories, or something went wrong when loading the registered factories.</exception>
        IExecutorFactory GetExecutorFactory(Configuration configuration);
    }
}
