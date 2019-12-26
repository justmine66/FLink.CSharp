using FLink.Runtime.Pluggable;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Partitioners
{
    /// <summary>
    /// Partitioner that sends all elements to the downstream operator with subtask ID=0.
    /// </summary>
    /// <typeparam name="TElement">Type of the elements in the Stream being partitioned</typeparam>
    public class GlobalPartitioner<TElement> : StreamPartitioner<TElement>
    {
        /// <summary>
        /// 将数据输出到下游算子的第一个实例.
        /// </summary>
        /// <param name="record">the stream record.</param>
        /// <returns>the sub-task id.</returns>
        public override int SelectChannel(SerializationDelegate<StreamRecord<TElement>> record) => 0;

        public override StreamPartitioner<TElement> Copy() => this;

        public override string ToString() => "GLOBAL";
    }
}
