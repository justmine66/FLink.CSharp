using FLink.Core.Api.Common.Resources;
using FLink.Core.Configurations;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace FLink.Core.Api.Common.Operators
{
    using GpuResource = GPUResource;

    /// <summary>
    /// Describe the different resource factors of the operator with UDF.
    /// The state backend provides the method to estimate memory usages based on state size in the resource.
    /// </summary>
    public class ResourceSpec:IEquatable<ResourceSpec>
    {
        /// <summary>
        /// A ResourceSpec that indicates an unknown set of resources.
        /// </summary>
        public static readonly ResourceSpec Unknown = new ResourceSpec();

        /// <summary>
        /// The default ResourceSpec used for operators and transformation functions.
        /// </summary>
        public static readonly ResourceSpec Default = Unknown;

        /// <summary>
        /// How many cpu cores are needed. Can be null only if it is unknown.
        /// </summary>
        public Resource CpuCores { get; }

        /// <summary>
        /// How much task heap memory is needed. Can be null only if it is unknown.
        /// </summary>
        public MemorySize TaskHeapMemory { get; }

        /// <summary>
        /// How much task off-heap memory is needed. Can be null only if it is unknown.
        /// </summary>
        public MemorySize TaskOffHeapMemory { get; }

        /// <summary>
        /// How much on-heap managed memory is needed. Can be null only if it is unknown.
        /// </summary>
        public MemorySize OnHeapManagedMemory { get; }

        /// <summary>
        /// How much off-heap managed memory is needed. Can be null only if it is unknown.
        /// </summary>
        public MemorySize OffHeapManagedMemory { get; }

        public Dictionary<string, Resource> ExtendedResources { get; }

        public Resource GPUResource => ExtendedResources[GpuResource.DefaultName];

        private ResourceSpec(
            Resource cpuCores,
            MemorySize taskHeapMemory,
            MemorySize taskOffHeapMemory,
            MemorySize onHeapManagedMemory,
            MemorySize offHeapManagedMemory,
            params Resource[] extendedResources)
        {

            Preconditions.CheckNotNull(cpuCores);
            Preconditions.CheckArgument(cpuCores is CPUResource, "cpuCores must be CPUResource");

            CpuCores = cpuCores;
            TaskHeapMemory = Preconditions.CheckNotNull(taskHeapMemory);
            TaskOffHeapMemory = Preconditions.CheckNotNull(taskOffHeapMemory);
            OnHeapManagedMemory = Preconditions.CheckNotNull(onHeapManagedMemory);
            OffHeapManagedMemory = Preconditions.CheckNotNull(offHeapManagedMemory);

            ExtendedResources = new Dictionary<string, Resource>();
            foreach (var resource in extendedResources)
                if (resource != null)
                    ExtendedResources.Add(resource.Name, resource);
        }

        private ResourceSpec()
        {
            CpuCores = null;
            TaskHeapMemory = null;
            TaskOffHeapMemory = null;
            OnHeapManagedMemory = null;
            OffHeapManagedMemory = null;
            ExtendedResources = new Dictionary<string, Resource>();
        }

        /// <summary>
        /// Used by system internally to merge the other resources of chained operators when generating the job graph.
        /// </summary>
        /// <param name="other">Reference to resource to merge in.</param>
        /// <returns>The new resource with merged values.</returns>
        public ResourceSpec Merge(ResourceSpec other)
        {
            Preconditions.CheckNotNull(other, "Cannot merge with null resources");

            if (Equals(Unknown) || other.Equals(Unknown))
                return Unknown;

            var target = new ResourceSpec(
                CpuCores.Merge(other.CpuCores),
                TaskHeapMemory.Add(other.TaskHeapMemory),
                TaskOffHeapMemory.Add(other.TaskOffHeapMemory),
                OnHeapManagedMemory.Add(other.OnHeapManagedMemory),
                OffHeapManagedMemory.Add(other.OffHeapManagedMemory));

            foreach (var (key, value) in ExtendedResources)
                target.ExtendedResources.Add(key, value);

            foreach (var resource in other.ExtendedResources.Values)
            {
                var temp = target.ExtendedResources[resource.Name];
                var merged = temp.Merge(resource);

                target.ExtendedResources.Add(temp.Name, merged);
            }

            return target;
        }

        /// <summary>
        /// Subtracts another resource spec from this one.
        /// </summary>
        /// <param name="other">The other resource spec to subtract.</param>
        /// <returns>The subtracted resource spec.</returns>
        public ResourceSpec Subtract(ResourceSpec other)
        {
            Preconditions.CheckNotNull(other, "Cannot subtract null resources");

            if (Equals(Unknown) || other.Equals(Unknown))
                return Unknown;

            Preconditions.CheckArgument(other.LessThanOrEqual(this), "Cannot subtract a larger ResourceSpec from this one.");

            var target = new ResourceSpec(
                CpuCores.Merge(other.CpuCores),
                TaskHeapMemory.Subtract(other.TaskHeapMemory),
                TaskOffHeapMemory.Subtract(other.TaskOffHeapMemory),
                OnHeapManagedMemory.Subtract(other.OnHeapManagedMemory),
                OffHeapManagedMemory.Subtract(other.OffHeapManagedMemory));

            foreach (var (key, value) in ExtendedResources)
                target.ExtendedResources.Add(key, value);

            foreach (var resource in other.ExtendedResources.Values)
            {
                var temp = target.ExtendedResources[resource.Name];
                var subtracted = temp.Subtract(resource);

                target.ExtendedResources.Add(temp.Name, subtracted);
            }

            return target;
        }

        /// <summary>
        /// Checks the current resource less than or equal with the other resource by comparing all the fields in the resource.
        /// </summary>
        /// <param name="other">The resource to compare</param>
        /// <returns>True if current resource is less than or equal with the other resource, otherwise return false.</returns>
        public bool LessThanOrEqual(ResourceSpec other)
        {
            Preconditions.CheckNotNull(other, "Cannot compare with null resources");

            if (Equals(Unknown) && other.Equals(Unknown))
            {
                return true;
            }

            if (Equals(Unknown) || other.Equals(Unknown))
            {
                throw new IllegalArgumentException("Cannot compare specified resources with UNKNOWN resources.");
            }

            var cmp1 = CpuCores.Value < other.CpuCores.Value;
            var cmp2 = TaskHeapMemory.CompareTo(other.TaskHeapMemory);
            var cmp3 = TaskOffHeapMemory.CompareTo(other.TaskOffHeapMemory);
            var cmp4 = OnHeapManagedMemory.CompareTo(other.OnHeapManagedMemory);
            var cmp5 = OffHeapManagedMemory.CompareTo(other.OffHeapManagedMemory);

            if (cmp1 && cmp2 <= 0 && cmp3 <= 0 && cmp4 <= 0 && cmp5 <= 0)
            {
                foreach (var resource in ExtendedResources.Values)
                {
                    if (!other.ExtendedResources.ContainsKey(resource.Name) ||
                        (other.ExtendedResources.TryGetValue(resource.Name, out var it) && it.Value < resource.Value))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public bool Equals(ResourceSpec other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Equals(CpuCores, other.CpuCores) && 
                   Equals(TaskHeapMemory, other.TaskHeapMemory) &&
                   Equals(TaskOffHeapMemory, other.TaskOffHeapMemory) &&
                   Equals(OnHeapManagedMemory, other.OnHeapManagedMemory) && 
                   Equals(OffHeapManagedMemory, other.OffHeapManagedMemory) && 
                   Equals(ExtendedResources, other.ExtendedResources);
        }

        public override bool Equals(object obj) => obj is ResourceSpec other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (CpuCores != null ? CpuCores.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TaskHeapMemory != null ? TaskHeapMemory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TaskOffHeapMemory != null ? TaskOffHeapMemory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OnHeapManagedMemory != null ? OnHeapManagedMemory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OffHeapManagedMemory != null ? OffHeapManagedMemory.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExtendedResources != null ? ExtendedResources.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            if (this.Equals(Unknown))
            {
                return "ResourceSpec{UNKNOWN}";
            }

            var extResources = new StringBuilder(ExtendedResources.Count * 10);
            foreach (var resource in ExtendedResources)
                extResources.Append(", ").Append(resource.Key).Append('=').Append(resource.Value.Value);

            return "ResourceSpec{" +
                   "cpuCores=" + CpuCores.Value +
                   ", taskHeapMemory=" + TaskHeapMemory +
                   ", taskOffHeapMemory=" + TaskOffHeapMemory +
                   ", onHeapManagedMemory=" + OnHeapManagedMemory +
                   ", offHeapManagedMemory=" + OffHeapManagedMemory + extResources +
                   '}';
        }
    }
}
