using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class LongSerializer : TypeSerializerSingleton<long>
    {
        public static readonly LongSerializer Instance = new LongSerializer();
        public override bool IsImmutableType { get; }
        public override TypeSerializer<long> Duplicate()
        {
            throw new System.NotImplementedException();
        }

        public override long CreateInstance()
        {
            throw new System.NotImplementedException();
        }

        public override long Copy(long @from)
        {
            throw new System.NotImplementedException();
        }

        public override long Copy(long @from, long reuse)
        {
            throw new System.NotImplementedException();
        }

        public override int Length { get; }
        public override void Serialize(long record, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override long Deserialize(IDataInputView source)
        {
            throw new System.NotImplementedException();
        }

        public override long Deserialize(long reuse, IDataInputView source)
        {
            throw new System.NotImplementedException();
        }
    }
}
