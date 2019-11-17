using FLink.Core.Api.Common.Accumulators;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.CSharp.Functions;
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
    /// When an element arrives it gets assigned a key using a <see cref="IKeySelector{TObject, TKey}"/> and it gets assigned to zero or more windows using a <see cref="WindowAssigner{T,TW}"/>. Based on this, the element is put into panes. A pane is the bucket of elements that have the same key and same window. An element can be in multiple panes if it was assigned to multiple windows by the <see cref="WindowAssigner{T,TW}"/>.
    /// Each pane gets its own instance of the provided <see cref="WindowTrigger{T,TW}"/>. This trigger determines when the contents of the pane should be processed to emit results. When a trigger fires, the given <see cref="IInternalWindowFunction{TInput, TOutput, TKey, TWindow}"/> is invoked to produce the results that are emitted for the pane to which the <see cref="WindowTrigger{T,TW}"/> belongs.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TInput">The type of the incoming elements.</typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    /// <typeparam name="TOutput">The type of elements emitted by the <see cref="IInternalWindowFunction{TIn,TOut,TKey,TW}"/>.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that the <see cref="WindowAssigner{T,TW}"/> assigns.</typeparam>
    public class WindowOperator<TKey, TInput, TAccumulator, TOutput, TWindow> : AbstractUdfStreamOperator<TOutput, IInternalWindowFunction<TAccumulator, TOutput, TKey, TWindow>>, IOneInputStreamOperator<TInput, TOutput>, ITriggerable<TKey, TWindow> where TWindow : Window
    {
        public WindowAssigner<TInput, TWindow> WindowAssigner;

        /// <summary>
        /// The allowed lateness for elements.
        /// 1. Deciding if an element should be dropped from a window due to lateness.
        /// 2. Clearing the state of a window if the system time passes the <code>window.maxTimestamp + allowedLateness</code> landmark.
        /// </summary>
        public long AllowedLateness;

        public IInternalTimerService<TWindow> InternalTimerService;

        public WindowOperator(
            WindowAssigner<TInput, TWindow> windowAssigner,
            TypeSerializer<TWindow> windowSerializer,
            IKeySelector<TInput, TKey> keySelector,
            TypeSerializer<TKey> keySerializer,
            StateDescriptor<IAppendingState<TInput, TAccumulator>, object> windowStateDescriptor,
            IInternalWindowFunction<TAccumulator, TOutput, TKey, TWindow> windowFunction,
            WindowTrigger<TInput, TWindow> trigger,
            long allowedLateness,
            OutputTag<TInput> lateDataOutputTag) 
            : base(windowFunction)
        {

        }

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

        /// <summary>
        /// Gets true if the watermark is after the end timestamp plus the allowed lateness of the given window.
        /// </summary>
        /// <param name="window">The given window</param>
        /// <returns></returns>
        protected virtual bool IsWindowLate(TWindow window) => (WindowAssigner.IsEventTime && (CleanupTime(window) <= InternalTimerService.CurrentWatermark));

        /// <summary>
        /// Decide if a record is currently late, based on current watermark and allowed lateness.
        /// </summary>
        /// <param name="element">The element to check</param>
        /// <returns>The element for which should be considered when sideoutputs</returns>
        protected virtual bool IsElementLate(StreamRecord<TInput> element) => (WindowAssigner.IsEventTime) &&
            (element.Timestamp + AllowedLateness <= InternalTimerService.CurrentWatermark);

        protected bool IsCleanupTime(TWindow window, long time) => time == CleanupTime(window);

        #region [ Private Methods ]

        private long CleanupTime(TWindow window)
        {
            if (WindowAssigner.IsEventTime)
            {
                var cleanupTime = window.MaxTimestamp + AllowedLateness;
                return cleanupTime >= window.MaxTimestamp ? cleanupTime : long.MaxValue;
            }
            else return window.MaxTimestamp;
        }

        #endregion
    }
}
