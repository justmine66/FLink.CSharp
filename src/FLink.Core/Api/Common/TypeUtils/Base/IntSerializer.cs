using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class IntSerializer : TypeSerializerSingleton<int>
    {
        // Sharable instance of the IntSerializer.
        public static readonly IntSerializer Instance = new IntSerializer();

        public override bool IsImmutableType { get; }
        public override TypeSerializer<int> Duplicate()
        {
            throw new System.NotImplementedException();
        }

        public override int CreateInstance()
        {
            throw new System.NotImplementedException();
        }

        public override int Copy(int @from)
        {
            throw new System.NotImplementedException();
        }

        public override int Copy(int @from, int reuse)
        {
            throw new System.NotImplementedException();
        }

        public override int Length { get; }
        public override void Serialize(int record, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Deserialize(IDataInputView source)
        {
            throw new System.NotImplementedException();
        }

        public override int Deserialize(int reuse, IDataInputView source)
        {
            throw new System.NotImplementedException();
        }
    }
}
