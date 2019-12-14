using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FLink.Runtime.Events;
using FLink.Runtime.Utils.Event;

namespace FLink.Runtime.IO.Network.Api
{
    /// <summary>
    /// The event handler manages <see cref="IEventListener{T}"/> instances and allows to publish events to them.
    /// </summary>
    public class TaskEventBus
    {
        /// <summary>
        /// Listeners for each event type.
        /// </summary>
        private readonly IDictionary<Type, IEventListener<TaskEvent>[]> _listeners = new ConcurrentDictionary<Type, IEventListener<TaskEvent>[]>();

        public void Subscribe(Type eventType, params IEventListener<TaskEvent>[] listeners)
        {
            _listeners.TryAdd(eventType, listeners);
        }

        /// <summary>
        /// Publishes the task event to all subscribed event listeners.
        /// </summary>
        /// <param name="event">The event to publish.</param>
        public void Publish(TaskEvent @event)
        {
            foreach (var listener in _listeners[@event.GetType()])
            {
                listener.OnEvent(@event);
            }
        }
    }
}
