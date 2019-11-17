namespace FLink.Connector.Kafka011
{
    /// <summary>
    /// Flink Sink to produce data into a Kafka topic.
    /// This producer is compatible with Kafka 0.11.x. By default producer will use <see cref="Semantic.AT_LEAST_ONCE"/> semantic.
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    public class FlinkKafkaProducer011<IN>
    {
        /// <summary>
        /// Semantics that can be chosen.
        /// </summary>
        public enum Semantic
        {
            EXACTLY_ONCE, AT_LEAST_ONCE, NONE
        }
    }
}
