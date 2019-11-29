using System.IO;
using FLink.Core.Api.Common;

namespace FLink.Runtime.Blob
{
    /// <summary>
    /// View on blobs stored in a <see cref="IBlobStore"/>.
    /// </summary>
    public interface IBlobView
    {
        /// <summary>
        /// Copies a blob to a local file.
        /// </summary>
        /// <param name="jobId">ID of the job this blob belongs to (or <tt>null</tt> if job-unrelated)</param>
        /// <param name="blobKey">The blob ID</param>
        /// <param name="localFile">The local file to copy to</param>
        /// <returns>whether the file was copied (<tt>true</tt>) or not (<tt>false</tt>)</returns>
        /// <exception cref="IOException">If the copy fails</exception>
        bool Get(JobId jobId, BlobKey blobKey, FileInfo localFile);
    }
}
