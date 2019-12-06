using FLink.Core.IO;
using FLink.Runtime.Instance;

namespace FLink.Runtime.ResourceManagers.SlotManagers
{
    /// <summary>
    /// The slot manager is responsible for maintaining a view on all registered task manager slots, their allocation and all pending slot requests. Whenever a new slot is registered or and allocated slot is freed, then it tries to fulfill another pending slot request.
    /// </summary>
    public interface ISlotManager : ICloseable
    {
        int NumberRegisteredSlots { get; }

        int GetNumberRegisteredSlotsOf(InstanceId instanceId);

        int NumberFreeSlots { get; }

        int GetNumberFreeSlotsOf(InstanceId instanceId);

        int NumberPendingTaskManagerSlots { get; }

        int NumberPendingSlotRequests { get; }

        /// <summary>
        /// Suspends the component. This clears the internal state of the slot manager.
        /// </summary>
        void Suspend();
    }
}
