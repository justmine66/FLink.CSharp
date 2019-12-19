using FLink.Core.Configurations;

namespace FLink.Core.Execution
{
    /// <summary>
    /// A factory for selecting and instantiating the adequate <see cref="IExecutor"/> based on a provided <see cref="Configuration"/>.
    /// </summary>
    public interface IExecutorFactory
    {
        /// <summary>
        /// Returns true if this factory is compatible with the options in the provided configuration, false otherwise.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        bool IsCompatibleWith(Configuration configuration);

        /// <summary>
        /// Instantiates an <see cref="IExecutor"/> compatible with the provided configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>the executor instance.</returns>
        IExecutor GetExecutor(Configuration configuration);
    }
}
