using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Commands
{
    public interface IDomainCommandStore
    {
        Task Append(IDomainCommand command);

        Task AppendAsync(IDomainCommand command, CancellationToken token = default);
    }
}
