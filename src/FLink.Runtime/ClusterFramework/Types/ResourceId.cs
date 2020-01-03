using System;
using FLink.Core.Util;

namespace FLink.Runtime.ClusterFramework.Types
{
    /// <summary>
    /// Class for Resource Ids identifying Flink's distributed components.
    /// </summary>
    public class ResourceId : IResourceIdRetrievable, IEquatable<ResourceId>
    {
        public ResourceId(string resourceId)
        {
            Preconditions.CheckNotNull(resourceId, "ResourceId must not be null");

            ResourceIdString = resourceId;
        }

        public string ResourceIdString { get; }

        public ResourceId ResourceIdentifier => this;

        public static ResourceId Generate() => new ResourceId(new AbstractId().ToString());

        public bool Equals(ResourceId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(ResourceIdString, other.ResourceIdString);
        }

        public override bool Equals(object obj) => obj is SlotId other && Equals(other);

        public override int GetHashCode() => (ResourceIdString != null ? ResourceIdString.GetHashCode() : 0);

        public override string ToString() => ResourceIdString;
    }
}
