using FLink.Core.Api.CSharp.TypeUtils;
using Xunit;

namespace FLink.Core.FunctionalTest
{
    public class TypeExtractorTest
    {
        [Fact]
        public void CanExtractTupleType()
        {
            var tuple = ("2", 3);

            var type = TypeExtractor.GetForObject(tuple);
        }
    }
}
