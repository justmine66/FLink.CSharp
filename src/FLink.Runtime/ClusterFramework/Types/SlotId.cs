using System;

namespace FLink.Runtime.ClusterFramework.Types
{
    /// <summary>
    /// Unique identifier for a slot on a TaskManager. This ID is constant across the life time of the TaskManager.
    /// </summary>
    public class SlotId : IResourceIdRetrievable, IEquatable<SlotId>
    {
        public SlotId(ResourceId resourceIdentifier, int slotNumber)
        {
            ResourceIdentifier = resourceIdentifier;
            SlotNumber = slotNumber;
        }

        /// <summary>
        /// The resource id which this slot located
        /// </summary>
        public ResourceId ResourceIdentifier { get; }

        /// <summary>
        /// The numeric id for single slot
        /// </summary>
        public int SlotNumber { get; }

        public bool Equals(SlotId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(ResourceIdentifier, other.ResourceIdentifier) && SlotNumber == other.SlotNumber;
        }

        public override bool Equals(object obj) => obj is SlotId other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return ((ResourceIdentifier != null ? ResourceIdentifier.GetHashCode() : 0) * 397) ^ SlotNumber;
            }
        }

        public override string ToString() => $"{ResourceIdentifier}_{SlotNumber}";
    }
}
