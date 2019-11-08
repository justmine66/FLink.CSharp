using System;
using FLink.Core.Util;
using FLink.Examples.Streaming.StateMachine.Events;

namespace FLink.Examples.Streaming.StateMachine.DFA
{
    /// <summary>
    /// A possible transition on a given event into a target state. The transition belongs to its originating state and has an associated probability that is used to generate random transition events.
    /// </summary>
    public class Transition : IEquatable<Transition>
    {
        /// <summary>
        /// The event that triggers the transition.
        /// </summary>
        public EventType EventType;

        /// <summary>
        /// The target state after the transition.
        /// </summary>
        public State TargetState;

        /// <summary>
        /// The probability of the transition.
        /// </summary>
        public float Probability;

        /// <summary>
        /// Creates a new transition.
        /// </summary>
        /// <param name="eventType">The event type that triggers the transition.</param>
        /// <param name="targetState">The target state after the transition.</param>
        /// <param name="probability">The probability of the transition.</param>
        public Transition(EventType eventType, State targetState, float probability)
        {
            EventType = eventType;
            TargetState = targetState;
            Probability = probability;
        }

        public bool Equals(Transition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EventType == other.EventType && TargetState == other.TargetState && Probability.Equals(other.Probability);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Transition)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)EventType;
                hashCode = (hashCode * 397) ^ TargetState.GetHashCode();
                hashCode = (hashCode * 397) ^ Probability.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()=> "--[" + EventType.GetName() + "]--> " + TargetState.Name + " (" + Probability + ')';
    }
}
