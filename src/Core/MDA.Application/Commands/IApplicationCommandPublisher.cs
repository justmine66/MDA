using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandPublisher
    {
        void Publish(IApplicationCommand command);

        void Publish<TId>(IApplicationCommand<TId> command);
    }

    public interface IAsyncApplicationCommandPublisher : IApplicationCommandPublisher
    {
        Task PublishAsync(IApplicationCommand command, CancellationToken token = default);

        Task PublishAsync<TId>(IApplicationCommand<TId> command, CancellationToken token = default);
    }
}
