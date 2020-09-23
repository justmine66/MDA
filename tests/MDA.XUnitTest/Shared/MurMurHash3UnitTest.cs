using MDA.Shared.Hashes;
using Xunit;

namespace MDA.XUnitTest.Shared
{
    public class MurMurHash3UnitTest
    {
        [Fact(DisplayName = "测试MurMurHash3")]
        public void TestGetMurMurHash3Hash()
        {
            var hash1 = MurMurHash3.Hash("fasdfsafsafsaf");
            var hash2 = MurMurHash3.Hash("afasdfsafsafsaf");
            var hash3 = MurMurHash3.Hash("afasdfsafsafsaf");

            Assert.NotEqual(hash1, hash2);
            Assert.Equal(hash2, hash3);
        }
    }
}
