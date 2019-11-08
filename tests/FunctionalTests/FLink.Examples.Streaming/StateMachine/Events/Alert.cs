using System;
using FLink.Core.Util;
using FLink.Examples.Streaming.StateMachine.DFA;

namespace FLink.Examples.Streaming.StateMachine.Events
{
    /// <summary>
    /// Data type for alerts.
    /// </summary>
    public class Alert : IEquatable<Alert>
    {
        public int Address;

        public State State;

        public EventType Transition;

        /// <summary>
        /// Creates a new alert.
        /// </summary>
        /// <param name="address">The originating address (think 32 bit IPv4 address).</param>
        /// <param name="state">The state that the event state machine found.</param>
        /// <param name="transition">The transition that was considered invalid.</param>
        public Alert(int address, State state, EventType transition)
        {
            Address = address;
            State = state;
            Transition = transition;
        }

        public bool Equals(Alert other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Address == other.Address && Equals(State, other.State) && Transition == other.Transition;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Alert)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Address;
                hashCode = (hashCode * 397) ^ (State != null ? State.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Transition;
                return hashCode;
            }
        }

        public override string ToString()=> "ALERT " + Event.FormatAddress(Address) + " : " + State.Name + " -> " + Transition.GetName();
    }
}
