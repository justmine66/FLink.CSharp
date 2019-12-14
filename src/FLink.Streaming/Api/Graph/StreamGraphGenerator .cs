using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.Cache;
using FLink.Core.Api.Dag;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Extensions.DependencyInjection;
using FLink.Runtime.JobGraphs;
using FLink.Runtime.State;
using FLink.Streaming.Api.Environment;
using FLink.Streaming.Api.Transformations;
using Microsoft.Extensions.Logging;

namespace FLink.Streaming.Api.Graph
{
    /// <summary>
    /// A generator that generates a <see cref="StreamGraph"/> from a graph of <see cref="tr"/>s.
    /// </summary>
    public class StreamGraphGenerator
    {
        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<StreamGraphGenerator>>();

        public static readonly int DefaultLowerBoundMaxParallelism = KeyGroupRangeAssignment.DefaultLowerBoundMaxParallelism;
        public static readonly ScheduleMode DefaultScheduleMode = ScheduleMode.Eager;
        public static readonly TimeCharacteristic DefaultTimeCharacteristic = TimeCharacteristic.ProcessingTime;
        public static readonly string DefaultJobName = "Flink Streaming Job";
        /// <summary>
        /// The default buffer timeout (max delay of records in the network stack).
        /// </summary>
        public static readonly long DefaultNetworkBufferTimeout = 100L;
        public static readonly string DefaultSlotSharingGroup = "default";

        private readonly IList<Transformation<object>> _transformations;
        private readonly ExecutionConfig _executionConfig;
        private readonly CheckpointConfig _checkpointConfig;
        private SavepointRestoreSettings _savepointRestoreSettings = SavepointRestoreSettings.None;
        private IStateBackend _stateBackend;
        private bool _chaining = true;
        private ScheduleMode _scheduleMode = DefaultScheduleMode;
        private IList<(string, DistributedCache.DistributedCacheEntry)> _userArtifacts;
        private TimeCharacteristic _timeCharacteristic = DefaultTimeCharacteristic;
        private long _defaultBufferTimeout = DefaultNetworkBufferTimeout;
        private string _jobName = DefaultJobName;
        /// <summary>
        /// If there are some stream edges that can not be chained and the shuffle mode of edge is not specified, translate these edges into BLOCKING result partition type.
        /// </summary>
        private bool _blockingConnectionsBetweenChains = false;

        // This is used to assign a unique ID to iteration source/sink
        protected static int _iterationIdCounter = 0;
        public static int NewIterationNodeId
        {
            get
            {
                _iterationIdCounter--;
                return _iterationIdCounter;
            }
        }

        private StreamGraph _streamGraph;
        // Keep track of which Transforms we have already transformed, this is necessary because we have loops, i.e. feedback edges.
        private IDictionary<Transformation<object>, IList<int>> _alreadyTransformed;

        public StreamGraphGenerator(List<Transformation<object>> transformations, ExecutionConfig executionConfig, CheckpointConfig checkpointConfig)
        {
            _transformations = Preconditions.CheckNotNull(transformations);
            _executionConfig = Preconditions.CheckNotNull(executionConfig);
            _checkpointConfig = Preconditions.CheckNotNull(checkpointConfig);
        }

        public StreamGraphGenerator SetStateBackend(IStateBackend stateBackend)
        {
            _stateBackend = stateBackend;
            return this;
        }

        public StreamGraphGenerator SetChaining(bool chaining)
        {
            _chaining = chaining;
            return this;
        }

        public StreamGraphGenerator SetScheduleMode(ScheduleMode scheduleMode)
        {
            _scheduleMode = scheduleMode;
            return this;
        }

        public StreamGraphGenerator SetUserArtifacts(IList<(string, DistributedCache.DistributedCacheEntry)> userArtifacts)
        {
            _userArtifacts = userArtifacts;
            return this;
        }

        public StreamGraphGenerator SetTimeCharacteristic(TimeCharacteristic timeCharacteristic)
        {
            _timeCharacteristic = timeCharacteristic;
            return this;
        }

        public StreamGraphGenerator SetDefaultBufferTimeout(long defaultBufferTimeout)
        {
            _defaultBufferTimeout = defaultBufferTimeout;
            return this;
        }

        public StreamGraphGenerator SetJobName(string jobName)
        {
            _jobName = jobName;
            return this;
        }

        public StreamGraphGenerator SetBlockingConnectionsBetweenChains(bool blockingConnectionsBetweenChains)
        {
            _blockingConnectionsBetweenChains = blockingConnectionsBetweenChains;
            return this;
        }

        public void SetSavepointRestoreSettings(SavepointRestoreSettings savepointRestoreSettings)
        {
            _savepointRestoreSettings = savepointRestoreSettings;
        }

        public StreamGraph Generate()
        {
            _streamGraph = new StreamGraph(_executionConfig, _checkpointConfig, _savepointRestoreSettings)
            {
                StateBackend = _stateBackend,
                IsChainingEnabled = _chaining,
                ScheduleMode = _scheduleMode,
                UserArtifacts = _userArtifacts,
                TimeCharacteristic = _timeCharacteristic,
                JobName = _jobName,
                IsBlockingConnectionsBetweenChains = _blockingConnectionsBetweenChains
            };

            _alreadyTransformed = new Dictionary<Transformation<object>, IList<int>>();

            foreach (var transformation in _transformations)
            {
                Transform(transformation);
            }

            var builtStreamGraph = _streamGraph;

            _alreadyTransformed.Clear();
            _alreadyTransformed = null;
            _streamGraph = null;

            return builtStreamGraph;
        }

        /// <summary>
        /// Transforms one <see cref="Transformation{TElement}"/>.
        /// This checks whether we already transformed it and exits early in that case.
        /// If not it delegates to one of the transformation specific methods.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        private IList<int> Transform(Transformation<object> transform)
        {
            if (_alreadyTransformed.ContainsKey(transform))
            {
                return _alreadyTransformed[transform];
            }

            Logger.LogDebug($"Transforming {transform}");

            if (transform.MaxParallelism <= 0)
            {
                // if the max parallelism hasn't been set, then first use the job wide max parallelism
                // from the ExecutionConfig.
                var globalMaxParallelismFromConfig = _executionConfig.MaxParallelism;
                if (globalMaxParallelismFromConfig > 0)
                {
                    transform.MaxParallelism = globalMaxParallelismFromConfig;
                }
            }

            // call at least once to trigger exceptions about MissingTypeInfo
            transform.GetOutputType();
            IList<int> transformedIds;

            switch (transform)
            {
                case OneInputTransformation<object, object> transformation:
                    transformedIds = TransformOneInputTransform(transformation);
                    break;
                case TwoInputTransformation<object, object, object> transformation:
                    transformedIds = TransformTwoInputTransform(transformation);
                    break;
                default:
                    throw new IllegalStateException("Unknown transformation: " + transform);
            }

            if (!_alreadyTransformed.ContainsKey(transform))
            {
                _alreadyTransformed.Add(transform, transformedIds);
            }

            if (transform.BufferTimeout >= 0)
            {
                _streamGraph.SetBufferTimeout(transform.Id, transform.BufferTimeout);
            }
            else
            {
                _streamGraph.SetBufferTimeout(transform.Id, _defaultBufferTimeout);
            }

            return transformedIds;
        }

        private IList<int> TransformTwoInputTransform(TwoInputTransformation<object, object, object> transform)
        {
            throw new NotImplementedException();
        }

        private IList<int> TransformOneInputTransform<IN, OUT>(OneInputTransformation<IN, OUT> transform)
        {
            return null;
        }
    }
}
