using FLink.Runtime.Events;
using FLink.Runtime.IO.Network.Buffers;

namespace FLink.Runtime.IO.Network.Partition.Consumer
{
    /// <summary>
    /// Either type for <see cref="IBuffer"/> or <see cref="AbstractEvent"/> instances tagged with the channel index, from which they were received.
    /// </summary>
    public class BufferOrEvent
    {
        public BufferOrEvent(AbstractEvent @event, int channelIndex, bool moreAvailable = true)
        {
            Event = @event;
            MoreAvailable = moreAvailable;
            ChannelIndex = channelIndex;
        }

        public BufferOrEvent(IBuffer buffer, int channelIndex, bool moreAvailable = true)
        {
            Buffer = buffer;
            MoreAvailable = moreAvailable;
            ChannelIndex = channelIndex;
        }

        public IBuffer Buffer { get; }

        public AbstractEvent Event { get; }

        /// <summary>
        /// Indicate availability of further instances for the union input gate.
        /// This is not needed outside of the input gate unioning logic and cannot be set outside of the consumer package.
        /// </summary>
        public bool MoreAvailable { get; set; }

        public int ChannelIndex { get; set; }

        public bool IsBuffer => Buffer != null;

        public bool IsEvent => Event != null;

        public override string ToString() => $"BufferOrEvent [{(IsBuffer ? Buffer.ToString() : Event.ToString())}, channelIndex = {ChannelIndex}]";
    }
}
