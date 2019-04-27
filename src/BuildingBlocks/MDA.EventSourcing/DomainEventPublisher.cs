using System;
using System.Collections.Generic;

namespace MDA.EventSourcing
{
    public class DomainEventPublisher
    {
        [ThreadStatic]
        static DomainEventPublisher _instance;

        public static DomainEventPublisher Instance => _instance ?? (_instance = new DomainEventPublisher());

        private DomainEventPublisher()
        {
            _publishing = false;
        }

        private bool _publishing;

        private List<IDomainEventSubscriber<IDomainEvent>> _subscribers;
        public List<IDomainEventSubscriber<IDomainEvent>> Subscribers
        {
            get => _subscribers ?? (_subscribers = new List<IDomainEventSubscriber<IDomainEvent>>());
            set => _subscribers = value;
        }

        public void Publish<T>(T domainEvent) where T : IDomainEvent
        {
            if (_publishing || !HasSubscribers()) return;

            try
            {
                _publishing = true;

                var eventType = domainEvent.GetType();

                foreach (var subscriber in Subscribers)
                {
                    var subscribedToType = subscriber.SubscribedToEventType();
                    if (eventType == subscribedToType || subscribedToType == typeof(IDomainEvent))
                    {
                        subscriber.HandleEvent(domainEvent);
                    }
                }
            }
            finally
            {
                _publishing = false;
            }
        }

        public void PublishAll(ICollection<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                Publish(domainEvent);
            }
        }

        public void Reset()
        {
            if (!_publishing)
            {
                Subscribers = null;
            }
        }

        public void Subscribe(IDomainEventSubscriber<IDomainEvent> subscriber)
        {
            if (!_publishing)
            {
                Subscribers.Add(subscriber);
            }
        }

        public void Subscribe(Action<IDomainEvent> handle)
        {
            Subscribe(new DomainEventSubscriber<IDomainEvent>(handle));
        }

        private class DomainEventSubscriber<TEvent> : IDomainEventSubscriber<TEvent>
            where TEvent : IDomainEvent
        {
            private readonly Action<TEvent> _handle;

            public DomainEventSubscriber(Action<TEvent> handle)
            {
                _handle = handle;
            }

            public void HandleEvent(TEvent domainEvent)
            {
                _handle(domainEvent);
            }

            public Type SubscribedToEventType()
            {
                return typeof(TEvent);
            }
        }

        private bool HasSubscribers()
        {
            return _subscribers != null && Subscribers.Count != 0;
        }
    }
}
