using FLink.Core.IO;

namespace FLink.Core.Util
{
    /// <summary>
    /// This is the abstract base class for registries that allow to register instances of <see cref="ICloseable"/>, which are all closed if this registry is closed.
    /// </summary>
    /// <typeparam name="TCloseable">Type of the closeable this registers</typeparam>
    /// <typeparam name="T">Type for potential meta data associated with the registering closeables</typeparam>
    public abstract class AbstractCloseableRegistry<TCloseable, T>: ICloseable where TCloseable: ICloseable
    {
        public void Close()
        {

        }
    }
}
