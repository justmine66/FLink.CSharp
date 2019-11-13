namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public class StringComparator : BasicTypeComparator<string>
    {
        public static readonly StringComparator Instance = new StringComparator();

        public override int Hash(string record)
        {
            throw new System.NotImplementedException();
        }

        public override int Compare(string first, string second)
        {
            throw new System.NotImplementedException();
        }
    }
}
