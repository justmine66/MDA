using System;

namespace MDA.Runtime.BuildBlocks.MessageBus.Specifications.Http
{
    [Flags]
    public enum HttpResponseCodes
    {
        MessageDelivered = 204,
        MessageForbiddenByAccessControls = 403,
        NoPubSubNameOrTopicGiven = 404,
        DeliveryFailed = 500
    }
}
