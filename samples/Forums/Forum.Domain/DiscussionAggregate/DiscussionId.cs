using MDA.Common.Domain.Model;

namespace Forum.Domain.DiscussionAggregate
{
    public class DiscussionId : Identity
    {
        public DiscussionId(string id)
            : base(id)
        {
        }
    }
}
