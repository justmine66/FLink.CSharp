using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLink.Core.Api.Common.Cache
{
    /// <summary>
    /// DistributedCache provides static methods to write the registered cache files into job configuration or decode them from job configuration. It also provides user access to the file locally.
    /// </summary>
    public class DistributedCache
    {
        /// <summary>
        /// Meta info about an entry in <see cref="DistributedCache"/>.
        /// </summary>
        public class DistributedCacheEntry
        {

        }

        private readonly IDictionary<string, TaskCompletionSource<string>> _cacheCopyTasks;

        public DistributedCache(IDictionary<string, TaskCompletionSource<string>> cacheCopyTasks)
        {
            _cacheCopyTasks = cacheCopyTasks;
        }
    }
}
