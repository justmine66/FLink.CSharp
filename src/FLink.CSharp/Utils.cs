using System.Threading;

namespace FLink.CSharp
{
    public class Utils
    {
        public static string GetCallLocationName() => GetCallLocationName(4);

        public static string GetCallLocationName(int depth)
        {
            return string.Empty;
        }

        /// <summary>
        /// Resolves the given factories. The thread local factory has preference over the static factory.
        /// </summary>
        /// <typeparam name="TFactory">type of factory</typeparam>
        /// <param name="threadLocalFactory">containing the thread local factory</param>
        /// <param name="staticFactory"></param>
        /// <returns>the resolved factory if it exists</returns>
        public static TFactory ResolveFactory<TFactory>(ThreadLocal<TFactory> threadLocalFactory, TFactory staticFactory = default)
        {
            var localFactory = threadLocalFactory.Value;
            var factory = localFactory == null ? staticFactory : localFactory;
            return factory;
        }
    }
}
