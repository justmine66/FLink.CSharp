using System;
using System.Linq;
using FLink.Core.Util;

namespace FLink.Runtime.State
{
    using static Preconditions;

    /// <summary>
    /// A reference to a storage location. This is a wrapper around an array of bytes that are subject to interpretation by the state backend's storage locations (similar as a serializer needs to interpret byte streams). There is special handling for a 'default location', which can be used as an optimization by state backends, when no extra information is needed to determine where the checkpoints should be stored(all information can be derived from the configuration and the checkpoint id).
    /// </summary>
    public class CheckpointStorageLocationReference : IEquatable<CheckpointStorageLocationReference>
    {
        // The encoded location reference. null indicates the default location.
        private readonly byte[] _encodedReference;

        public CheckpointStorageLocationReference(byte[] encodedReference)
        {
            CheckNotNull(encodedReference);
            CheckArgument(encodedReference.Length > 0);

            _encodedReference = encodedReference;
        }

        // Private constructor for singleton only.
        private CheckpointStorageLocationReference() => _encodedReference = null;

        /// <summary>
        /// Gets the reference bytes.
        /// Important: For efficiency, this method does not make a defensive copy, so the caller must not modify the bytes in the array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetReferenceBytes() => _encodedReference ?? new byte[0];

        /// <summary>
        /// Returns true, if this object is the default reference.
        /// </summary>
        public bool IsDefaultReference => _encodedReference == null;

        public override bool Equals(object obj) => ReferenceEquals(obj, null) || obj is CheckpointStorageLocationReference other && Equals(other);

        public override int GetHashCode() => _encodedReference == null ? 2059243550 : HashCodeHelper.GetHashCode(_encodedReference);

        public bool Equals(CheckpointStorageLocationReference other) => other != null && _encodedReference.SequenceEqual(other._encodedReference);

        #region [ Default Location Reference ]

        private static readonly CheckpointStorageLocationReference Default = new CheckpointStorageLocationReference();

        #endregion
    }
}
