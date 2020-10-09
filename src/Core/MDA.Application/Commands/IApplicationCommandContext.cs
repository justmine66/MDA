using MDA.Application.Notifications;
using MDA.Domain.Commands;
using System;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandContext
    {
        IServiceProvider ServiceProvider { get; }

        IDomainCommandPublisher DomainCommandPublisher { get; }

        IApplicationNotificationPublisher ApplicationNotificationPublisher { get; }
    }
}
