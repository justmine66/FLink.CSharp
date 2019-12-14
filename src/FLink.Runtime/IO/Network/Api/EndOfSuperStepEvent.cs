using System;
using FLink.Core.Memory;
using FLink.Runtime.Events;

namespace FLink.Runtime.IO.Network.Api
{
    /// <summary>
    /// Marks the end of a superstep of one particular iteration superstep.
    /// </summary>
    public class EndOfSuperStepEvent : RuntimeEvent, IEquatable<EndOfPartitionEvent>
    {
        /// <summary>
        /// The singleton instance of this event.
        /// </summary>
        public static readonly EndOfSuperStepEvent Instance = new EndOfSuperStepEvent();

        // not instantiable
        private EndOfSuperStepEvent() { }

        public override void Write(IDataOutputView output)
        {
            // Nothing to do here
        }

        public override void Read(IDataInputView input)
        {
            // Nothing to do here
        }

        public bool Equals(EndOfPartitionEvent other) => other != null && other.GetType() == GetType();

        public override bool Equals(object obj) => obj is EndOfPartitionEvent other && Equals(other);

        public override int GetHashCode() => 1965146673;
    }
}
