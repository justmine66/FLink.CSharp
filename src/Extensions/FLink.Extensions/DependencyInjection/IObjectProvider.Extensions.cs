using System;

namespace FLink.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for getting services from an <see cref="IObjectProvider" />.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Get service of type <typeparamref name="T" /> from the <see cref="IObjectProvider" />.
        /// </summary>
        /// <typeparam name="T">The type of service object to get.</typeparam>
        /// <param name="provider">The <see cref="IObjectProvider" /> to retrieve the service object from.</param>
        /// <returns>A service object of type <typeparamref name="T" /> or null if there is no such service.</returns>
        public static T GetService<T>(this IObjectProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            return (T)provider.GetService(typeof(T));
        }
    }
}
