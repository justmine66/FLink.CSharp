using System;
using FLink.Core.Memory;
using FLink.Core.Util;

namespace FLink.Runtime.IO.Network.Buffers
{
    /// <summary>
    /// Not thread safe class for filling in the content of the <see cref="MemorySegment"/>.
    /// To access written data please use <see cref="BufferConsumer"/> which allows to build <see cref="IBuffer"/> instances from the written data.
    /// </summary>
    public class BufferBuilder
    {
        private readonly MemorySegment _memorySegment;

        private readonly IBufferRecycler _recycler;

        private readonly SettablePositionMarker _positionMarker = new SettablePositionMarker();

        private bool _bufferConsumerCreated = false;

        public BufferBuilder(MemorySegment memorySegment, IBufferRecycler recycler)
        {
            _memorySegment = Preconditions.CheckNotNull(memorySegment);
            _recycler = Preconditions.CheckNotNull(recycler);
        }

        /// <summary>
        /// Gets created matching instance of <see cref="BufferConsumer"/> to this <see cref="BufferBuilder"/>.
        /// There can exist only one <see cref="BufferConsumer"/> per each <see cref="BufferBuilder"/> and vice versa.
        /// </summary>
        /// <returns></returns>
        public BufferConsumer CreateBufferConsumer()
        {
            Preconditions.CheckState(!_bufferConsumerCreated, "There can not exists two BufferConsumer for one BufferBuilder");
            _bufferConsumerCreated = true;

            return new BufferConsumer(
                _memorySegment,
                _recycler,
                _positionMarker);
        }

        /// <summary>
        /// Holds a reference to the current writer position.
        /// Negative values indicate that writer <see cref="BufferBuilder"/> has finished.
        /// Value <see cref="int.MinValue"/> represents finished empty buffer.
        /// </summary>
        public abstract class PositionMarker
        {
            protected const int FinishedEmpty = int.MinValue;

            public abstract int Position { get; }

            public static bool IsFinished(int position) => position < 0;

            public static int GetAbsolute(int position) => position == FinishedEmpty ? 0 : Math.Abs(position);
        }

        /// <summary>
        /// Cached writing implementation of <see cref="PositionMarker"/>.
        /// Writer (<see cref="BufferBuilder"/>) and reader (<see cref="BufferConsumer"/>) caches must be implemented independently of one another - so that the cached values can not accidentally leak from one to another.
        /// Remember to commit the <see cref="SettablePositionMarker"/> to make the changes visible.
        /// </summary>
        public class SettablePositionMarker : PositionMarker
        {
            private volatile int _position = 0;

            /// <summary>
            /// Locally cached value of volatile <see cref="_position"/> to avoid unnecessary volatile accesses.
            /// </summary>
            private int _cachedPosition = 0;

            public SettablePositionMarker(int position = 0) => _cachedPosition = position;

            public override int Position => _position;

            public new bool IsFinished => PositionMarker.IsFinished(_cachedPosition);

            public int Cached => PositionMarker.GetAbsolute(_cachedPosition);

            /// <summary>
            /// Marks this position as finished and returns the current position.
            /// </summary>
            /// <returns>current position as of <see cref="Cached"/></returns>
            public int MarkFinished()
            {
                var currentPosition = Cached;
                var newValue = -currentPosition;

                if (newValue == 0)
                {
                    newValue = FinishedEmpty;
                }

                Set(newValue);

                return currentPosition;
            }

            public void Set(int value) => _cachedPosition = value;

            public void Commit() => _position = _cachedPosition;
        }
    }
}
