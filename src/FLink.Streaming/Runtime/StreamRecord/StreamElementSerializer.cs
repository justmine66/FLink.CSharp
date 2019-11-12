using System;
using System.IO;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Core.Memory;
using FLink.Core.Util;
using FLink.Runtime.JobGraphs;
using FLink.Streaming.Api.Watermarks;
using FLink.Streaming.Runtime.StreamStatuses;

namespace FLink.Streaming.Runtime.StreamRecord
{
    /// <summary>
    /// Serializer for <see cref="StreamRecord{T}"/>, <see cref="Watermark"/>, <see cref="LatencyMarker"/>, and <see cref="StreamStatus"/>.
    /// This does not behave like a normal <see cref="TypeSerializer{T}"/>, instead, this is only used at the stream task/operator level for transmitting StreamRecords and Watermarks.
    /// </summary>
    public class StreamElementSerializer<T> : TypeSerializer<StreamElement>
    {
        private static readonly int TagRecWithTimestamp = 0;
        private static readonly int TagRecWithoutTimestamp = 1;
        private static readonly int TagWatermark = 2;
        private static readonly int TagLatencyMarker = 3;
        private static readonly int TagStreamStatus = 4;

        public TypeSerializer<T> ContainedTypeSerializer;

        public StreamElementSerializer(TypeSerializer<T> serializer)
        {
            if (serializer is StreamElementSerializer<T>)
            {
                throw new RuntimeException("StreamRecordSerializer given to StreamRecordSerializer as value TypeSerializer: " + serializer);
            }

            ContainedTypeSerializer = Preconditions.CheckNotNull(serializer);
        }

        public override bool IsImmutableType => false;

        public override TypeSerializer<StreamElement> Duplicate()
        {
            throw new NotImplementedException();
        }

        public override StreamElement CreateInstance()
        {
            throw new NotImplementedException();
        }

        public override StreamElement Copy(StreamElement @from)
        {
            throw new NotImplementedException();
        }

        public override StreamElement Copy(StreamElement @from, StreamElement reuse)
        {
            throw new NotImplementedException();
        }

        public override int Length => -1;

        public override void Serialize(StreamElement value, IDataOutputView target)
        {
            if (value.IsRecord)
            {
                var record = value.AsRecord<T>();

                if (record.HasTimestamp)
                {
                    target.Write(TagRecWithTimestamp);
                    target.WriteLong(record.Timestamp);
                }
                else
                {
                    target.Write(TagRecWithoutTimestamp);
                }

                ContainedTypeSerializer.Serialize(record.Value, target);
            }
            else if (value.IsWatermark)
            {
                target.Write(TagWatermark);
                target.WriteLong(value.AsWatermark().Timestamp);
            }
            else if (value.IsStreamStatus)
            {
                target.Write(TagStreamStatus);
                target.WriteInt(value.AsStreamStatus().Status);
            }
            else if (value.IsLatencyMarker)
            {
                target.Write(TagLatencyMarker);
                target.WriteLong(value.AsLatencyMarker().MarkedTime);
                target.WriteLong(value.AsLatencyMarker().OperatorId.LowerPart);
                target.WriteLong(value.AsLatencyMarker().OperatorId.UpperPart);
                target.WriteInt(value.AsLatencyMarker().SubTaskIndex);
            }
            else
            {
                throw new RuntimeException();
            }
        }

        public override StreamElement Deserialize(IDataInputView source)
        {
            int tag = source.ReadByte();
            if (tag == TagRecWithTimestamp)
            {
                var timestamp = source.ReadLong();
                return new StreamRecord<T>(ContainedTypeSerializer.Deserialize(source), timestamp);
            }
            else if (tag == TagRecWithoutTimestamp)
            {
                return new StreamRecord<T>(ContainedTypeSerializer.Deserialize(source));
            }
            else if (tag == TagWatermark)
            {
                return new Watermark(source.ReadLong());
            }
            else if (tag == TagStreamStatus)
            {
                return new StreamStatus(source.ReadInt());
            }
            else if (tag == TagLatencyMarker)
            {
                return new LatencyMarker(source.ReadLong(), new OperatorId(source.ReadLong(), source.ReadLong()), source.ReadInt());
            }
            else
            {
                throw new IOException("Corrupt stream, found tag: " + tag);
            }
        }

        public override StreamElement Deserialize(StreamElement reuse, IDataInputView source)
        {
            int tag = source.ReadByte();
            if (tag == TagRecWithTimestamp)
            {
                var timestamp = source.ReadLong();
                var value = ContainedTypeSerializer.Deserialize(source);
                var reuseRecord = reuse.AsRecord<T>();
                reuseRecord.Replace(value, timestamp);
                return reuseRecord;
            }
            else if (tag == TagRecWithoutTimestamp)
            {
                var value = ContainedTypeSerializer.Deserialize(source);
                var reuseRecord = reuse.AsRecord<T>();
                reuseRecord.Replace(value);
                return reuseRecord;
            }
            else if (tag == TagWatermark)
            {
                return new Watermark(source.ReadLong());
            }
            else if (tag == TagLatencyMarker)
            {
                return new LatencyMarker(source.ReadLong(), new OperatorId(source.ReadLong(), source.ReadLong()), source.ReadInt());
            }
            else
            {
                throw new IOException("Corrupt stream, found tag: " + tag);
            }
        }
    }
}
