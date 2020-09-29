﻿using MDA.Application.Notifications;
using MDA.Domain.Commands;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandContext
    {
        IDomainCommandPublisher DomainCommandPublisher { get; }

        IApplicationNotificationPublisher ApplicationNotificationPublisher { get; }
    }
}
