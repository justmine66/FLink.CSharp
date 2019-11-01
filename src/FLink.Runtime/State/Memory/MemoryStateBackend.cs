using FLink.Core.Configurations;
using FLink.Runtime.State.FileSystem;

namespace FLink.Runtime.State.Memory
{
    /// <summary>
    /// This state backend holds the working state in the memory (JVM heap) of the TaskManagers. The state backend checkpoints state directly to the JobManager's memory (hence the backend's name), but the checkpoints will be persisted to a file system for high-availability setups and savepoints. The MemoryStateBackend is consequently a FileSystem-based backend that can work without a file system dependency in simple setups.
    /// </summary>
    public class MemoryStateBackend : AbstractFileStateBackend, IConfigurableStateBackend
    {
        public IStateBackend Configure(Configuration config)
        {
            throw new System.NotImplementedException();
        }
    }
}
