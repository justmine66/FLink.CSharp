using System;
using System.Threading;
using System.Threading.Tasks;
using FLink.Core.IO;
using FLink.Core.Util;
using FLink.Extensions.DependencyInjection;
using FLink.Metrics.Core;
using FLink.Runtime.IO.Network.Api.Serialization;
using Microsoft.Extensions.Logging;

namespace FLink.Runtime.IO.Network.Api.Writer
{
    /// <summary>
    /// An abstract record-oriented runtime result writer.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class RecordWriter<TRecord> : AvailabilityProvider where TRecord : IIOReadableWritable
    {
        /// <summary>
        /// Default name for the output flush thread, if no name with a task reference is given.
        /// </summary>
        public const string DefaultOutputFlushThreadName = "OutputFlusher";

        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<RecordWriter<TRecord>>>();

        public IResultPartitionWriter TargetPartition { get; }
        public int NumberOfChannels { get; }
        public IRecordSerializer<TRecord> Serializer { get; }

        private readonly ICounter _numBytesOut = new SimpleCounter();
        private readonly ICounter _numBuffersOut = new SimpleCounter();
        private readonly bool _flushAlways;
        private readonly OutputFlusher _outputFlusher;

        // To avoid synchronization overhead on the critical path, best-effort error tracking is enough here.
        private Exception _flusherException;

        protected RecordWriter(IResultPartitionWriter writer, int timeout, string taskName)
        {
            TargetPartition = writer;
            NumberOfChannels = writer.NumberOfSubpartitions;
            Serializer = new SpanningRecordSerializer<TRecord>();

            Preconditions.CheckArgument(timeout >= -1);
            _flushAlways = (timeout == 0);
            if (timeout == -1 || timeout == 0)
            {
                _outputFlusher = null;
            }
            else
            {
                var threadName = taskName == null
                    ? DefaultOutputFlushThreadName
                    : $"{DefaultOutputFlushThreadName} for {taskName}";

                _outputFlusher = new OutputFlusher(threadName, timeout, FlushAll);
                _outputFlusher.Start();
            }
        }

        protected void Emit(TRecord record, int targetChannel)
        {

        }

        public void FlushAll() => TargetPartition.FlushAll();

        /// <summary>
        /// A dedicated thread that periodically flushes the output buffers, to set upper latency bounds.
        /// The thread is daemonic, because it is only a utility thread.
        /// </summary>
        public class OutputFlusher
        {
            private readonly string _name;
            private readonly int _timeout;
            private readonly Action _callback;

            private Task _task;
            private volatile bool _running = true;

            public OutputFlusher(string name, int timeout, Action callback)
            {
                _name = name;
                _timeout = timeout;
                _callback = callback;
                _task = new Task(Run, TaskCreationOptions.LongRunning);
            }

            public virtual void Start() => _task.Start();

            public virtual void Run()
            {
                while (_running)
                {
                    try
                    {
                        Thread.Sleep(_timeout);
                    }
                    catch (Exception)
                    {
                        // propagate this if we are still running, because it should not happen
                        // in that case
                        if (_running)
                        {
                            throw;
                        }
                    }

                    // any errors here should let the thread come to a halt and be recognized by the writer
                    _callback();
                }
            }

            public void Terminate()
            {
                _running = false;
                _task?.Dispose();
                _task = null;
            }
        }
    }
}
