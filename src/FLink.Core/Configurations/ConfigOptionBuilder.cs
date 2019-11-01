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

            public ConfigOption<string> NoDefaultValue()
            {
                return new ConfigOption<string>(
                    _key,
                    typeof(string),
                    ConfigOption<string>.EmptyDescription,
                    null,
                    false);
            }
        }
    }
}
