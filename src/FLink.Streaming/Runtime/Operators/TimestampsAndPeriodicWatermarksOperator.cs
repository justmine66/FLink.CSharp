using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecords;
using FLink.Streaming.Runtime.Tasks;

namespace FLink.Streaming.Runtime.Operators
{
    /// <summary>
    /// A stream operator that extracts timestamps from stream elements and generates periodic watermarks.
    /// </summary>
    /// <typeparam name="T">The type of the input elements</typeparam>
    public class TimestampsAndPeriodicWatermarksOperator<T> : AbstractUdfStreamOperator<T, IAssignerWithPeriodicWatermarks<T>>, IOneInputStreamOperator<T, T>, IProcessingTimeCallback
    {

        private long _watermarkInterval;

        private long _currentWatermark;

        public TimestampsAndPeriodicWatermarksOperator(IAssignerWithPeriodicWatermarks<T> userFunction)
            : base(userFunction)
        {
            ChainingStrategy = ChainingStrategy.Always;
        }

        public override void Open()
        {
            base.Open();

            _currentWatermark = long.MinValue;
            _watermarkInterval = ExecutionConfig.AutoWatermarkInterval;

            if (_watermarkInterval > 0)
            {
                var now = ProcessingTimeService.CurrentProcessingTime;
                ProcessingTimeService.RegisterTimer<bool>(now + _watermarkInterval, this);
            }
        }

        public virtual void ProcessElement(StreamRecord<T> record)
        {
            var element = record.Value;
            var newTimestamp = UserFunction.ExtractTimestamp(element, record.HasTimestamp ? record.Timestamp : long.MinValue);

            Output.Collect(record.Replace(record.Value, newTimestamp));
        }

        public virtual void OnProcessingTime(long timestamp)
        {
            // emit watermark
            var newWatermark = UserFunction.CurrentWatermark;
            if (newWatermark != null && newWatermark.Timestamp > _currentWatermark)
            {
                _currentWatermark = newWatermark.Timestamp;
                Output.EmitWatermark(newWatermark);
            }

            // register next timer
            var now = ProcessingTimeService.CurrentProcessingTime;
            ProcessingTimeService.RegisterTimer<bool>(now + _watermarkInterval, this);
        }

        /// <summary>
        /// Override the base implementation to completely ignore watermarks propagated from upstream(we rely only on the <see cref="IAssignerWithPeriodicWatermarks{TElement}"/> to emit watermarks from here).
        /// </summary>
        /// <param name="mark"></param>
        public override void ProcessWatermark(Watermark mark)
        {
            // if we receive a long.MaxValue watermark we forward it since it is used
            // to signal the end of input and to not block watermark progress downstream.
            if (mark.Timestamp == long.MaxValue && _currentWatermark != long.MaxValue)
            {
                _currentWatermark = long.MaxValue;
                Output.EmitWatermark(mark);
            }
        }

        public override void Close()
        {
            base.Close();

            // emit a final watermark
            var newWatermark = UserFunction.CurrentWatermark;
            if (newWatermark != null && newWatermark.Timestamp > _currentWatermark)
            {
                _currentWatermark = newWatermark.Timestamp;
                Output.EmitWatermark(newWatermark);
            }
        }
    }
}
