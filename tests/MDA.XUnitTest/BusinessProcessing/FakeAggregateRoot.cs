using MDA.Domain.Models;

namespace MDA.XUnitTest.BusinessProcessing
{
    public partial class FakeAggregateRoot
    {
        public FakeAggregateRoot(long payload)
        {
            Payload = payload;
        }

        public long Payload { get; private set; }
    }

    public partial class FakeAggregateRoot : EventSourcedAggregateRoot<long>
    {
        #region [ Inbound Domain Commands ]

        public void OnDomainCommand(CreateDomainCommand command)
        {
            // 1. 参数预检
            // todo

            // 2. 输出事件
            ApplyDomainEvent(new FakeAggregateRootCreatedDomainEvent(command.Payload));
        }

        public void OnDomainCommand(ChangePayloadDomainCommand command)
        {
            // 1. 参数预检
            // todo

            // 2. 业务操作
            // todo

            // 3. 输出事件
            ApplyDomainEvent(new FakePayloadChangedDomainEvent(command.Payload));
        }

        #endregion

        #region [ Outbound Domain Events ]

        public void OnDomainEvent(FakeAggregateRootCreatedDomainEvent @event)
        {
           new FakeAggregateRoot(@event.Payload);
        }

        public void OnDomainEvent(FakePayloadChangedDomainEvent @event)
        {
            Payload = @event.Payload;
        }

        #endregion
    }
}
