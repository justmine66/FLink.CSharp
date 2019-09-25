namespace FLink.Core.Api.Common.Functions
{
    /// <summary>
    /// Base interface for Reduce functions. Reduce functions combine groups of elements to a single value, by taking always two elements and combining them into one.Reduce functions may be used on entire data sets, or on grouped data sets.In the latter case, each group is reduced individually.
    /// </summary>
    /// <typeparam name="T">Type of the elements that this function processes.</typeparam>
    public interface IReduceFunction<T> : IFunction
    {
        /// <summary>
        /// The core method of ReduceFunction, combining two values into one value of the same type. The reduce function is consecutively applied to all values of a group until only a single value remains.
        /// </summary>
        /// <param name="value1">The first value to combine.</param>
        /// <param name="value2">The second value to combine.</param>
        /// <returns>The combined value of both input values.</returns>
        /// <exception cref="System.Exception">This method may throw exceptions. Throwing an exception will cause the operation to fail and may trigger recovery.</exception>
        T Reduce(T value1, T value2);
    }
}
