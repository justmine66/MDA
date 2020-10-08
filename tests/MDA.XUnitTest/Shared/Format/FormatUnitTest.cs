using Xunit;
using Xunit.Abstractions;

namespace MDA.XUnitTest.Shared.Format
{
    public class FormatUnitTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FormatUnitTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestCustomFormat()
        {
            var p1 = new Person { FirstName = "XY", LastName = "CN" };
            var pf = new PersonFormatter();

            _testOutputHelper.WriteLine(string.Format(pf, "{0:CN}", p1));
            _testOutputHelper.WriteLine(string.Format(pf, "{0:EN}", p1));

            _testOutputHelper.WriteLine($"{p1}");
            _testOutputHelper.WriteLine($"{p1:CN}");
            _testOutputHelper.WriteLine($"{p1:EN}");
        }
    }
}
