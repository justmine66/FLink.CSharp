using FLink.Runtime.Events;

namespace FLink.Runtime.IO.Network.Partition.Consumer
{
    /// <summary>
    /// An input gate consumes one or more partitions of a single produced intermediate result.
    /// Each intermediate result is partitioned over its producing parallel subtasks;
    /// each of these partitions is furthermore partitioned into one or more subpartitions.
    /// </summary>
    public interface IInputGate
    {
        int NumberOfInputChannels { get; }

        string OwningTaskName { get; }

        bool IsFinished { get; }

        void RequestPartitions();

        /// <summary>
        /// Blocking call waiting for next <see cref="BufferOrEvent"/>.
        /// </summary>
        BufferOrEvent NextBufferOrEvent { get; }

        BufferOrEvent PollNextBufferOrEvent();

        void SendTaskEvent(TaskEvent @event);

        void RegisterListener(IInputGateListener listener);

        int PageSize { get; }
    }
}
