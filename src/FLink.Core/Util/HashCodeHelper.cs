using System.Collections.Generic;

namespace FLink.Core.Util
{
    public static class HashCodeHelper
    {
        public static int GetHashCode(IEnumerable<object> entries)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in entries)
                    hash = hash * 23 + (obj?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public static int GetHashCode(params byte[] data)
        {
            unchecked
            {
                var result = 0;
                foreach (byte b in data)
                    result = (result * 31) ^ b;
                return result;
            }
        }
    }
}
