using FLink.Runtime.State;

namespace FLink.Runtime.Checkpoint
{
    public class TaskStateSnapshot : ICompositeStateHandle
    {
        public void DiscardState()
        {
            throw new System.NotImplementedException();
        }

        public void GetStateSize()
        {
            throw new System.NotImplementedException();
        }
    }
}
