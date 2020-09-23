using MDA.Shared.Hashes;

namespace MDA.Domain.Commands
{
    public static class DomainCommandExtensions
    {
        public static string GetTopic(this IDomainCommand command) 
            => $"{command.AggregateRootType.Name}.{MurMurHash3.Hash(command.AggregateRootType.FullName)}";
    }
}
