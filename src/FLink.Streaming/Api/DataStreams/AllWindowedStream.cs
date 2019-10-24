using System;
using FLink.Core.Util;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Evictors;
using FLink.Streaming.Api.Windowing.Triggers;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.DataStreams
{
    /// <summary>
    /// A <see cref="AllWindowedStream{TElement,TWindow}"/> represents a data stream where the stream of elements is split into windows based on a <see cref="WindowAssigner{TElement,TWindow}"/>. Window emission is triggered based on a <see cref="WindowTrigger{TElement,TWindow}"/>.
    /// Note that the <see cref="AllWindowedStream{TElement,TWindow}"/> is purely an API construct, during runtime the <see cref="AllWindowedStream{TElement,TWindow}"/> will be collapsed together with the operation over the window into one single operation.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the stream.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> that the <see cref="WindowAssigner{TElement,TWindow}"/> assigns the elements to.</typeparam>
    public class AllWindowedStream<TElement, TWindow> where TWindow : Window
    {
        private const long DefaultAllowedLateness = 0L;

        // The keyed data stream that is windowed by this stream.
        private readonly KeyedStream<TElement, byte> _input;
        private readonly WindowAssigner<TElement, TWindow> _assigner;

        private WindowTrigger<TElement, TWindow> _trigger;
        private IWindowEvictor<TElement, TWindow> _evictor;
        // The user-specified allowed lateness.
        private long _allowedLateness = DefaultAllowedLateness;
        // Side output for late data. If no tag is set late data will simply be dropped.
        private OutputTag<TElement> _lateDataOutputTag;

        public AllWindowedStream(DataStream<TElement> input, WindowAssigner<TElement, TWindow> windowAssigner)
        {
            _assigner = windowAssigner;
            _trigger = windowAssigner.GetDefaultTrigger(input.Environment);
        }

        /// <summary>
        /// Sets the <see cref="WindowTrigger{TElement,TWindow}"/> that should be used to trigger window emission.
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public AllWindowedStream<TElement, TWindow> Trigger(WindowTrigger<TElement, TWindow> trigger)
        {
            if (trigger is MergingWindowAssigner<TElement, TWindow> && !trigger.CanMerge)
                throw new InvalidOperationException("A merging window assigner cannot be used with a trigger that does not support merging.");

            _trigger = trigger;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="IWindowEvictor{TElement,TWindow}"/> which will be able to remove elements from the window after the trigger fires and before and/or after the function is applied.
        /// Note: When using an evictor window performance will degrade significantly, since incremental aggregation of window results cannot be used.
        /// </summary>
        /// <param name="evictor"></param>
        /// <returns></returns>
        public AllWindowedStream<TElement, TWindow> Evictor(IWindowEvictor<TElement, TWindow> evictor)
        {
            if (_assigner is BaseAlignedWindowAssigner<TElement>)
                throw new InvalidOperationException("Cannot use a " + _assigner + " with an Evictor.");

            _evictor = evictor;
            return this;
        }

        /// <summary>
        /// Sets the time by which elements are allowed to be late. Elements that arrive behind the watermark by more than the specified time will be dropped.
        /// By default, the allowed lateness is 0.
        /// Setting an allowed lateness is only valid for event-time windows.
        /// </summary>
        /// <param name="lateness"></param>
        /// <returns></returns>
        public AllWindowedStream<TElement, TWindow> AllowedLateness(TimeSpan lateness)
        {
            var millis = lateness.TotalMilliseconds;
            Preconditions.CheckArgument(millis >= 0, "The allowed lateness cannot be negative.");

            _allowedLateness = (long)millis;
            return this;
        }

        /// <summary>
        /// Send late arriving data to the side output identified by the given <param name="outputTag"></param>.
        /// Data is considered late after the watermark has passed the end of the window plus the allowed lateness set using <see cref="AllowedLateness"/>.
        /// </summary>
        /// <param name="outputTag"></param>
        /// <returns></returns>
        public AllWindowedStream<TElement, TWindow> SideOutputLateData(OutputTag<TElement> outputTag)
        {
            Preconditions.CheckNotNull(outputTag, "Side output tag must not be null.");

            _lateDataOutputTag = _input.Environment.Clean(outputTag);
            return this;
        }

        #region [ Operations on the keyed windows ]



        #endregion
    }
}
