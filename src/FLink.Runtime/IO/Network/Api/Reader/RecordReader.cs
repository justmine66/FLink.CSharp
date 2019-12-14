using System;
using FLink.Core.Exceptions;
using FLink.Core.IO;
using FLink.Runtime.IO.Network.Partition.Consumer;

namespace FLink.Runtime.IO.Network.Api.Reader
{
    /// <summary>
    /// Record oriented reader for immutable types.
    /// </summary>
    /// <typeparam name="T">Thy type of the records that is read.</typeparam>
    public class RecordReader<T> : AbstractRecordReader<T>, IReader<T>
        where T : IIOReadableWritable
    {
        private readonly Type _recordType;

        private T _currentRecord;

        /// <summary>
        /// Creates a new RecordReader that de-serializes records from the given input gate and can spill partial records to disk, if they grow large.
        /// </summary>
        /// <param name="inputGate">The input gate to read from.</param>
        /// <param name="recordType"></param>
        /// <param name="tmpDirectories">The temp directories. USed for spilling if the reader concurrently reconstructs multiple large records.</param>
        public RecordReader(IInputGate inputGate, Type recordType, string[] tmpDirectories)
            : base(inputGate, tmpDirectories)
        {
            _recordType = recordType;
        }

        public virtual bool HasNext
        {
            get
            {
                if (_currentRecord != null)
                {
                    return true;
                }

                var record = InstantiateRecordType();
                if (!GetNextRecord(record)) return false;

                _currentRecord = record;
                return true;
            }
        }

        public virtual T Next
        {
            get
            {
                if (!HasNext) return default;

                var tmp = _currentRecord;
                _currentRecord = default;
                return tmp;
            }
        }

        private T InstantiateRecordType()
        {
            try
            {
                return (T)Activator.CreateInstance(_recordType);
            }
            catch (Exception e)
            {
                throw new RuntimeException($"Cannot instantiate class {_recordType.Name}", e);
            }
        }
    }
}
