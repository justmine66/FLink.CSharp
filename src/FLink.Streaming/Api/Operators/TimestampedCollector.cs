using FLink.Core.Util;
using FLink.Streaming.Runtime.StreamRecords;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Wrapper around an <see cref="IOutput{TElement}"/> for user functions that expect a <see cref="ICollector{TElement}"/>.
    /// Before giving the <see cref="TimestampedCollector{TElement}"/> to a user function you must set the timestamp that should be attached to emitted elements. Most operators would set the timestamp of the incoming <see cref="StreamRecord{T}"/> here.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that can be emitted.</typeparam>
    public class TimestampedCollector<TElement> : ICollector<TElement>
    {
        private readonly IOutput<StreamRecord<TElement>> _output;

        private readonly StreamRecord<TElement> _reuse;

        /// <summary>
        /// Creates a new <see cref="TimestampedCollector{TElement}"/> that wraps the given <see cref="IOutput{TElement}"/>.
        /// </summary>
        /// <param name="output"></param>
        public TimestampedCollector(IOutput<StreamRecord<TElement>> output)
        {
            _output = output;
            _reuse = new StreamRecord<TElement>(default);
        }

        public void Collect(TElement element) => _output.Collect(_reuse.Replace(element));

        public void Close() => _output.Close();

        public void SetTimestamp<T>(StreamRecord<T> timestampBase)
        {
            if (timestampBase.HasTimestamp)
                _reuse.Timestamp = timestampBase.Timestamp;
            else
                _reuse.EraseTimestamp();
        }

        public void SetAbsoluteTimestamp(long timestamp) => _reuse.Timestamp = timestamp;

        public void EraseTimestamp() => _reuse.EraseTimestamp();
    }
}
