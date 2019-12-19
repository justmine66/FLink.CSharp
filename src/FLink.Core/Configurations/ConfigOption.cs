using System;
using System.Linq;
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
        private static readonly FallbackKey[] Empty = new FallbackKey[0];

        public static readonly Description EmptyDescription = Description.Builder().Text("").Build();

        /// <summary>
        /// The current key for that config option. 
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The default value for this option.
        /// </summary>
        public T DefaultValue { get; }

        /// <summary>
        /// True if it has a default value, false if not.
        /// Checks if this option has a default value.
        /// </summary>
        public bool HasDefaultValue => DefaultValue != null;

        /// <summary>
        /// The description for this option.
        /// </summary>
        public Description Description { get; }

        /// <summary>
        /// Type of the value that this ConfigOption describes.
        /// </summary>
        private readonly Type _type;
        private readonly bool _isList;
        /// <summary>
        /// The list of deprecated keys, in the order to be checked.
        /// </summary>
        private readonly FallbackKey[] _fallbackKeys;

        /// <summary>
        /// True if the option has deprecated keys, false if not.
        /// Checks whether this option has deprecated keys.
        /// </summary>
        public bool HasDeprecatedKeys => _fallbackKeys != Empty && _fallbackKeys.Any(it => it.IsDeprecated);

        /// <summary>
        /// True if the option has fallback keys, false if not.
        /// Checks whether this option has fallback keys.
        /// </summary>
        public bool HasFallbackKeys => _fallbackKeys != Empty && _fallbackKeys.Any();

        /// <summary>
        /// Creates a new config option with fallback keys.
        /// </summary>
        /// <param name="key">The current key for that config option</param>
        /// <param name="type">describes type of the ConfigOption, see description of the clazz field</param>
        /// <param name="description">Description for that option</param>
        /// <param name="defaultValue">The default value for this option</param>
        /// <param name="isList">tells if the ConfigOption describes a list option, see description of the clazz field</param>
        /// <param name="fallbackKeys">The list of fallback keys, in the order to be checked</param>
        public ConfigOption(
            string key,
            Type type,
            Description description,
            T defaultValue,
            bool isList,
            params FallbackKey[] fallbackKeys)
        {
            Key = key;
            _type = type;
            Description = description;
            DefaultValue = defaultValue;
            _isList = isList;
            _fallbackKeys = fallbackKeys;
        }

        /// <summary>
        /// Creates a new config option, using this option's key and default value, and adding the given fallback keys.
        /// </summary>
        /// <param name="fallbackKeys">The fallback keys, in the order in which they should be checked.</param>
        /// <returns>A new config options, with the given fallback keys.</returns>
        public ConfigOption<T> WithFallbackKeys(params string[] fallbackKeys)
        {
            return this;
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

        /// <summary>
        /// Creates a new config option, using this option's key and default value, and adding the given description.The given description is used when generation the configuration documention.
        /// </summary>
        /// <param name="description">The description for this option.</param>
        /// <returns>A new config option, with given description.</returns>
        public ConfigOption<T> WithDescription(Description description)
        {
            return this;
        }
    }
}
