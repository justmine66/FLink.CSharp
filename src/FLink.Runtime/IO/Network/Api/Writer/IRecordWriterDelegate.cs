using FLink.Core.IO;
using FLink.Runtime.Events;

namespace FLink.Runtime.IO.Network.Api.Writer
{
    /// <summary>
    /// The record writer delegate provides the availability function for task processor, and it might represent a single <see cref="RecordWriter{TRecord}"/> or multiple <see cref="RecordWriter{TRecord}"/> instances in specific implementations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRecordWriterDelegate<T> : IAvailabilityProvider, ICloseable where T : IIOReadableWritable
    {
        /// <summary>
        /// Broadcasts the provided event to all the internal record writer instances.
        /// </summary>
        /// <param name="event">the event to be emitted to all the output channels.</param>
        void BroadcastEvent(AbstractEvent @event);

        /// <summary>
        /// Gets the internal actual record writer instance based on the output index.
        /// </summary>
        /// <param name="outputIndex">the index respective to the record writer instance.</param>
        /// <returns></returns>
        RecordWriter<T> GetRecordWriter(int outputIndex);
    }
}
