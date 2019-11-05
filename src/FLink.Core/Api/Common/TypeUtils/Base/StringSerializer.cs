using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class StringSerializer : TypeSerializerSingleton<string>
    {
        // Sharable instance of the IntSerializer.
        public static readonly StringSerializer Instance = new StringSerializer();

        public override bool IsImmutableType { get; }
        public override TypeSerializer<string> Duplicate()
        {
            throw new NotImplementedException();
        }

        public override string CreateInstance()
        {
            throw new NotImplementedException();
        }

        public override string Copy(string @from)
        {
            throw new NotImplementedException();
        }

        public override string Copy(string @from, string reuse)
        {
            throw new NotImplementedException();
        }

        public override int Length { get; }
        public override void Serialize(string record, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override string Deserialize(IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override string Deserialize(string reuse, IDataInputView source)
        {
            throw new NotImplementedException();
        }
    }
}
