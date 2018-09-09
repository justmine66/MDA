using Forum.Domain.ForumAggregate;
using MDA.Event.Abstractions;

namespace Forum.Domain.DiscussionAggregate.DomainEvents
{
    public sealed class DiscussionStarted : DomainEvent
    {
        public DiscussionStarted(
            string userId,
            ForumId forumId,
            DiscussionId discussionId,
            Author author,
            string subject) :
            base(null, discussionId.Id, nameof(Discussion), 1)
        {
            UserId = userId;
            ForumId = forumId;
            DiscussionId = discussionId;
            Author = author;
            Subject = subject;
        }

        public string UserId { get; set; }
        public ForumId ForumId { get; set; }
        public DiscussionId DiscussionId { get; set; }
        public Author Author { get; set; }
        public string Subject { get; set; }
    }
}
