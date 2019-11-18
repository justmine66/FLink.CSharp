using System.Collections.Generic;

namespace FLink.Connector.Kafka011.Internal
{
    using static Core.Util.Preconditions;

    /// <summary>
    /// Class responsible for generating transactional ids to use when communicating with Kafka.
    /// </summary>
    public class TransactionalIdsGenerator
    {
        private readonly string _prefix;
        private readonly int _subTaskIndex;
        private readonly int _totalNumberOfSubTasks;
        private readonly int _poolSize;
        private readonly int _safeScaleDownFactor;

        public TransactionalIdsGenerator(
            string prefix,
            int subTaskIndex,
            int totalNumberOfSubTasks,
            int poolSize,
            int safeScaleDownFactor)
        {
            CheckArgument(subTaskIndex < totalNumberOfSubTasks);
            CheckArgument(poolSize > 0);
            CheckArgument(safeScaleDownFactor > 0);
            CheckArgument(subTaskIndex >= 0);

            _prefix = CheckNotNull(prefix);
            _subTaskIndex = subTaskIndex;
            _totalNumberOfSubTasks = totalNumberOfSubTasks;
            _poolSize = poolSize;
            _safeScaleDownFactor = safeScaleDownFactor;
        }

        /// <summary>
        /// Range of available transactional ids to use is: [nextFreeTransactionalId, nextFreeTransactionalId + parallelism * kafkaProducersPoolSize) loop below picks in a deterministic way a subrange of those available transactional ids based on index of this subtask.
        /// </summary>
        /// <param name="nextFreeTransactionalId"></param>
        /// <returns></returns>
        public HashSet<string> GenerateIdsToUse(long nextFreeTransactionalId)
        {
            var transactionalIds = new HashSet<string>();
            for (var i = 0; i < _poolSize; i++)
            {
                var transactionalId = nextFreeTransactionalId + _subTaskIndex * _poolSize + i;
                transactionalIds.Add(GenerateTransactionalId(transactionalId));
            }
            return transactionalIds;
        }

        /// <summary>
        /// If we have to abort previous transactional id in case of restart after a failure BEFORE first checkpoint completed, we don't know what was the parallelism used in previous attempt. In that case we must guess the ids range to abort based on current configured pool size, current parallelism and safeScaleDownFactor.
        /// </summary>
        /// <returns></returns>
        public HashSet<string> GenerateIdsToAbort()
        {
            var idsToAbort = new HashSet<string>();
            for (var i = 0; i < _safeScaleDownFactor; i++)
            {
                var idsToUse = GenerateIdsToUse(i * _poolSize * _totalNumberOfSubTasks);
                idsToAbort.UnionWith(idsToUse);
            }
            return idsToAbort;
        }

        private string GenerateTransactionalId(long transactionalId) => _prefix + "-" + transactionalId;
    }
}
