using System;
using System.Collections.Generic;
using FLink.Core.Configurations.Descriptions;
using FLink.Core.Util;

namespace FLink.Core.Configurations
{
    /// <summary>
    /// <see cref="ConfigOptionBuilder"/> are used to build a <see cref="ConfigOption{T}"/>.
    /// </summary>
    public class ConfigOptionBuilder
    {
        public static OptionBuilder Key(string key)
        {
            Preconditions.CheckNotNull(key);

            return new OptionBuilder(key);
        }

        public class OptionBuilder
        {
            private readonly string _key;

            public OptionBuilder(string key)
            {
                _key = key;
            }

            public ConfigOption<string> NoDefaultValue() => new ConfigOption<string>(_key, typeof(string),
                ConfigOption<string>.EmptyDescription, null, false);

            public ConfigOption<T> DefaultValue<T>(T value)
            {
                Preconditions.CheckNotNull(value);

                return new ConfigOption<T>(
                    _key,
                    typeof(T),
                    ConfigOption<T>.EmptyDescription,
                    value,
                    false);
            }

            public TypedConfigOptionBuilder<bool> BoolType => new TypedConfigOptionBuilder<bool>(_key, typeof(bool));
            public TypedConfigOptionBuilder<string> StringType => new TypedConfigOptionBuilder<string>(_key, typeof(string));
            public TypedConfigOptionBuilder<int> IntType => new TypedConfigOptionBuilder<int>(_key, typeof(int));
            public TypedConfigOptionBuilder<long> LongType => new TypedConfigOptionBuilder<long>(_key, typeof(long));
            public TypedConfigOptionBuilder<double> DoubleType => new TypedConfigOptionBuilder<double>(_key, typeof(double));
            public TypedConfigOptionBuilder<float> FloatType => new TypedConfigOptionBuilder<float>(_key, typeof(float));
            public TypedConfigOptionBuilder<decimal> DecimalType => new TypedConfigOptionBuilder<decimal>(_key, typeof(decimal));
            public TypedConfigOptionBuilder<IDictionary<string, string>> MapType => new TypedConfigOptionBuilder<IDictionary<string, string>>(_key, typeof(IDictionary<string, string>));
        }

        /// <summary>
        /// Builder for <see cref="Configuration"/> with a defined atomic type.
        /// </summary>
        /// <typeparam name="T">atomic type of the option</typeparam>
        public class TypedConfigOptionBuilder<T>
        {
            private readonly string _key;
            private readonly Type _type;

            public TypedConfigOptionBuilder(string key, Type type)
            {
                _key = key;
                _type = type;
            }

            public ConfigOption<T> DefaultValue(T value) =>
                new ConfigOption<T>(
                    _key,
                    _type,
                    ConfigOption<T>.EmptyDescription,
                    value,
                    false);

            public ConfigOption<T> NoDefaultValue() =>
                new ConfigOption<T>(
                    _key,
                    _type,
                    Description.Builder().Text("").Build(),
                    default,
                    false);
        }
    }
}
