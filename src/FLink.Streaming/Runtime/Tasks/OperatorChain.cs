﻿using FLink.Extensions.DependencyInjection;
using FLink.Metrics.Core;
using FLink.Runtime.IO.Network.Api.Writer;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Runtime.IO;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.StreamStatuses;
using Microsoft.Extensions.Logging;

namespace FLink.Streaming.Runtime.Tasks
{
    public class OperatorChain<OUT, OP> : IStreamStatusMaintainer where OP : IStreamOperator<OUT>
    {
        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<OperatorChain<OUT, OP>>>();

        /// <summary>
        /// Stores all operators on this chain in reverse order.
        /// </summary>
        private readonly IStreamOperator<object>[] _allOperators;

        private readonly RecordWriterOutput<object>[] _streamOutputs;

        private readonly IWatermarkGaugeExposingOutput<StreamRecord<OUT>> _chainEntryPoint;

        private readonly OP _headOperator;

        /// <summary>
        /// Current status of the input stream of the operator chain.
        /// Watermarks explicitly generated by operators in the chain (i.e. timestamp assigner / watermark extractors), will be blocked and not forwarded if this value is <see cref="StreamStatus.Idle"/>
        /// </summary>
        private StreamStatus _streamStatus = StreamStatus.Active;

        public OperatorChain(
            StreamTask<OUT, OP> containingTask,
            IRecordWriterDelegate<SerializationDelegate<StreamRecord<OUT>>> recordWriterDelegate)
        {

        }

        public void ToggleStreamStatus(StreamStatus streamStatus)
        {
            throw new System.NotImplementedException();
        }

        public StreamStatus StreamStatus { get; }
    }

    /// <summary>
    /// An output that measures the last emitted watermark with a <see cref="WatermarkGauge"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements that can be emitted.</typeparam>
    public interface IWatermarkGaugeExposingOutput<in T> : IOutput<T>
    {
        IGauge<long> WatermarkGauge { get; }
    }
}