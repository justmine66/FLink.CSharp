using FLink.Core.Configurations;

namespace FLink.Runtime.State
{
    /// <summary>
    /// An interface for state backends that pick up additional parameters from a configuration.
    /// </summary>
    public interface IConfigurableStateBackend
    {
        /// <summary>
        /// Creates a variant of the state backend that applies additional configuration parameters.
        /// </summary>
        /// <param name="config">The configuration to pick the values from.</param>
        /// <returns>A reconfigured state backend.</returns>
        /// <exception cref="IllegalConfigurationException">Thrown if the configuration contained invalid entries.</exception>
        IStateBackend Configure(Configuration config);
    }
}
