namespace FLink.Core.Api.Common.Accumulators
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue">Type of values that are added to the accumulator.</typeparam>
    /// <typeparam name="TResult">Type of the accumulator result as it will be reported to the client</typeparam>
    public interface IAccumulator<TValue, TResult>
    {
        /// <summary>
        /// The value to add to the accumulator object.
        /// </summary>
        /// <param name="value">value</param>
        void Add(TValue value);

        /// <summary>
        /// The local value from the current UDF(User Defined Function) context.
        /// </summary>
        /// <returns></returns>
        TResult GetLocalValue();

        /// <summary>
        /// Reset the local value. This only affects the current UDF(User Defined Function) context.
        /// </summary>
        void ResetLocal();

        void Merge(IAccumulator<TValue, TResult> other);

        IAccumulator<TValue, TResult> Clone();
    }
}
