using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.IO;
using FLink.Core.Memory;

namespace FLink.Core.Configurations
{
    /// <summary>
    /// Lightweight configuration object which stores key/value pairs.
    /// </summary>
    public class Configuration : GlobalJobParameters, IIOReadableWritable, ICloneable, IWritableConfig, IReadableConfig
    {
        public IDictionary<string, object> ConfData { get; }

        /// <summary>
        /// Creates a new empty configuration.
        /// </summary>
        public Configuration() => ConfData = new Dictionary<string, object>();

        /// <summary>
        /// Creates a new configuration with the copy of the given configuration.
        /// </summary>
        /// <param name="other">The configuration to copy the entries from.</param>
        public Configuration(Configuration other) => ConfData = new ConcurrentDictionary<string, object>();

        public string GetString(ConfigOption<string> option)
        {
            throw new NotImplementedException();
        }

        public int GetInt(ConfigOption<int> option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the value associated with the given key as a bool.
        /// </summary>
        /// <param name="key">the key pointing to the associated value</param>
        /// <param name="defaultValue">the default value which is returned in case there is no value associated with the given key</param>
        /// <returns>the (default) value associated with the given key</returns>
        public bool GetBool(string key, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public long GetLong(string key, long defaultValue)
        {
            throw new NotImplementedException();
        }

        public long GetLong(ConfigOption<long> configOption)
        {
            throw new NotImplementedException();
        }

        public void Write(IDataOutputView output)
        {
            throw new NotImplementedException();
        }

        public void Read(IDataInputView input)
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IWritableConfig Set<T>(ConfigOption<T> option, T value)
        {
            SetValueInternal(option.Key, value);

            return this;
        }

        public T Get<T>(ConfigOption<T> option)
        {
            throw new NotImplementedException();
        }

        private void SetValueInternal<T>(string key, T value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            ConfData.Add(key, value);
        }
    }
}
