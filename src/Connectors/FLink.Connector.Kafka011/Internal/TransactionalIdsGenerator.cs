namespace FLink.Connector.Kafka011.Internal
{
    /// <summary>
    /// Class responsible for generating transactional ids to use when communicating with Kafka.
    /// </summary>
    public class TransactionalIdsGenerator
    {
        private readonly string prefix;
        private readonly int subtaskIndex;
        private readonly int totalNumberOfSubtasks;
        private readonly int poolSize;
        private readonly int safeScaleDownFactor;

        public TransactionalIdsGenerator(
            string prefix,
            int subtaskIndex,
            int totalNumberOfSubtasks,
            int poolSize,
            int safeScaleDownFactor)
        {
             
        }
    }
}
