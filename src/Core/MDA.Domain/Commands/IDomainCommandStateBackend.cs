using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Commands
{
    /// <summary>
    /// 领域命令状态后端
    /// </summary>
    public interface IDomainCommandStateBackend
    {
        Task AppendAsync(IDomainCommand command, CancellationToken token = default);
    }
}
