using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.Common.State;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.Windowing.Triggers
{
    /// <summary>
    /// A <see cref="WindowTrigger{TElement,TWindow}"/> that fires once the count of elements in a pane reaches the given count.
    /// </summary>
    public class CountWindowTrigger<TElement, TWindow> : WindowTrigger<TElement, TWindow>
        where TWindow : Window
    {
        private readonly ReducingStateDescriptor<long> _stateDesc = new ReducingStateDescriptor<long>("count", new Sum(), LongSerializer.Instance);

        public long Limit;

        public CountWindowTrigger(long maxCount)
        {
            Limit = maxCount;
        }

        public override void Clear(TWindow window, IWindowTriggerContext ctx) => ctx.GetPartitionedState(_stateDesc).Clear();

        public override WindowTriggerResult OnElement(TElement element, long timestamp, TWindow window, IWindowTriggerContext ctx)
        {
            var count = ctx.GetPartitionedState(_stateDesc);
            count.Add(1L);

            if (count.Get() < Limit)
                return WindowTriggerResult.Continue;

            count.Clear();
            return WindowTriggerResult.Fire;

        }

        public override WindowTriggerResult OnEventTime(long time, TWindow window, IWindowTriggerContext ctx) => WindowTriggerResult.Continue;

        public override WindowTriggerResult OnProcessingTime(long time, TWindow window, IWindowTriggerContext ctx) => WindowTriggerResult.Continue;

        public override bool CanMerge => true;

        public class Sum : IReduceFunction<long>
        {
            public long Reduce(long value1, long value2) => value1 + value2;
        }
    }

    public class CountWindowTrigger
    {
        public static CountWindowTrigger<TE, TW> Of<TE, TW>(long maxCount) where TW : Window =>
            new CountWindowTrigger<TE, TW>(maxCount);
    }
}
