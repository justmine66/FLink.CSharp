using FLink.Core.IO;
using FLink.Core.Memory;
using FLink.Core.Util;

namespace FLink.Runtime.IO.Network.Buffers
{
    /// <summary>
    /// Not thread safe class for producing <see cref="IBuffer"/>.
    /// It reads data written by <see cref="BufferBuilder"/>.
    /// Although it is not thread safe and can be used only by one single thread, this thread can be different than the thread using/writing to <see cref="BufferBuilder"/>.
    /// Pattern here is simple: one thread writes data to <see cref="BufferBuilder"/> and there can be a different thread reading from it using <see cref="BufferConsumer"/>.
    /// </summary>
    public class BufferConsumer : ICloseable
    {
        private readonly IBuffer _buffer;
        private readonly CachedPositionMarker _writerPosition;
        public BufferConsumer(
            MemorySegment memorySegment,
            IBufferRecycler recycler,
            BufferBuilder.PositionMarker currentWriterPosition) : this(
            new NetworkBuffer(Preconditions.CheckNotNull(memorySegment), Preconditions.CheckNotNull(recycler), true),
            currentWriterPosition,
            0)
        {
        }

        public BufferConsumer(
            MemorySegment memorySegment,
            IBufferRecycler recycler,
            BufferBuilder.PositionMarker currentWriterPosition,
            int currentReaderPosition)
            : this(
            new NetworkBuffer(Preconditions.CheckNotNull(memorySegment), Preconditions.CheckNotNull(recycler), true),
            currentWriterPosition,
            currentReaderPosition)
        {
        }

        public BufferConsumer(MemorySegment memorySegment, IBufferRecycler recycler, bool isBuffer)
            : this(memorySegment, recycler, memorySegment.Size, isBuffer)
        {
        }

        public BufferConsumer(MemorySegment memorySegment, IBufferRecycler recycler, int size, bool isBuffer)
            : this(
                new NetworkBuffer(Preconditions.CheckNotNull(memorySegment), Preconditions.CheckNotNull(recycler), isBuffer),
                new BufferBuilder.SettablePositionMarker(size),
                0)
        {
            Preconditions.CheckState(memorySegment.Size > 0);
            Preconditions.CheckState(IsFinished, "BufferConsumer with static size must be finished after construction!");
        }

        private BufferConsumer(
            IBuffer buffer,
            BufferBuilder.PositionMarker currentWriterPosition,
            int currentReaderPosition)
        {
            _buffer = Preconditions.CheckNotNull(buffer);
            _writerPosition = new CachedPositionMarker(Preconditions.CheckNotNull(currentWriterPosition));
            CurrentReaderPosition = currentReaderPosition;
        }

        /// <summary>
        /// Checks whether the <see cref="BufferBuilder"/> has already been finished.
        /// true if the buffer was finished, false otherwise
        /// BEWARE: this method accesses the cached value of the position marker which is only updated after calls to <see cref="Build()"/>
        /// </summary>
        public bool IsFinished => _writerPosition.IsFinished;

        /// <summary>
        /// @return sliced <see cref="IBuffer"/> containing the not yet consumed data. Returned <see cref="IBuffer"/> shares the reference counter with the parent <see cref="BufferConsumer"/> - in order to recycle memory both of them must be recycled/closed.
        /// </summary>
        /// <returns></returns>
        public IBuffer Build()
        {
            _writerPosition.Update();
            var cachedWriterPosition = _writerPosition.Cached;
            var slice = _buffer.ReadOnlySlice(CurrentReaderPosition, cachedWriterPosition - CurrentReaderPosition);
            CurrentReaderPosition = cachedWriterPosition;

            return slice.RetainBuffer();
        }

        public BufferConsumer Copy() => new BufferConsumer(_buffer.RetainBuffer(), _writerPosition.PositionMarker,
            CurrentReaderPosition);

        public bool IsBuffer => _buffer.IsBuffer;

        public void Close()
        {
            if (!_buffer.IsRecycled)
            {
                _buffer.RecycleBuffer();
            }
        }

        public int WrittenBytes => _writerPosition.Cached;

        public int CurrentReaderPosition { get; private set; }

        public bool IsDataAvailable => CurrentReaderPosition < _writerPosition.Latest;

        /// <summary>
        /// Cached reading wrapper around <see cref="BufferBuilder.PositionMarker"/>.
        /// </summary>
        public class CachedPositionMarker
        {
            public BufferBuilder.PositionMarker PositionMarker { get; }

            /// <summary>
            /// Locally cached value of <see cref="PositionMarker"/> to avoid unnecessary volatile accesses.
            /// </summary>
            private int _cachedPosition;

            public CachedPositionMarker(BufferBuilder.PositionMarker positionMarker) => PositionMarker = positionMarker;

            public bool IsFinished => BufferBuilder.PositionMarker.IsFinished(_cachedPosition);

            public int Cached => BufferBuilder.PositionMarker.GetAbsolute(_cachedPosition);

            public int Latest => BufferBuilder.PositionMarker.GetAbsolute(PositionMarker.Position);

            public void Update() => _cachedPosition = PositionMarker.Position;
        }
    }
}
