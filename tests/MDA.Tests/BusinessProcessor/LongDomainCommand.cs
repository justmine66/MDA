using MDA.Domain.Commands;

namespace MDA.Tests.BusinessProcessor
{
    public class LongDomainCommand : DomainCommand
    {
        public long Value { get; set; }
    }
}
