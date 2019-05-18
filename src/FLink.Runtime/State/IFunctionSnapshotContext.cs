namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface provides a context in which user functions that use managed state (i.e. state that is managed by state backends) can participate in a snapshot. As snapshots of the backends themselves are taken by the system, this interface mainly provides meta information about the checkpoint.
    /// </summary>
    public interface IFunctionSnapshotContext : IManagedSnapshotContext
    {

    }
}
