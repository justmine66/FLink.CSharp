using System;
using FLink.Core.Configurations.Descriptions;

namespace FLink.Core.Configurations
{
    /// <summary>
    /// A <see cref="ConfigOption{T}"/> describes a configuration parameter. It encapsulates the configuration key, deprecated older versions of the key, and an optional default value for the configuration parameter.
    /// {@code ConfigOptions} are built via the <see cref="ConfigOptionBuilder"/> class. Once created, a config option is immutable.
    /// </summary>
    /// <typeparam name="T">The type of value associated with the configuration option.</typeparam>
    public class ConfigOption<T>
    {
        public static readonly Description EmptyDescription = Description.Builder().Text("").Build();

        public string Key;
        public T DefaultValue;

        /// <summary>
        /// Creates a new config option with fallback keys.
        /// </summary>
        /// <param name="key">The current key for that config option</param>
        /// <param name="clazz">describes type of the ConfigOption, see description of the clazz field</param>
        /// <param name="description">Description for that option</param>
        /// <param name="defaultValue">The default value for this option</param>
        /// <param name="isList">tells if the ConfigOption describes a list option, see description of the clazz field</param>
        /// <param name="fallbackKeys">The list of fallback keys, in the order to be checked</param>
        public ConfigOption(
            string key,
            Type clazz,
            Description description,
            T defaultValue,
            bool isList,
            params FallbackKey[] fallbackKeys)
        {
        }

        /// <summary>
        /// Creates a new config option, using this option's key and default value, and adding the given deprecated keys.
        /// </summary>
        /// <param name="deprecatedKeys">The deprecated keys, in the order in which they should be checked.</param>
        /// <returns>A new config options, with the given deprecated keys.</returns>
        public ConfigOption<T> WithDeprecatedKeys(params string[] deprecatedKeys)
        {
            return this;
        }

        /// <summary>
        /// Creates a new config option, using this option's key and default value, and adding the given description. The given description is used when generation the configuration documentation.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public ConfigOption<T> WithDescription(string description)
        {
            return this;
        }
    }
}
