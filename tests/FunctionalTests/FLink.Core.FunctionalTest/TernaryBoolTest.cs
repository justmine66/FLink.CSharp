using FLink.Core.Util;
using Xunit;
using Xunit.Abstractions;

namespace FLink.Core.FunctionalTest
{
    public class TernaryBoolTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TernaryBoolTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Get_Enum_Name()
        {
            var value = TernaryBool.Undefined;

            var name = value.GetName();

            _testOutputHelper.WriteLine(name);
        }
    }
}
