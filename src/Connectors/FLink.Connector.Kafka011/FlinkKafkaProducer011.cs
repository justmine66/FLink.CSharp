namespace FLink.Connector.Kafka011
{
    /// <summary>
    /// Flink Sink to produce data into a Kafka topic.
    /// This producer is compatible with Kafka 0.11.x. By default producer will use <see cref="Semantic.AtLeastOnce"/> semantic.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public class FLinkKafkaProducer011<TInput>
    {
        /// <summary>
        /// Semantics that can be chosen.
        /// </summary>
        public enum Semantic
        {
            ExactlyOnce, AtLeastOnce, None
        }
    }
}
