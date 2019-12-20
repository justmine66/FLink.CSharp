using FLink.Core.Util;

namespace FLink.Core.Api.Common.Operators.Util
{
    public class OperatorValidationUtils
    {
        private OperatorValidationUtils() { }

        public static void ValidateParallelism(int parallelism, bool canBeParallel = true)
        {
            Preconditions.CheckArgument(canBeParallel || parallelism == 1,
                "The parallelism of non parallel operator must be 1.");
            Preconditions.CheckArgument(parallelism > 0 || parallelism == ExecutionConfig.DefaultParallelism,
                $"The parallelism of an operator must be at least 1, or {nameof(ExecutionConfig.DefaultParallelism)} (use system default).");
        }

        public static void ValidateMaxParallelism(int maxParallelism, bool canBeParallel) =>
            ValidateMaxParallelism(maxParallelism, int.MaxValue, canBeParallel);

        public static void ValidateMaxParallelism(int maxParallelism, int upperBound = int.MaxValue, bool canBeParallel = true)
        {
            Preconditions.CheckArgument(maxParallelism > 0,
                "The maximum parallelism must be greater than 0.");
            Preconditions.CheckArgument(canBeParallel || maxParallelism == 1,
                "The maximum parallelism of non parallel operator must be 1.");
            Preconditions.CheckArgument(maxParallelism > 0 && maxParallelism <= upperBound,
                "Maximum parallelism must be between 1 and " + upperBound + ". Found: " + maxParallelism);
        }

        public static void ValidateMinAndPreferredResources(ResourceSpec minResources, ResourceSpec preferredResources)
        {
            Preconditions.CheckNotNull(minResources, "The min resources must be not null.");
            Preconditions.CheckNotNull(preferredResources, "The preferred resources must be not null.");
            Preconditions.CheckArgument(minResources.LessThanOrEqual(preferredResources),
                "The resources must be either both UNKNOWN or both not UNKNOWN. If not UNKNOWN,"
                + " the preferred resources must be greater than or equal to the min resources.");
        }
    }
}
