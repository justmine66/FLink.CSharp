using System;
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

        public static int AssignKeyToParallelOperator(object key, int maxParallelism, int parallelism)
        {
            throw new NotImplementedException();
        }

        public static void CheckParallelismPreconditions(int parallelism)
        {
            Preconditions.CheckArgument(parallelism > 0
                                        && parallelism <= UpperBoundMaxParallelism,
                "Operator parallelism not within bounds: " + parallelism);
        }
    }
}
