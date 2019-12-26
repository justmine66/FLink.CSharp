using FLink.Core.Api.Dag;
using FLink.Core.Util;

namespace FLink.Runtime.State
{
    /// <summary>
    /// The default lower bound for max parallelism if nothing was configured by the user. We have this so allow users some degree of scale-up in case they forgot to configure maximum parallelism explicitly.
    /// </summary>
    public sealed class KeyGroupRangeAssignment
    {
        /// <summary>
        /// The default lower bound for max parallelism if nothing was configured by the user. We have this so allow users some degree of scale-up in case they forgot to configure maximum parallelism explicitly.
        /// </summary>
        public static readonly int DefaultLowerBoundMaxParallelism = 1 << 7;

        public static readonly int UpperBoundMaxParallelism = Transformation<object>.UpperBoundMaxParallelism;

        /// <summary>
        /// Assigns the given key to a parallel operator index.
        /// </summary>
        /// <param name="key">the key to assign</param>
        /// <param name="maxParallelism">the maximum supported parallelism, aka the number of key-groups.</param>
        /// <param name="parallelism">the current parallelism of the operator</param>
        /// <returns>the index of the parallel operator to which the given key should be routed.</returns>
        public static int AssignKeyToParallelOperator<TKey>(TKey key, int maxParallelism, int parallelism)
        {
            Preconditions.CheckNotNull(key, "Assigned key must not be null!");

            return ComputeOperatorIndexForKeyGroup(maxParallelism, parallelism, AssignToKeyGroup(key, maxParallelism));
        }

        public static void CheckParallelismPreconditions(int parallelism)
        {
            Preconditions.CheckArgument(parallelism > 0
                                        && parallelism <= UpperBoundMaxParallelism,
                "Operator parallelism not within bounds: " + parallelism);
        }

        /// <summary>
        /// Assigns the given key to a key-group index.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key">the key to assign</param>
        /// <param name="maxParallelism">the maximum supported parallelism, aka the number of key-groups.</param>
        /// <returns>the key-group to which the given key is assigned</returns>
        public static int AssignToKeyGroup<TKey>(TKey key, int maxParallelism)
        {
            Preconditions.CheckNotNull(key, "Assigned key must not be null!");

            return ComputeKeyGroupForKeyHash(key.GetHashCode(), maxParallelism);
        }

        /// <summary>
        /// Assigns the given key to a key-group index.
        /// </summary>
        /// <param name="keyHash">the hash of the key to assign</param>
        /// <param name="maxParallelism">the maximum supported parallelism, aka the number of key-groups.</param>
        /// <returns>the key-group to which the given key is assigned</returns>
        public static int ComputeKeyGroupForKeyHash(int keyHash, int maxParallelism) =>
            MathUtils.MurmurHash(keyHash.ToString()) % maxParallelism;

        /// <summary>
        /// Computes the index of the operator to which a key-group belongs under the given parallelism and maximum parallelism.
        /// IMPORTANT: maxParallelism must be <= <see cref="short.MaxValue"/> to avoid rounding problems in this method. If we ever want to go beyond this boundary, this method must perform arithmetic on long values.
        /// </summary>
        /// <param name="maxParallelism">Maximal parallelism that the job was initially created with. 0 < parallelism <= maxParallelism <= <see cref="short.MaxValue"/> must hold.</param>
        /// <param name="parallelism">The current parallelism under which the job runs. Must be <= maxParallelism.</param>
        /// <param name="keyGroupId">Id of a key-group. 0 <= keyGroupID < maxParallelism.</param>
        /// <returns>The index of the operator to which elements from the given key-group should be routed under the given parallelism and maxParallelism.</returns>
        public static int ComputeOperatorIndexForKeyGroup(int maxParallelism, int parallelism, int keyGroupId) => keyGroupId * parallelism / maxParallelism;
    }
}
