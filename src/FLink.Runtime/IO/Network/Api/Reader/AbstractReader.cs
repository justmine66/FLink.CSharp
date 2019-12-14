using System;
using System.IO;
using FLink.Core.Exceptions;
using FLink.Core.Util;
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
        /// <summary>
        /// The task event bus to manage task event subscriptions.
        /// </summary>
        private readonly TaskEventBus _taskEventBus = new TaskEventBus();
        /// <summary>
        /// Flag indicating whether this reader allows iteration events.
        /// </summary>
        private bool _isIterative;
        /// <summary>
        /// The current number of end of superstep events (reset for each superstep).
        /// A superstep is finished after an end of superstep event has been received for each input channel.
        /// </summary>
        private int _currentNumberOfEndOfSuperStepEvents;

        protected AbstractReader(IInputGate inputGate)
        {
            InputGate = inputGate;
        }

        /// <summary>
        /// The input gate to read from.
        /// </summary>
        public IInputGate InputGate { get; }

        public virtual bool IsFinished => InputGate.IsFinished;

        public virtual void SendTaskEvent(TaskEvent @event)
        {
            _taskEventBus.Publish(@event);
        }

        public virtual void RegisterTaskEventListener(IEventListener<TaskEvent> listener, Type eventType)
        {
            _taskEventBus.Subscribe(eventType, listener);
        }

        /// <summary>
        /// Handles the event and returns whether the reader reached an end-of-stream event (either the end of the whole stream or the end of an superstep).
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        protected virtual bool HandleEvent(AbstractEvent @event)
        {
            var eventType = @event.GetType();

            try
            {
                // ------------------------------------------------------------
                // Runtime events
                // ------------------------------------------------------------

                switch (@event)
                {
                    // This event is also checked at the (single) input gate to release the respective
                    // channel, at which it was received.
                    case EndOfPartitionEvent _:
                        return true;
                    // ------------------------------------------------------------
                    // Task events (user)
                    // ------------------------------------------------------------
                    case EndOfSuperStepEvent _:
                        return IncrementEndOfSuperstepEventAndCheck();
                    case TaskEvent taskEvent:
                        _taskEventBus.Publish(taskEvent);

                        return false;
                    default:
                        throw new IllegalStateException($"Received unexpected event of type {eventType} at reader.");
                }
            }
            catch (Exception e)
            {
                throw new IOException($"Error while handling event of type {eventType}.", e);
            }
        }

        public void Publish(TaskEvent @event) => _taskEventBus.Publish(@event);

        public virtual void SetIterativeReader()
        {
            _isIterative = true;
        }

        public virtual void StartNextSuperStep()
        {
            Preconditions.CheckState(_isIterative, "Tried to start next superstep in a non-iterative reader.");
            Preconditions.CheckState(_currentNumberOfEndOfSuperStepEvents == InputGate.NumberOfInputChannels, "Tried to start next superstep before reaching end of previous superstep.");

            _currentNumberOfEndOfSuperStepEvents = 0;
        }

        public virtual bool HasReachedEndOfSuperStep => _isIterative && _currentNumberOfEndOfSuperStepEvents == InputGate.NumberOfInputChannels;

        private bool IncrementEndOfSuperstepEventAndCheck()
        {
            Preconditions.CheckState(_isIterative, "Tried to increment superstep count in a non-iterative reader.");
            Preconditions.CheckState(_currentNumberOfEndOfSuperStepEvents + 1 <= InputGate.NumberOfInputChannels,
                $"Received too many ({_currentNumberOfEndOfSuperStepEvents}) end of superstep events.");

            return ++_currentNumberOfEndOfSuperStepEvents == InputGate.NumberOfInputChannels;
        }
    }
}
