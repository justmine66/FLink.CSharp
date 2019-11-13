using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Util;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.State;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing.Functions;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Operators.Windowing
{
    /// <summary>
    /// An operator that implements the logic for windowing based on a <see cref="WindowAssigner{T,TW}"/> and <see cref="WindowTrigger{T,TW}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TInput">The type of the incoming elements.</typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    /// <typeparam name="TOutput">The type of elements emitted by the <see cref="IInternalWindowFunction{TIn,TOut,TKey,TW}"/>.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that the <see cref="WindowAssigner{T,TW}"/> assigns.</typeparam>
    public class WindowOperator<TKey, TInput, TAccumulator, TOutput, TWindow> : AbstractUdfStreamOperator<TOutput, IInternalWindowFunction<TAccumulator, TOutput, TKey, TWindow>>, IOneInputStreamOperator<TInput, TOutput>, ITriggerable<TKey, TWindow> where TWindow : Window
    {
        public WindowAssigner<TInput, TWindow> WindowAssigner;

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public object GetCurrentKey()
        {
            throw new System.NotImplementedException();
        }

        public void InitializeState()
        {
            throw new System.NotImplementedException();
        }

        public void NotifyCheckpointComplete(long checkpointId)
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }

        public void PrepareSnapshotPreBarrier(long checkpointId)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessElement(StreamRecord<TInput> element)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessLatencyMarker(LatencyMarker latencyMarker)
        {
            throw new System.NotImplementedException();
        }

        public void ProcessWatermark(Watermark mark)
        {
            throw new System.NotImplementedException();
        }

        public void SetCurrentKey(object key)
        {
            throw new System.NotImplementedException();
        }

        public OperatorSnapshotFutures SnapshotState(long checkpointId, long timestamp, CheckpointOptions checkpointOptions, ICheckpointStreamFactory storageLocation)
        {
            throw new System.NotImplementedException();
        }

        public void OnEventTime(IInternalTimer<TKey, TWindow> timer)
        {
            throw new System.NotImplementedException();
        }

        public void OnProcessingTime(IInternalTimer<TKey, TWindow> timer)
        {
            throw new System.NotImplementedException();
        }

        public WindowOperator(IInternalWindowFunction<TAccumulator, TOutput, TKey, TWindow> userFunction) : base(userFunction)
        {
        }
    }
}
