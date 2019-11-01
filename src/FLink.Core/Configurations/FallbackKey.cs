using System;

namespace FLink.Core.Configurations
{
    /// <summary>
    /// A key with FallbackKeys will fall back to the FallbackKeys if it itself is not configured.
    /// </summary>
    public class FallbackKey : IEquatable<FallbackKey>
    {
        public string Key;

        public bool IsDeprecated;

        private FallbackKey(string key, bool isDeprecated)
        {
            Key = key;
            IsDeprecated = isDeprecated;
        }

        #region [ Override Methods ]

        public bool Equals(FallbackKey other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Key, other.Key) && IsDeprecated == other.IsDeprecated;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FallbackKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Key != null ? Key.GetHashCode() : 0) * 397) ^ IsDeprecated.GetHashCode();
            }
        }

        #endregion

        #region [ Factory methods ]

        public static FallbackKey CreateFallbackKey(string key) => new FallbackKey(key, false);

        public static FallbackKey CreateDeprecatedKey(string key) => new FallbackKey(key, true);

        #endregion
    }
}
