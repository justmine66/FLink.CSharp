namespace FLink.Runtime.State
{
    /// <summary>
    /// Implementations of this interface should implement methods acting as an administration role for checkpoint storage, which defined in <see cref="ICheckpointStorageCoordinatorView"/>. And also implement methods acting as a worker role, which defined in <see cref="ICheckpointStorageWorkerView"/>.
    /// </summary>
    public interface ICheckpointStorage : ICheckpointStorageCoordinatorView, ICheckpointStorageWorkerView { }
}
