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

    }
}
