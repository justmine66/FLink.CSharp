using System.Collections.Generic;

namespace FLink.Core.Util
{
    public class SingletonList<T>
    {
        private SingletonList() { }

        public static IList<T> Instance = new List<T>();
    }
}
