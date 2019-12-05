using FLink.Streaming.Api.Functions;
using FLink.Streaming.Api.Operators;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Runtime.Operators
{
    /// <summary>
    /// A stream operator that extracts timestamps from stream elements and generates watermarks based on punctuation elements.
    /// </summary>
    /// <typeparam name="TElement">The type of the input elements</typeparam>
    public class TimestampsAndPunctuatedWatermarksOperator<TElement> : AbstractUdfStreamOperator<TElement, IAssignerWithPunctuatedWatermarks<TElement>>, IOneInputStreamOperator<TElement, TElement>
    {
        private long _currentWatermark = long.MaxValue;

        public TimestampsAndPunctuatedWatermarksOperator(IAssignerWithPunctuatedWatermarks<TElement> userFunction)
            : base(userFunction)
        {
            ChainingStrategy = ChainingStrategy.Always;
        }

        public virtual void ProcessElement(StreamRecord<TElement> record)
        {
            var element = record.Value;
            var newTimestamp = UserFunction.ExtractTimestamp(element, record.HasTimestamp ? record.Timestamp : long.MinValue);

            Output.Collect(record.Replace(record.Value, newTimestamp));

            var nextWatermark = UserFunction.CheckAndGetNextWatermark(element, newTimestamp);
            if (nextWatermark != null && nextWatermark.Timestamp > _currentWatermark)
            {
                _currentWatermark = nextWatermark.Timestamp;
                Output.EmitWatermark(nextWatermark);
            }
        }

        /// <summary>
        /// Override the base implementation to completely ignore watermarks propagated from upstream (we rely only on the <see cref="IAssignerWithPunctuatedWatermarks{TElement}"/> to emit watermarks from here).
        /// </summary>
        /// <param name="watermark"></param>
        public override void ProcessWatermark(Watermark watermark)
        {
            // if we receive a long.MaxValue watermark we forward it since it is used
            // to signal the end of input and to not block watermark progress downstream.
            if (watermark.Timestamp == long.MaxValue && _currentWatermark != long.MaxValue)
            {
                _currentWatermark = long.MaxValue;
                Output.EmitWatermark(watermark);
            }
        }
    }
}
