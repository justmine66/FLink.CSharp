namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Defines the chaining scheme for the operator. When an operator is chained to the
    /// predecessor, it means that they run in the same thread.They become one operator
    /// consisting of multiple steps.
    /// </summary>
    public enum ChainingStrategy
    {
        /// <summary>
        /// Operators will be eagerly chained whenever possible.
        /// </summary>
        Always,

        /// <summary>
        /// The operator will not be chained to the preceding or succeeding operators.
        /// </summary>
        Never,

        /// <summary>
        /// The operator will not be chained to the predecessor, but successors may chain to this operator.
        /// </summary>
        Head
    }
}
