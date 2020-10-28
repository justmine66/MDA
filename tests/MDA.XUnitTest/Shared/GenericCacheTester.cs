using MDA.Shared.DataStructures;
using Xunit;

namespace MDA.XUnitTest.Shared
{
    public class GenericCacheTester
    {
        [Fact]
        public void CacheTest()
        {
            var people = new People() { Id = 1, Name = "a" };

            GenericCache<People>.GetOrSet(() => people);

            GenericCache<People>.Set(people);

            GenericCache<People>.Get();
        }

        public class People
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }
    }
}
