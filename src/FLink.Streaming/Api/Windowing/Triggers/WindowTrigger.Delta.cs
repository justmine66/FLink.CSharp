using FLink.Core.Api.Common.States;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Streaming.Api.Functions.Windowing.Delta;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A <see cref="WindowTrigger{TElement,TWindow}"/> that fires based on a <see cref="IDeltaFunction{TData}"/> and a threshold.
    /// This trigger calculates a delta between the data point which triggered last and the currently arrived data point. It triggers if the delta is higher than a specified threshold.
    /// </summary>
    /// <typeparam name="TElement">The type of elements on which this trigger works.</typeparam>
    /// <typeparam name="TWindow">The type of <see cref="Window"/> on which this trigger can operate.</typeparam>
    public class DeltaWindowTrigger<TElement, TWindow> : WindowTrigger<TElement, TWindow>
        where TWindow : Window
    {
        public double Threshold;
        public IDeltaFunction<TElement> DeltaFunction;
        private readonly ValueStateDescriptor<TElement> _stateDesc;

        /// <summary>
        /// Creates a delta trigger from the given threshold and <see cref="IDeltaFunction{TData}"/>.
        /// </summary>
        /// <param name="threshold">The threshold at which to trigger.</param>
        /// <param name="deltaFunction">The delta function to use</param>
        /// <param name="serializer">TypeSerializer for the data elements.</param>
        public DeltaWindowTrigger(double threshold, IDeltaFunction<TElement> deltaFunction, TypeSerializer<TElement> serializer)
        {
            Threshold = threshold;
            DeltaFunction = deltaFunction;
            _stateDesc = new ValueStateDescriptor<TElement>("last-element", serializer);
        }

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TWindow window, IWindowTriggerContext ctx)
        {
            var lastElementState = ctx.GetPartitionedState(_stateDesc);
            if (lastElementState.Value == null)
            {
                lastElementState.Value = element;
                return WindowTriggerResult.Continue;
            }

            if (DeltaFunction.GetDelta(lastElementState.Value, element) <= Threshold)
                return WindowTriggerResult.Continue;

            lastElementState.Value = element;
            return WindowTriggerResult.Fire;
        }

        public override WindowTriggerResult OnProcessingTime(long time, TWindow window, IWindowTriggerContext ctx) => WindowTriggerResult.Continue;

        public override WindowTriggerResult OnEventTime(long time, TWindow window, IWindowTriggerContext ctx) => WindowTriggerResult.Continue;

        public override void Clear(TWindow window, IWindowTriggerContext ctx) => ctx.GetPartitionedState(_stateDesc).Clear();

        public override string ToString() => "DeltaTrigger(" + DeltaFunction + ", " + Threshold + ")";
    }

    public class DeltaWindowTrigger
    {
        /// <summary>
        /// Creates a delta trigger from the given threshold and <see cref="IDeltaFunction{TData}"/>.
        /// </summary>
        /// <typeparam name="TElement">The type of elements on which this trigger can operate.</typeparam>
        /// <typeparam name="TWindow">The type of <see cref="Window"/> on which this trigger can operate.</typeparam>
        /// <param name="threshold">The threshold at which to trigger.</param>
        /// <param name="deltaFunction">The delta function to use</param>
        /// <param name="serializer">TypeSerializer for the data elements.</param>
        public static DeltaWindowTrigger<TElement, TWindow> Of<TElement, TWindow>(double threshold, IDeltaFunction<TElement> deltaFunction, TypeSerializer<TElement> serializer) where TWindow : Window => new DeltaWindowTrigger<TElement, TWindow>(threshold, deltaFunction, serializer);
    }
}
