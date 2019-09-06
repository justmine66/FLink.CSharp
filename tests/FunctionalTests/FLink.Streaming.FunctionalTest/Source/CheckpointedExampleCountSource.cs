using FLink.Core.Api.Common.State;
using FLink.Runtime.State;
using FLink.Streaming.Api.Checkpoint;
using FLink.Streaming.Api.Functions.Source;

namespace FLink.Streaming.FunctionalTest.Source
{
    public class CheckpointedExampleCountSource : ISourceFunction<long>, ICheckpointedFunction
    {
        private long _counter;
        private volatile bool _isRunning = true;

        private IListState<long> _checkpointedCount;

        public void Run(ISourceContext<long> ctx)
        {
            while (_isRunning && _counter < 1000)
            {
                lock (ctx.GetCheckpointLock())
                {
                    ctx.Collect(_counter);
                    _counter++;
                }
            }
        }

        public void Cancel()
        {
            _isRunning = false;
        }

        public void InitializeState(IFunctionInitializationContext context)
        {
            _checkpointedCount =
                context.OperatorStateStore.GetListState(new ListStateDescriptor<long>("count", default));

            if (context.IsRestored)
            {
                foreach (var count in _checkpointedCount.Get())
                {
                    _counter = count;
                }
            }
        }

        public void SnapshotState(IFunctionSnapshotContext context)
        {
            _checkpointedCount.Clear();
            _checkpointedCount.Add(_counter);
        }
    }
}
