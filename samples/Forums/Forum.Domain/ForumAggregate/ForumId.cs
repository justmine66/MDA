using MDA.Common.Domain.Model;

namespace Forum.Domain.ForumAggregate
{
    public class ForumId : Identity
    {
        public ForumId(string id)
            : base(id)
        {
        }
    }
}
