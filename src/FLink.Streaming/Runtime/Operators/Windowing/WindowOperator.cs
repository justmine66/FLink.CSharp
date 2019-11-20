using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Runtime.Checkpoint;
using FLink.Runtime.State;
using FLink.Streaming.Api.Functions.Windowing;
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

        public TimestampedCollector<TOutput> TimestampedCollector;

        public WindowContext ProcessContext;

        public Context TriggerContext = new Context(default, default);

        /// <summary>
        /// For serializing the key in checkpoints.
        /// </summary>
        public TypeSerializer<TKey> KeySerializer;

        /// <summary>
        /// For serializing the window in checkpoints.
        /// </summary>
        public TypeSerializer<TWindow> WindowSerializer;

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

        /// <summary>
        /// Emits the contents of the given window using the <see cref="IInternalWindowFunction{TInput,TOutput,TKey,TWindow}"/>.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="contents"></param>
        private void EmitWindowContents(TWindow window, TAccumulator contents)
        {
            TimestampedCollector.SetAbsoluteTimestamp(window.MaxTimestamp);
            ProcessContext.Window = window;
            UserFunction.Process(TriggerContext.Key, TriggerContext.Window, ProcessContext, contents,
                TimestampedCollector);
        }

        #endregion

        /// <summary>
        /// Base class for per-window <see cref="IKeyedStateStore"/>.
        /// Used to allow per-window state access for <see cref="ProcessWindowFunction{TInput,TOutput,TKey,TWindow}"/>
        /// </summary>
        public abstract class AbstractPerWindowStateStore : DefaultKeyedStateStore<TKey>
        {
            /// <summary>
            /// we have this in the base class even though it's not used in MergingKeyStore so that we can always set it and ignore what actual implementation we have.
            /// </summary>
            public TWindow Window;

            public TypeSerializer<TWindow> WindowSerializer;

            protected AbstractPerWindowStateStore(IKeyedStateBackend<TKey> keyedStateBackend, ExecutionConfig executionConfig)
                : base(keyedStateBackend, executionConfig) { }
        }

        /// <summary>
        /// Special <see cref="AbstractPerWindowStateStore"/> that doesn't allow access to per-window state.
        /// </summary>
        public class MergingWindowStateStore : AbstractPerWindowStateStore
        {
            public MergingWindowStateStore(IKeyedStateBackend<TKey> keyedStateBackend, ExecutionConfig executionConfig)
                : base(keyedStateBackend, executionConfig) { }

            public override IValueState<TValue> GetState<TValue>(ValueStateDescriptor<TValue> stateProperties)
            {
                throw new InvalidOperationException("Per-window state is not allowed when using merging windows.");
            }

            public override IListState<TValue> GetListState<TValue>(ListStateDescriptor<TValue> stateProperties)
            {
                throw new InvalidOperationException("Per-window state is not allowed when using merging windows.");
            }

            public override IReducingState<TValue> GetReducingState<TValue>(ReducingStateDescriptor<TValue> stateProperties)
            {
                throw new InvalidOperationException("Per-window state is not allowed when using merging windows.");
            }

            public override IMapState<TKey1, TValue> GetMapState<TKey1, TValue>(MapStateDescriptor<TKey1, TValue> stateProperties)
            {
                throw new InvalidOperationException("Per-window state is not allowed when using merging windows.");
            }

            public override IAggregatingState<TInput1, TOutput1> GetAggregatingState<TInput1, TAccumulator1, TOutput1>(AggregatingStateDescriptor<TInput1, TAccumulator1, TOutput1> stateProperties)
            {
                throw new InvalidOperationException("Per-window state is not allowed when using merging windows.");
            }
        }

        /// <summary>
        /// Regular per-window state store for use with <see cref="ProcessWindowFunction{TInput,TOutput,TKey,TWindow}"/>.
        /// </summary>
        public class PerWindowStateStore : AbstractPerWindowStateStore
        {
            public PerWindowStateStore(IKeyedStateBackend<TKey> keyedStateBackend, ExecutionConfig executionConfig)
                : base(keyedStateBackend, executionConfig)
            {
            }

            protected override TState GetPartitionedState<TState, TValue>(StateDescriptor<TState, TValue> stateDescriptor)
            {
                return KeyedStateBackend.GetPartitionedState(Window, WindowSerializer, stateDescriptor);
            }
        }

        /// <summary>
        /// A utility class for handling <see cref="ProcessWindowFunction{TInput,TOutput,TKey,TWindow}"/> invocations.
        /// </summary>
        public class WindowContext : IInternalWindowContext
        {
            public TWindow Window;

            public AbstractPerWindowStateStore WindowStateStore;

            private readonly IInternalTimerService<TWindow> _internalTimerService;

            private readonly IOutput<StreamRecord<TOutput>> _output;

            private readonly WindowOperator<TKey, TInput, TAccumulator, TOutput, TWindow> _windowOperator;

            public WindowContext(TWindow window,
                WindowAssigner<TInput, TWindow> windowAssigner,
                AbstractKeyedStateBackend<TKey> keyedStateBackend,
                ExecutionConfig executionConfig,
                IInternalTimerService<TWindow> internalTimerService,
                IOutput<StreamRecord<TOutput>> output,
                WindowOperator<TKey, TInput, TAccumulator, TOutput, TWindow> windowOperator)
            {
                Window = window;
                _internalTimerService = internalTimerService;
                _output = output;
                _windowOperator = windowOperator;
                WindowStateStore = windowAssigner is MergingWindowAssigner<TInput, TWindow>
                    ? (AbstractPerWindowStateStore)new MergingWindowStateStore(keyedStateBackend, executionConfig)
                    : new PerWindowStateStore(keyedStateBackend, executionConfig);
            }

            public long CurrentProcessingTime => _internalTimerService.CurrentProcessingTime;
            public long CurrentWatermark => _internalTimerService.CurrentWatermark;

            public IKeyedStateStore WindowState
            {
                get
                {
                    WindowStateStore.Window = Window;
                    return WindowStateStore;
                }
            }

            public IKeyedStateStore GlobalState => _windowOperator.KeyedStateStore;

            public void Output<T>(OutputTag<T> outputTag, T value)
            {
                if (outputTag == null)
                    throw new IllegalArgumentException("OutputTag must not be null.");

                _output.Collect(outputTag, new StreamRecord<T>(value, Window.MaxTimestamp));
            }

            public override string ToString() => "WindowContext{Window = " + Window + "}";
        }

        public class Context : IWindowTriggerContext
        {
            public TKey Key;
            public TWindow Window;

            public IList<TWindow> MergedWindows;

            public Context(TKey key, TWindow window)
            {
                Key = key;
                Window = window;
            }

            public long CurrentProcessingTime => throw new NotImplementedException();

            public long CurrentWatermark => throw new NotImplementedException();

            public void DeleteEventTimeTimer(long time)
            {
                throw new NotImplementedException();
            }

            public void DeleteProcessingTimeTimer(long time)
            {
                throw new NotImplementedException();
            }

            public TState GetPartitionedState<TState, TValue>(StateDescriptor<TState, TValue> stateDescriptor) where TState : IState
            {
                throw new NotImplementedException();
            }

            public void RegisterEventTimeTimer(long time)
            {
                throw new NotImplementedException();
            }

            public void RegisterProcessingTimeTimer(long time)
            {
                throw new NotImplementedException();
            }
        }
    }
}
