namespace MDA.Domain.Shared.Commands
{
    public interface IDomainCommandPublisher
    {
        void Publish(IDomainCommand command);

        void Publish<TAggregateRootId>(IDomainCommand<TAggregateRootId> command);
    }
}
