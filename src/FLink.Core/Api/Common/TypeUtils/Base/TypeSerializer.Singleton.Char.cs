using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public sealed class CharSerializer : TypeSerializerSingleton<char>
    {
        public static readonly CharSerializer Instance = new CharSerializer();

        private const char Zero = (char)0;

        public override bool IsImmutableType => true;

        public override char CreateInstance() => Zero;

        public override char Copy(char @from) => @from;

        public override char Copy(char @from, char reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target) => target.WriteChar(source.ReadChar());

        public override int Length => sizeof(char);

        public override void Serialize(char record, IDataOutputView target) => target.WriteChar(record);

        public override char Deserialize(IDataInputView source) => source.ReadChar();

        public override char Deserialize(char reuse, IDataInputView source) => Deserialize(source);

        public override ITypeSerializerSnapshot<char> SnapshotConfiguration() => new CharSerializerSnapshot();

        public sealed class CharSerializerSnapshot : SimpleTypeSerializerSnapshot<char>
        {
            public CharSerializerSnapshot() : base(() => Instance)
            {
            }
        }
    }
}
