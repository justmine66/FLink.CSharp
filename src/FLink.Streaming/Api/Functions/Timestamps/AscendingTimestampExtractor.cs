using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Extensions.DependencyInjection;
using FLink.Streaming.Api.Watermarks;
using Microsoft.Extensions.Logging;

namespace FLink.Streaming.Api.Functions.Timestamps
{
    /// <summary>
    /// A timestamp assigner and watermark generator for streams where timestamps are monotonously ascending.In this case, the local watermarks for the streams are easy to generate, because they strictly follow the timestamps.
    /// </summary>
    /// <typeparam name="T">The type of the elements that this function can extract timestamps from</typeparam>
    public abstract class AscendingTimestampExtractor<T> : IAssignerWithPeriodicWatermarks<T>
    {
        private long _currentTimestamp = long.MinValue;
        // Handler that is called when timestamp monotony is violated.
        private IMonotonyViolationHandler _violationHandler = new LoggingMonotonyViolationHandler();

        public long ExtractTimestamp(T element, long previousElementTimestamp)
        {
            var newTimestamp = ExtractAscendingTimestamp(element);
            if (newTimestamp >= _currentTimestamp)
            {
                return _currentTimestamp = newTimestamp;
            }
            else
            {
                _violationHandler.HandleViolation(newTimestamp, _currentTimestamp);
                return newTimestamp;
            }
        }

        public Watermark GetCurrentWatermark() =>
            new Watermark(_currentTimestamp == long.MinValue ? long.MinValue : _currentTimestamp - 1);

        public AscendingTimestampExtractor<T> WithViolationHandler(IMonotonyViolationHandler handler)
        {
            _violationHandler = Preconditions.CheckNotNull(handler);
            return this;
        }

        /// <summary>
        /// Extracts the timestamp from the given element. The timestamp must be monotonically increasing.
        /// </summary>
        /// <param name="element">The element that the timestamp is extracted from.</param>
        /// <returns>The new timestamp.</returns>
        public abstract long ExtractAscendingTimestamp(T element);

        /// <summary>
        /// Interface for handlers that handle violations of the monotonous ascending timestamps property.
        /// </summary>
        public interface IMonotonyViolationHandler
        {
            /// <summary>
            /// Called when the property of monotonously ascending timestamps is violated, i.e.,when <see cref="elementTimestamp"/> > <see cref="lastTimestamp"/>.
            /// </summary>
            /// <param name="elementTimestamp">The timestamp of the current element.</param>
            /// <param name="lastTimestamp">The last timestamp.</param>
            void HandleViolation(long elementTimestamp, long lastTimestamp);
        }

        public class LoggingMonotonyViolationHandler : IMonotonyViolationHandler
        {
            private readonly ILogger _logger =
                ServiceLocator.GetService<ILogger<LoggingMonotonyViolationHandler>>();

            public void HandleViolation(long elementTimestamp, long lastTimestamp)
            {
                _logger.LogError(
                    $"Timestamp monotony violated:[ElementTimestamp: {elementTimestamp}, LastTimestamp: {lastTimestamp}].");
            }
        }

        public class FailingMonotonyViolationHandler : IMonotonyViolationHandler
        {
            public void HandleViolation(long elementTimestamp, long lastTimestamp)
            {
                throw new RuntimeException(
                    $"Timestamp monotony violated:[ElementTimestamp: {elementTimestamp}, LastTimestamp: {lastTimestamp}].");
            }
        }

        public class IgnoringMonotonyViolationHandler : IMonotonyViolationHandler
        {
            public void HandleViolation(long elementTimestamp, long lastTimestamp) { }
        }
    }
}
