using MDA.Domain.Commands;
using Xunit;

namespace MDA.Tests.BusinessProcessor
{
    public class BusinessProcessorUnitTest
    {
        private readonly IDomainCommandPublisher _domainCommandPublisher;

        public BusinessProcessorUnitTest(IDomainCommandPublisher domainCommandPublisher)
        {
            _domainCommandPublisher = domainCommandPublisher;
        }

        [Fact]
        public void AggregateRootActorModel()
        {
            // 测试聚合根处理业务的完整流程，包括以下步骤：
            // 1. 接收领域命令
            // 2. 发送领域事件
            // 3. 处理领域事件

            _domainCommandPublisher.Publish(new LongDomainCommand());
        }
    }
}
