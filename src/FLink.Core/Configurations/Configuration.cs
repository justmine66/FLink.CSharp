using System;

namespace FLink.Core.Configurations
{
    public class Configuration
    {
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
    }
}
