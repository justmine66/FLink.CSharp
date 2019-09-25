namespace FLink.Streaming.Api.Windowing.Windows
{
    /// <summary>
    /// A <see cref="Window"/> is a grouping of elements into finite buckets. Windows have a maximum timestamp which means that, at some point, all elements that go into one window will have arrived.
    /// Subclasses should implement <code>Equals()</code> and <code>GetHashCode()</code> so that logically same windows are treated the same.
    /// </summary>
    public abstract class Window
    {
        /// <summary>
        /// Gets the largest timestamp that still belongs to this window.
        /// </summary>
        /// <returns>The largest timestamp that still belongs to this window.</returns>
        public abstract long MaxTimestamp();
    }
}
