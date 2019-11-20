namespace FLink.Core.Configurations
{
    public class GlobalConfiguration
    {
        /// <summary>
        /// Loads the global configuration from the environment.
        /// Fails if an error occurs during loading. Returns an empty configuration object if the environment variable is not set. In production this variable is set but tests and local execution/debugging don't have this environment variable set. That's why we should fail if it is not set.
        /// </summary>
        /// <returns>Returns the Configuration</returns>
        public static Configuration LoadConfiguration()
        {
            return null;
        }
    }
}
