namespace FLink.Core.Util
{
    /// <summary>
    /// Collects a record and forwards it. 
    /// </summary>
    public interface ICollector<in TRecord>
    {
        /// <summary>
        /// Emits a record.
        /// </summary>
        /// <param name="record">The record to collect.</param>
        void Collect(TRecord record);

        /// <summary>
        /// Closes the collector. If any data was buffered, that data will be flushed.
        /// </summary>
        void Close();
    }
}
