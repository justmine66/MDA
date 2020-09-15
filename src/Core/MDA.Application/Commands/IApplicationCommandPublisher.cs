using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandPublisher
    {
        void Publish(IApplicationCommand command);
    }

    public interface IAsyncApplicationCommandPublisher : IApplicationCommandPublisher
    {
        Task PublishAsync(IApplicationCommand command, CancellationToken token = default);
    }
}
