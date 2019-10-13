using FLink.Runtime.Checkpoint;
using FLink.Runtime.State;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing.Functions;
using FLink.Streaming.Runtime.StreamRecord;

namespace FLink.Streaming.Runtime.Operators.Windowing
{
    /// <summary>
    /// An operator that implements the logic for windowing based on a <see cref="WindowAssigner{T,TW}"/> and <see cref="WindowTrigger{T,TW}"/>.
    /// </summary>
    /// <typeparam name="TK">The type of key.</typeparam>
    /// <typeparam name="TIn">The type of the incoming elements.</typeparam>
    /// <typeparam name="TAcc"></typeparam>
    /// <typeparam name="TOut">The type of elements emitted by the <see cref="IInternalWindowFunction{TIn,TOut,TKey,TW}"/>.</typeparam>
    /// <typeparam name="TW">The type of <see cref="Window"/> that the <see cref="WindowAssigner{T,TW}"/> assigns.</typeparam>
    public class WindowOperator<TK, TIn, TAcc, TOut, TW> : AbstractUdfStreamOperator<TOut, IInternalWindowFunction<TAcc, TOut, TK, TW>>, IOneInputStreamOperator<TIn, TOut>, ITriggerable<TK, TW> where TW : Window
    {
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

        public void ProcessElement(StreamRecord<TIn> element)
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

        public void OnEventTime(IInternalTimer<TK, TW> timer)
        {
            throw new System.NotImplementedException();
        }

        public void OnProcessingTime(IInternalTimer<TK, TW> timer)
        {
            throw new System.NotImplementedException();
        }

        public WindowOperator(IInternalWindowFunction<TAcc, TOut, TK, TW> userFunction) : base(userFunction)
        {
        }
    }
}
