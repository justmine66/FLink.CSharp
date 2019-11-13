namespace FLink.Extensions.DependencyInjection
{
    public enum ObjectLifetime
    {
        /// <summary>
        /// Specifies that a single instance of the service will be created.
        /// </summary>
        Singleton,
        /// <summary>
        /// Specifies that a new instance of the service will be created for each scope.
        /// </summary>
        /// <remarks>
        /// A scope is created defined by user.
        /// </remarks>
        Scoped,
        /// <summary>
        /// Specifies that a new instance of the service will be created every time it is requested.
        /// </summary>
        Transient,
    }
}
