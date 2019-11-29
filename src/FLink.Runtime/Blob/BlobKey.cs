using System;

namespace FLink.Runtime.Blob
{
    /// <summary>
    /// A BLOB key uniquely identifies a BLOB.
    /// </summary>
    public abstract class BlobKey : IComparable<BlobKey>
    {
        public int CompareTo(BlobKey other)
        {
            throw new NotImplementedException();
        }
    }
}
