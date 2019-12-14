using System.IO;
using FLink.Core.Exceptions;
using FLink.Core.IO;
using FLink.Runtime.IO.Network.Api.Serialization;
using FLink.Runtime.IO.Network.Partition.Consumer;

namespace FLink.Runtime.IO.Network.Api.Reader
{
    /// <summary>
    /// A record-oriented reader.
    /// This abstract base class is used by both the mutable and immutable record readers.
    /// </summary>
    /// <typeparam name="T">The type of the record that can be read with this record reader.</typeparam>
    public abstract class AbstractRecordReader<T> : AbstractReader where T : IIOReadableWritable
    {
        private readonly IRecordDeserializer<T>[] _recordDeserializers;

        private IRecordDeserializer<T> _currentRecordDeserializer;

        private bool _isFinished;

        /// <summary>
        /// Creates a new AbstractRecordReader that de-serializes records from the given input gate and can spill partial records to disk, if they grow large.
        /// </summary>
        /// <param name="inputGate">The input gate to read from.</param>
        /// <param name="tmpDirectories">The temp directories. USed for spilling if the reader concurrently reconstructs multiple large records.</param>
        protected AbstractRecordReader(IInputGate inputGate, string[] tmpDirectories)
            : base(inputGate)
        {
            // Initialize one deserializer per input channel
            _recordDeserializers = new IRecordDeserializer<T>[inputGate.NumberOfInputChannels];
            for (var i = 0; i < _recordDeserializers.Length; i++)
            {
                _recordDeserializers[i] = new SpillingAdaptiveSpanningRecordDeserializer<T>(tmpDirectories);
            }
        }

        protected bool GetNextRecord(T target)
        {
            if (_isFinished)
            {
                return false;
            }

            while (true)
            {
                if (_currentRecordDeserializer != null)
                {
                    var result = _currentRecordDeserializer.GetNextRecord(target);

                    if (result.IsBufferConsumed)
                    {
                        var currentBuffer = _currentRecordDeserializer.Buffer;

                        currentBuffer.RecycleBuffer();
                        _currentRecordDeserializer = null;
                    }

                    if (result.IsFullRecord)
                    {
                        return true;
                    }
                }

                var bufferOrEvent = InputGate.NextBufferOrEvent ?? throw new IllegalStateException();

                if (bufferOrEvent.IsBuffer)
                {
                    _currentRecordDeserializer = _recordDeserializers[bufferOrEvent.ChannelIndex];
                    _currentRecordDeserializer.Buffer = bufferOrEvent.Buffer;
                }
                else
                {
                    // sanity check for leftover data in deserializers. events should only come between
                    // records, not in the middle of a fragment
                    if (_recordDeserializers[bufferOrEvent.ChannelIndex].HasUnfinishedData)
                    {
                        throw new IOException(
                            "Received an event in channel " + bufferOrEvent.ChannelIndex + " while still having "
                            + "data from a record. This indicates broken serialization logic. "
                            + "If you are using custom serialization code (Writable or Value types), check their "
                            + "serialization routines. In the case of Kryo, check the respective Kryo serializer.");
                    }

                    if (!HandleEvent(bufferOrEvent.Event)) continue;

                    if (InputGate.IsFinished)
                    {
                        _isFinished = true;
                        return false;
                    }

                    if (HasReachedEndOfSuperStep)
                    {
                        return false;
                    }

                    // else: More data is coming...
                }
            }
        }

        public void ClearBuffers()
        {
            foreach (var deserializer in _recordDeserializers)
            {
                var buffer = deserializer.Buffer;
                if (buffer != null && !buffer.IsRecycled)
                {
                    buffer.RecycleBuffer();
                }

                deserializer.Clear();
            }
        }
    }
}
