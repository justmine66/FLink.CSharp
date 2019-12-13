using System;
using FLink.Runtime.Events;
using FLink.Runtime.Utils.Event;

namespace FLink.Runtime.IO.Network.Api.Reader
{
    /// <summary>
    /// The basic API for every reader.
    /// </summary>
    public interface IReaderBase
    {
        /// <summary>
        /// Returns whether the reader has consumed the input.
        /// </summary>
        bool IsFinished { get; }

        void SendTaskEvent(TaskEvent @event);

        void RegisterTaskEventListener(IEventListener<TaskEvent> listener, Type eventType);

        void SetIterativeReader();

        void StartNextSuperStep();

        bool HasReachedEndOfSuperStep { get; }
    }
}
