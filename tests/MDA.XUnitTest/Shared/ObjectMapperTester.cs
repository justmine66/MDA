using MDA.Shared.Mappers;
using Xunit;

namespace MDA.XUnitTest.Shared
{
    public class ObjectMapperTester
    {
        [Fact]
        public void MapTest()
        {
            var people = new People() { Id = 1, Name = "a" };

            var mapped = ObjectPortMapper<People, Cat>.Map(people);
        }

        public class People
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class Cat
        {
            public long Id { get; set; }

            public string Name { get; set; }
        }
    }
}
