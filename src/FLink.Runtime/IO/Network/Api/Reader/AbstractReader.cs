using System;
using FLink.Runtime.Events;
using FLink.Runtime.IO.Network.Partition.Consumer;
using FLink.Runtime.Utils.Event;

namespace FLink.Runtime.IO.Network.Api.Reader
{
    /// <summary>
    /// A basic reader implementation, which wraps an input gate and handles events.
    /// </summary>
    public abstract class AbstractReader : IReaderBase
    {
        protected AbstractReader(IInputGate inputGate)
        {
            InputGate = inputGate;
        }

        /// <summary>
        /// The input gate to read from.
        /// </summary>
        public IInputGate InputGate { get; }

        public virtual bool IsFinished => InputGate.IsFinished;

        public void SendTaskEvent(TaskEvent @event)
        {
            throw new NotImplementedException();
        }

        public void RegisterTaskEventListener(IEventListener<TaskEvent> listener, Type eventType)
        {
            throw new NotImplementedException();
        }

        public void SetIterativeReader()
        {
            throw new NotImplementedException();
        }

        public void StartNextSuperStep()
        {
            throw new NotImplementedException();
        }

        public bool HasReachedEndOfSuperStep { get; }
    }
}
