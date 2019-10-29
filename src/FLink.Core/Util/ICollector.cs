namespace FLink.Core.Util
{
    /// <summary>
    /// Collects a record and forwards it. 
    /// </summary>
    public interface ICollector<in TElement>
    {
        /// <summary>
        /// Emits a record.
        /// </summary>
        /// <param name="element">The record to collect.</param>
        void Collect(TElement element);

        /// <summary>
        /// Closes the collector. If any data was buffered, that data will be flushed.
        /// </summary>
        void Close();
    }
}
