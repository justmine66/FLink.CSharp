namespace FLink.Runtime.State
{
    /// <summary>
    /// A storage location for one particular checkpoint, offering data persistent, metadata persistence, and lifecycle/cleanup methods.
    /// </summary>
    public interface ICheckpointStorageLocation : ICheckpointStreamFactory
    {

    }
}
