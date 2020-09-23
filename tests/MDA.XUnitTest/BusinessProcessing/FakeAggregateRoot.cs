using MDA.Domain.Commands;
using MDA.Domain.Models;

namespace MDA.XUnitTest.BusinessProcessing
{
    public sealed class FakeAggregateRoot : EventSourcedAggregateRoot<long>, IDomainCommandHandler<ChangePayloadDomainCommand>
    {
        public FakeAggregateRoot(long id, long payload, int version = 1)
            : base(id, version)
        {
            // 1. 参数预检
            // todo

            // 2. 记录状态
            ApplyDomainEvent(new FakeAggregateRootCreatedDomainEvent(payload));
        }

        public long Payload { get; private set; }

        #region [ Inbound Domain Commands ]

        public void Handle(ChangePayloadDomainCommand command)
        {
            // 1. 参数预检
            // todo

            // 2. 业务操作
            // todo

            // 3. 记录状态
            ApplyDomainEvent(new FakePayloadChangedDomainEvent(command.Payload));
        }

        #endregion

        #region [ Outbound Domain Events ]

        public void Handle(FakeAggregateRootCreatedDomainEvent @event)
        {
            Payload = @event.Payload;
        }

        public void Handle(FakePayloadChangedDomainEvent @event)
        {
            Payload = @event.Payload;
        }

        #endregion
    }
}
