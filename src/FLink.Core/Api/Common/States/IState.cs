namespace FLink.Core.Api.Common.States
{
    /// <summary>
    /// Interface that different types of partitioned state must implement. The state is only accessible by functions applied on a KeyedStream. The key is automatically supplied by the system, so the function always sees the value mapped to the key of the current element.That way, the system can handle stream and state partitioning consistently together.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Removes the value mapped under the current key.
        /// </summary>
        void Clear();
    }
}
