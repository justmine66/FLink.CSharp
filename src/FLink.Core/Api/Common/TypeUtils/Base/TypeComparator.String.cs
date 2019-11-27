using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class StringComparator : BasicTypeComparator<string>
    {
        public static readonly StringComparator Instance = new StringComparator();

        public StringComparator(bool @ascending = false) 
            : base(@ascending)
        {
        }

        public override int CompareSerialized(IDataInputView firstSource, IDataInputView secondSource)
        {
            throw new System.NotImplementedException();
        }

        public override bool SupportsNormalizedKey => true;
        public override int NormalizeKeyLength => int.MaxValue;
        public override bool IsNormalizedKeyPrefixOnly(int keyBytes) => true;

        public override void PutNormalizedKey(string record, MemorySegment target, int offset, int numBytes)
        {
            throw new System.NotImplementedException();
        }

        public override TypeComparator<string> Duplicate()=> new StringComparator(AscendingComparison);
    }
}
