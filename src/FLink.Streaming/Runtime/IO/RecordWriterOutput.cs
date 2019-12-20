using System;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Metrics.Core;
using FLink.Runtime.Events;
using FLink.Runtime.IO.Network.Api.Writer;
using FLink.Runtime.Pluggable;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.Metrics;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.StreamStatuses;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Runtime.IO
{
    /// <summary>
    /// Implementation of <see cref="IOutput{TElement}"/> that sends data using a <see cref="RecordWriter{TRecord}"/>.
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    public class RecordWriterOutput<TOutput> : IWatermarkGaugeExposingOutput<StreamRecord<TOutput>>
    {
        private RecordWriter<SerializationDelegate<StreamElement>> _recordWriter;

        private SerializationDelegate<StreamElement> _serializationDelegate;

        private readonly IStreamStatusProvider _streamStatusProvider;

        private readonly OutputTag<TOutput> _outputTag;

        private readonly WatermarkGauge _watermarkGauge = new WatermarkGauge();

        public RecordWriterOutput(
            RecordWriter<SerializationDelegate<StreamElement>> recordWriter,
            TypeSerializer<TOutput> outSerializer,
            OutputTag<TOutput> outputTag, 
            IStreamStatusProvider streamStatusProvider)
        {
            Preconditions.CheckNotNull(recordWriter);

            _outputTag = outputTag;
            _streamStatusProvider = streamStatusProvider;

            if (outSerializer != null)
            {
                var outRecordSerializer = new StreamElementSerializer<TOutput>(outSerializer);
                _serializationDelegate = new SerializationDelegate<StreamElement>(outRecordSerializer);
            }

            _streamStatusProvider = Preconditions.CheckNotNull(streamStatusProvider);
        }

        public void Collect(StreamRecord<TOutput> record)
        {
            if (_outputTag != null)
            {
                // we are not responsible for emitting to the main output.
                return;
            }

            PushToRecordWriter(record);
        }

        public void Close() => _recordWriter.Close();

        public void EmitWatermark(Watermark mark)
        {
            _watermarkGauge.Value = mark.Timestamp;
            _serializationDelegate.Instance = mark;

            if (!_streamStatusProvider.StreamStatus.IsActive) return;

            try
            {
                _recordWriter.BroadcastEmit(_serializationDelegate);
            }
            catch (Exception e)
            {
                throw new RuntimeException(e.Message, e);
            }
        }

        public void Collect<T>(OutputTag<T> outputTag, StreamRecord<T> record)
        {
            if (_outputTag == null || !_outputTag.Equals(outputTag))
            {
                // we are not responsible for emitting to the side-output specified by this OutputTag.
                return;
            }

            PushToRecordWriter(record);
        }

        public void EmitLatencyMarker(LatencyMarker latencyMarker)
        {
            _serializationDelegate.Instance = latencyMarker;

            try
            {
                _recordWriter.RandomEmit(_serializationDelegate);
            }
            catch (Exception e)
            {
                throw new RuntimeException(e.Message, e);
            }
        }

        public void EmitStreamStatus(StreamStatus streamStatus)
        {
            _serializationDelegate.Instance = streamStatus;

            try
            {
                _recordWriter.BroadcastEmit(_serializationDelegate);
            }
            catch (Exception e)
            {
                throw new RuntimeException(e.Message, e);
            }
        }

        public void BroadcastEvent(AbstractEvent @event) => _recordWriter.BroadcastEvent(@event);

        public void Flush() => _recordWriter.FlushAll();

        public IGauge<long> WatermarkGauge => _watermarkGauge;

        private  void PushToRecordWriter<T>(StreamRecord<T> record)
        {
            _serializationDelegate.Instance = record;

            try
            {
                _recordWriter.Emit(_serializationDelegate);
            }
            catch (Exception e)
            {
                throw new RuntimeException(e.Message, e);
            }
        }
    }
}
