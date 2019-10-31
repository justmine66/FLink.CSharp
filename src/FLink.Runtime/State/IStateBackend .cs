using System.IO;
using FLink.Core.Api.Common;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A State Backend defines how the state of a streaming application is stored and check pointed. Different State Backend store their state in different fashions, and use different data structures to hold the state of a running application.
    /// </summary>
    public interface IStateBackend
    {
        #region [ Checkpoint storage - the durable persistence of checkpoint data ]

        /// <summary>
        /// Resolves the given pointer to a checkpoint/savepoint into a checkpoint location.
        /// The location supports reading the checkpoint metadata, or disposing the checkpoint storage location.
        /// If the state backend cannot understand the format of the pointer (for example because it was created by a different state backend) this method should throw an <see cref="IOException"/>.
        /// </summary>
        /// <param name="externalPointer">The external checkpoint pointer to resolve.</param>
        /// <returns>The checkpoint location handle.</returns>
        /// <exception cref="IOException">Thrown, if the state backend does not understand the pointer, or if the pointer could not be resolved due to an I/O error.</exception>
        ICompletedCheckpointStorageLocation ResolveCheckpoint(string externalPointer);

        /// <summary>
        /// Creates a storage for checkpoints for the given job. The checkpoint storage is used to write checkpoint data and metadata.
        /// </summary>
        /// <param name="jobId">The job to store checkpoint data for.</param>
        /// <returns>A checkpoint storage for the given job.</returns>
        /// <exception cref="IOException">Thrown if the checkpoint storage cannot be initialized.</exception>
        ICheckpointStorage CreateCheckpointStorage(JobId jobId);

        #endregion

        #region [ Structure Backends ]



        #endregion
    }
}
