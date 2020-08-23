using MDA.Domain.Models;

namespace MDA.Tests.BusinessProcessor
{
    public class LongAggregateRoot : EventSourcedAggregateRoot<long>
    {
        public LongAggregateRoot(long id)
            : base(id)
        {

        }

        public LongAggregateRoot(long id, int version)
            : base(id, version)
        {

        }

        public long Value { get; private set; }

        void OnDomainCommand(LongDomainCommand command)
        {
            Value = command.Value;

            ApplyDomainEvent(new LongDomainEvent(Value));
        }

        void OnDomainEvent(LongDomainEvent command)
        {
            Value = command.Value;
        }
    }
}
