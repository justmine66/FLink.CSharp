using System.Threading.Tasks;

namespace FLink.Runtime.IO
{
    /// <summary>
    /// Interface defining couple of essential methods for listening on data availability.
    /// </summary>
    public abstract class AvailabilityProvider
    {
        private static readonly TaskCompletionSource<object> Available = new TaskCompletionSource<object>(null);

        public abstract TaskCompletionSource<object> AvailableFuture { get; }

        public virtual bool IsAvailable => AvailableFuture == Available && AvailableFuture.Task.IsCompleted;
        public virtual bool IsApproximatelyAvailable => AvailableFuture == Available;
    }
}
