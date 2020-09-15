﻿using MDA.MessageBus;
using Microsoft.Extensions.Logging;

namespace MDA.XUnitTest.MessageBus
{
    public class FakeMessageHandler : IMessageHandler<FakeMessage>
    {
        private readonly ILogger<FakeMessageHandler> _logger;

        public FakeMessageHandler(ILogger<FakeMessageHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(FakeMessage message)
        {
            _logger.LogInformation($"The message: {nameof(FakeMessage)}[Payload: {message.Payload}] handled.");
        }
    }
}
