using FLink.Core.Util;

namespace FLink.Runtime.ClusterFramework.Types
{
    /// <summary>
    /// Unique identifier for a physical slot allocated by a JobManager via the ResourceManager from a TaskManager.The ID is assigned once the JobManager (or its SlotPool) first requests the slot and is constant across retries.
    /// </summary>
    public class AllocationId : AbstractId
    {
        public AllocationId() : base() { }

        /// <summary>
        /// Constructs a new AllocationID with the given parts.
        /// </summary>
        /// <param name="lowerPart">the lower bytes of the ID</param>
        /// <param name="upperPart">the higher bytes of the ID</param>
        public AllocationId(long lowerPart, long upperPart)
            : base(lowerPart, upperPart)
        { }
    }
}
