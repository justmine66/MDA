using Forum.Domain.DiscussionAggregate.DomainEvents;
using Forum.Domain.ForumAggregate;
using MDA.Common;
using MDA.Event.Abstractions;
using System.Collections.Generic;

namespace Forum.Domain.DiscussionAggregate
{
    public class Discussion : EventSourcedRootEntity,
        IDomainEventHandler<DiscussionStarted>
    {
        public Discussion(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {

        }

        public Discussion(
            string userId,
            ForumId forumId,
            DiscussionId discussionId,
            Author author,
            string subject)
        {
            Assert.NotNullOrEmpty(userId, nameof(userId));
            Assert.NotNull(forumId, nameof(forumId));
            Assert.NotNull(discussionId, nameof(discussionId));
            Assert.NotNull(author, nameof(author));
            Assert.NotNullOrEmpty(subject, nameof(subject));

            Apply(new DiscussionStarted(userId, forumId, discussionId, author, subject));
        }

        private string _userId;
        private ForumId _forumId;
        private DiscussionId _discussionId;
        private Author _author;
        private string _subject;
        private bool _closed;

        public void Handle(DiscussionStarted @event)
        {
            _userId = @event.UserId;
            _forumId = @event.ForumId;
            _discussionId = @event.DiscussionId;
            _author = @event.Author;
            _subject = @event.Subject;
            _closed = false;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return _userId;
            yield return _forumId;
            yield return _discussionId;
        }
    }
}
