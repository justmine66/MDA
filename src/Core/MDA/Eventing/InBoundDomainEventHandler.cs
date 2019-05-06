using System.Threading.Tasks;
using MDA.EventSourcing;
using MDA.Persistence;

namespace MDA.Eventing
{
    public abstract class InBoundDomainEventHandler<TRootEntity, TDomainEvent> : IInBoundDomainEventHandler<TDomainEvent>
        where TRootEntity : EventSourcedRootEntity
        where TDomainEvent : IDomainEvent, new()
    {
        private readonly IAppStateProvider _context;

        protected InBoundDomainEventHandler(IAppStateProvider context)
        {
            _context = context;
        }

        public async void OnEvent(TDomainEvent data, long sequence, bool endOfBatch)
        {
            var principal = await _context.GetAsync<TRootEntity>(data.Principal.Id);
            principal.OnDomainEvent(data);
            await OnEventAsync(principal);
        }

        public abstract Task OnEventAsync(TRootEntity principal);

    }
}
