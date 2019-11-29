using System;
using System.Collections.Generic;
using FLink.Core.Api.Common;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.CSharp.Functions;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Metrics.Core;
using FLink.Runtime.State;
using FLink.Runtime.State.Internal;
using FLink.Streaming.Api.Functions.Windowing;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;
using FLink.Streaming.Runtime.Operators.Windowing.Functions;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Operators.Windowing
{
    using static Preconditions;

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
    public class WindowOperator<TKey, TInput, TAccumulator, TOutput, TWindow>
        : AbstractUdfStreamOperator<TOutput, IInternalWindowFunction<TAccumulator, TOutput, TKey, TWindow>>,
          IOneInputStreamOperator<TInput, TOutput>,
          ITriggerable<TKey, TWindow>
        where TWindow : Window
    {
        private const string LateElementsDroppedMetricName = "numLateRecordsDropped";
        private bool _disposed = false;

        public WindowAssigner<TInput, TWindow> WindowAssigner;

        private readonly IKeySelector<TInput, TKey> _keySelector;
        private readonly WindowTrigger<TInput, TWindow> _trigger;
        private readonly StateDescriptor<IAppendingState<TInput, TAccumulator>, TAccumulator> _windowStateDescriptor;

        /// <summary>
        /// The allowed lateness for elements.
        /// 1. Deciding if an element should be dropped from a window due to lateness.
        /// 2. Clearing the state of a window if the system time passes the <code>window.maxTimestamp + allowedLateness</code> landmark.
        /// </summary>
        public long AllowedLateness;

        /// <summary>
        /// For serializing the key in checkpoints.
        /// </summary>
        public TypeSerializer<TKey> KeySerializer;

        /// <summary>
        /// For serializing the window in checkpoints.
        /// </summary>
        public TypeSerializer<TWindow> WindowSerializer;

        /// <summary>
        /// <see cref="OutputTag{T}"/> to use for late arriving events. Elements for which <code>window.maxTimestamp + allowedLateness</code> is smaller than the current watermark will be emitted to this.
        /// </summary>
        public OutputTag<TInput> LateDataOutputTag;

        public ICounter NumLateRecordsDropped;

        #region [ State that is not checkpointed ]

        /// <summary>
        /// The state in which the window contents is stored. Each window is a namespace.
        /// </summary>
        private readonly IInternalAppendingState<TKey, TWindow, TInput, TAccumulator, TAccumulator> _windowState;

        /// <summary>
        /// The window state, typed to merging state for merging windows. Null if the window state is not mergeable.
        /// </summary>
        private readonly IInternalMergingState<TKey, TWindow, TInput, TAccumulator, TAccumulator> _windowMergingState;

        /// <summary>
        /// The state that holds the merging window metadata (the sets that describe what is merged).
        /// </summary>
        private readonly IInternalListState<TKey, VoidNamespace, (TWindow, TWindow)> _mergingSetsState;

        /// <summary>
        /// This is given to the <see cref="IInternalWindowFunction{TInput,TOutput,TKey,TWindow}"/> for emitting elements with a given timestamp.
        /// </summary>
        public TimestampedCollector<TOutput> TimestampedCollector;

        public WindowTriggerContext TriggerContext = new WindowTriggerContext(default, default);

        public WindowContext ProcessContext;

        public WindowAssignerContext WindowAssignerContext;

        #endregion

        public IInternalTimerService<TWindow> InternalTimerService;

        /// <summary>
        /// Creates a new <see cref="WindowOperator{TKey,TInput,TAccumulator,TOutput,TWindow}"/> based on the given policies and user functions.
        /// </summary>
        /// <param name="windowAssigner"></param>
        /// <param name="windowSerializer"></param>
        /// <param name="keySelector"></param>
        /// <param name="keySerializer"></param>
        /// <param name="windowStateDescriptor"></param>
        /// <param name="windowFunction"></param>
        /// <param name="trigger"></param>
        /// <param name="allowedLateness"></param>
        /// <param name="lateDataOutputTag"></param>
        public WindowOperator(
            WindowAssigner<TInput, TWindow> windowAssigner,
            TypeSerializer<TWindow> windowSerializer,
            IKeySelector<TInput, TKey> keySelector,
            TypeSerializer<TKey> keySerializer,
            StateDescriptor<IAppendingState<TInput, TAccumulator>, TAccumulator> windowStateDescriptor,
            IInternalWindowFunction<TAccumulator, TOutput, TKey, TWindow> windowFunction,
            WindowTrigger<TInput, TWindow> trigger,
            long allowedLateness,
            OutputTag<TInput> lateDataOutputTag)
            : base(windowFunction)
        {
            CheckArgument(!(windowAssigner is BaseAlignedWindowAssigner<TInput>),
                $"The {windowAssigner.GetType().Name} cannot be used with a WindowOperator. This assigner is only used with the AccumulatingProcessingTimeWindowOperator and the AggregatingProcessingTimeWindowOperator");
            CheckArgument(allowedLateness >= 0);
            CheckArgument(windowStateDescriptor == null || windowStateDescriptor.IsSerializerInitialized, "window state serializer is not properly initialized");

            WindowAssigner = CheckNotNull(windowAssigner);
            WindowSerializer = CheckNotNull(windowSerializer);
            _keySelector = CheckNotNull(keySelector);
            KeySerializer = CheckNotNull(keySerializer);
            _windowStateDescriptor = windowStateDescriptor;
            _trigger = CheckNotNull(trigger);
            AllowedLateness = allowedLateness;
            LateDataOutputTag = lateDataOutputTag;

            ChainingStrategy = ChainingStrategy.Always;
        }

        public override void Open()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessElement(StreamRecord<TInput> element)
        {
            TriggerContext.Trigger = _trigger;

            var elementWindows = WindowAssigner.AssignWindows(element.Value, element.Timestamp, WindowAssignerContext);

            // if element is handled by none of assigned elementWindows.
            var isSkippedElement = true;
            var key = (TKey)KeyedStateBackend.CurrentKey;

            if (WindowAssigner is MergingWindowAssigner<TInput, TWindow>)
            {
                var mergingWindows = MergingWindowSet;

                foreach (var window in elementWindows)
                {
                    // adding the new window might result in a merge, in that case the actualWindow
                    // is the merged window and we work with that. If we don't merge then
                    // actualWindow == window
                    var actualWindow = mergingWindows.AddWindow(window, new MergeFunction(this, key));
                    // drop if the window is already late
                    if (IsWindowLate(actualWindow))
                    {
                        mergingWindows.RetireWindow(actualWindow);
                        continue;
                    }

                    isSkippedElement = false;

                    var stateWindow = mergingWindows.GetStateWindow(actualWindow);
                    if (stateWindow == null)
                    {
                        throw new IllegalStateException($"Window {window} is not in in-flight window set.");
                    }

                    _windowState.SetCurrentNamespace(stateWindow);
                    _windowState.Add(element.Value);

                    TriggerContext.Key = key;
                    TriggerContext.Window = actualWindow;

                    var triggerResult = TriggerContext.OnElement(element);

                    if (triggerResult.IsFire)
                    {
                        var contents = _windowState.Get();
                        if (contents == null)
                        {
                            continue;
                        }

                        EmitWindowContents(actualWindow, contents);
                    }

                    if (triggerResult.IsPurge)
                    {
                        _windowState.Clear();
                    }

                    RegisterCleanupTimer(actualWindow);
                }

                // need to make sure to update the merging state in state
                mergingWindows.Persist();
            }
            else
            {
                foreach (var window in elementWindows)
                {
                    // drop if the window is already late
                    if (IsWindowLate(window))
                    {
                        continue;
                    }

                    isSkippedElement = false;

                    _windowState.SetCurrentNamespace(window);
                    _windowState.Add(element.Value);

                    TriggerContext.Key = key;
                    TriggerContext.Window = window;

                    var triggerResult = TriggerContext.OnElement(element);

                    if (triggerResult.IsFire)
                    {
                        var contents = _windowState.Get();
                        if (contents == null)
                        {
                            continue;
                        }

                        EmitWindowContents(window, contents);
                    }

                    if (triggerResult.IsPurge)
                    {
                        _windowState.Clear();
                    }

                    RegisterCleanupTimer(window);
                }
            }

            // side output input event if
            // element not handled by any window
            // late arriving tag has been set
            // windowAssigner is event time and current timestamp + allowed lateness no less than element timestamp
            if (isSkippedElement && IsElementLate(element))
            {
                if (LateDataOutputTag != null)
                {
                    SideOutput(element);
                }
                else
                {
                    NumLateRecordsDropped.Increment();
                }
            }
        }

        public virtual void OnEventTime(IInternalTimer<TKey, TWindow> timer)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnProcessingTime(IInternalTimer<TKey, TWindow> timer)
        {
            throw new System.NotImplementedException();
        }

        public override void Close()
        {
            base.Close();
            TimestampedCollector = null;
            TriggerContext = null;
            ProcessContext = null;
            WindowAssignerContext = null;
        }

        #region [ Release resources ]

        public sealed override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WindowOperator() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // called via myClass.Dispose(). 
                // OK to use any private object references
                base.Dispose();
            }

            // Release unmanaged resources.
            // Set large fields to null.  
            TimestampedCollector = null;
            TriggerContext = null;
            ProcessContext = null;
            WindowAssignerContext = null;

            _disposed = true;
        }

        #endregion

        /// <summary>
        /// Gets true if the watermark is after the end timestamp plus the allowed lateness of the given window.
        /// </summary>
        /// <param name="window">The given window</param>
        /// <returns></returns>
        protected virtual bool IsWindowLate(TWindow window) => (WindowAssigner.IsEventTime && (GetCleanupTime(window) <= InternalTimerService.CurrentWatermark));

        /// <summary>
        /// Decide if a record is currently late, based on current watermark and allowed lateness.
        /// </summary>
        /// <param name="element">The element to check</param>
        /// <returns>The element for which should be considered when sideoutputs</returns>
        protected virtual bool IsElementLate(StreamRecord<TInput> element) => (WindowAssigner.IsEventTime) &&
            (element.Timestamp + AllowedLateness <= InternalTimerService.CurrentWatermark);

        protected bool IsCleanupTime(TWindow window, long time) => time == GetCleanupTime(window);

        /// <summary>
        /// Retrieves the <see cref="MergingWindowSet"/> for the currently active key.
        /// The caller must ensure that the correct key is set in the state backend.
        /// The caller must also ensure to properly persist changes to state using <see cref="MergingWindowSet.Persist()"/>
        /// </summary>
        protected MergingWindowSet<TInput, TWindow> MergingWindowSet => new MergingWindowSet<TInput, TWindow>((MergingWindowAssigner<TInput, TWindow>)WindowAssigner, _mergingSetsState);

        /// <summary>
        /// Registers a timer to cleanup the content of the window. 
        /// </summary>
        /// <param name="window">the window whose state to discard</param>
        protected void RegisterCleanupTimer(TWindow window)
        {
            var cleanupTime = GetCleanupTime(window);
            if (cleanupTime == long.MaxValue)
            {
                // don't set a GC timer for "end of time"
                return;
            }

            if (WindowAssigner.IsEventTime)
            {
                TriggerContext.RegisterEventTimeTimer(cleanupTime);
            }
            else
            {
                TriggerContext.RegisterProcessingTimeTimer(cleanupTime);
            }
        }

        /// <summary>
        /// Deletes the cleanup timer set for the contents of the provided window. 
        /// </summary>
        /// <param name="window">the window whose state to discard</param>
        protected void DeleteCleanupTimer(TWindow window)
        {
            var cleanupTime = GetCleanupTime(window);
            if (cleanupTime == long.MaxValue)
            {
                // no need to clean up because we didn't set one
                return;
            }

            if (WindowAssigner.IsEventTime)
            {
                TriggerContext.RegisterEventTimeTimer(cleanupTime);
            }
            else
            {
                TriggerContext.RegisterProcessingTimeTimer(cleanupTime);
            }
        }

        /// <summary>
        /// Write skipped late arriving element to SideOutput.
        /// </summary>
        /// <param name="element">skipped late arriving element to side output</param>
        protected void SideOutput(StreamRecord<TInput> element) => Output.Collect(LateDataOutputTag, element);

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

        public class WindowTriggerContext : IWindowOnMergeContext
        {
            public TKey Key;
            public TWindow Window;
            public WindowTrigger<TInput, TWindow> Trigger { get; set; }

            public IList<TWindow> MergedWindows;

            public WindowTriggerContext(TKey key, TWindow window)
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

            public void MergePartitionedState<TState>(StateDescriptor<TState, object> stateDescriptor) where TState : IMergingState<object, object>
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

            public WindowTriggerResult OnElement(StreamRecord<TInput> element)
            {
                return Trigger.OnElement(element.Value, element.Timestamp, Window, this);
            }

            public WindowTriggerResult OnProcessingTime(long time)
            {
                return Trigger.OnProcessingTime(time, Window, this);
            }

            public WindowTriggerResult OnEventTime(long time)
            {
                return Trigger.OnEventTime(time, Window, this);
            }

            public void OnMerge(IList<TWindow> mergedWindows)
            {
                MergedWindows = mergedWindows;
                Trigger.OnMerge(Window, this);
            }

            public void Clear()
            {
                Trigger.Clear(Window, this);
            }
        }

        public class MergeFunction : MergingWindowSet<TInput, TWindow>.IMergeFunction<TWindow>
        {
            private readonly TKey _key;
            private readonly WindowOperator<TKey, TInput, TAccumulator, TOutput, TWindow> _operator;

            public MergeFunction(WindowOperator<TKey, TInput, TAccumulator, TOutput, TWindow> @operator, TKey key)
            {
                _operator = @operator;
                _key = key;
            }

            public void Merge(TWindow mergeResult, IList<TWindow> mergedWindows, TWindow stateWindowResult, IList<TWindow> mergedStateWindows)
            {
                var timerSvc = _operator.InternalTimerService;
                var triggerContext = _operator.TriggerContext;

                if (_operator.WindowAssigner.IsEventTime)
                {
                    if (mergeResult.MaxTimestamp + _operator.AllowedLateness <= timerSvc.CurrentWatermark)
                    {
                        throw new InvalidOperationException(
                            $"The end timestamp of an event-time window cannot become earlier than the current watermark by merging. Current watermark: {timerSvc.CurrentWatermark} window: {mergeResult}");
                    }
                }
                else
                {
                    var currentProcessingTime = timerSvc.CurrentProcessingTime;
                    if (mergeResult.MaxTimestamp <= currentProcessingTime)
                    {
                        throw new InvalidOperationException(
                            $"The end timestamp of a processing-time window cannot become earlier than the current processing time by merging. Current processing time: {currentProcessingTime} window: {mergeResult}");
                    }
                }

                triggerContext.Key = _key;
                triggerContext.Window = mergeResult;
                triggerContext.OnMerge(mergedWindows);

                foreach (var m in mergedWindows)
                {
                    triggerContext.Window = m;
                    triggerContext.Clear();
                    _operator.DeleteCleanupTimer(m);
                }

                // merge the merged state windows into the newly resulting state window
                _operator._windowMergingState.MergeNamespaces(stateWindowResult, mergedStateWindows);
            }
        }

        #region [ Private Methods ]

        private long GetCleanupTime(TWindow window)
        {
            if (!WindowAssigner.IsEventTime)
                return window.MaxTimestamp;

            var cleanupTime = window.MaxTimestamp + AllowedLateness;

            return cleanupTime >= window.MaxTimestamp ? cleanupTime : long.MaxValue;
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
    }
}
