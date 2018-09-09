using MDA.Common.Domain.Model;

namespace Forum.Domain.PostAggregate
{
    public class PostId : Identity
    {
        public PostId(string id)
            : base(id)
        {
        }
    }
}
