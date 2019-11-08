using System;
using FLink.Core.Util;

namespace FLink.Examples.Streaming.StateMachine.Events
{
    /// <summary>
    /// Data type for events, consisting of the originating IP address and an event type.
    /// </summary>
    public class Event : IEquatable<Event>
    {
        public EventType Type;

        public int SourceAddress;

        public Event(EventType type, int sourceAddress)
        {
            Type = type;
            SourceAddress = sourceAddress;
        }

        public bool Equals(Event other) =>
            other != null && (Type == other.Type && SourceAddress == other.SourceAddress);

        public override bool Equals(object obj) => obj is Event other && Equals(other);

        public override int GetHashCode() => 31 * Type.GetHashCode() + SourceAddress;

        public override string ToString() => "Event " + FormatAddress(SourceAddress) + " : " + Type.GetName();

        /// <summary>
        /// Util method to create a string representation of a 32 bit integer representing an IPv4 address. 
        /// </summary>
        /// <param name="address">The address, MSB first.</param>
        /// <returns>The IP address string.</returns>
        public static string FormatAddress(int address)
        {
            var b1 = (address >> 24) & 0xff;
            var b2 = (address >> 16) & 0xff;
            var b3 = (address >> 8) & 0xff;
            var b4 = address & 0xff;

            return "" + b1 + '.' + b2 + '.' + b3 + '.' + b4;
        }
    }
}
