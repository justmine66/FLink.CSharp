namespace FLink.Core.Api.Common.Operators
{
    /// <summary>
    /// Describe the different resource factors of the operator with UDF.
    /// The state backend provides the method to estimate memory usages based on state size in the resource.
    /// </summary>
    public class ResourceSpec
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
        /// How many cpu cores are needed, use double so we can specify cpu like 0.1.
        /// </summary>
        public double CpuCores;

        /// <summary>
        /// How many java heap memory in mb are needed.
        /// </summary>
        public int HeapMemoryInMb;

        /// <summary>
        /// How many nio direct memory in mb are needed.
        /// </summary>
        public int DirectMemoryInMb;

        /// <summary>
        /// How many native memory in mb are needed.
        /// </summary>
        public int NativeMemoryInMb;

        /// <summary>
        /// How many state size in mb are used.
        /// </summary>
        public int StateSizeInMb;

        /// <summary>
        /// The required amount of managed memory (in MB).
        /// </summary>
        public int ManagedMemoryInMb;

        public static string GpuName { get; internal set; }
    }
}
