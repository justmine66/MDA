﻿namespace MDA.Application.Commands
{
    public static class ApplicationCommandResultExtensions
    {
        public static bool IsSuccessful(ApplicationCommandResult result) =>
            result.Status == ApplicationCommandStatus.Successed;

        public static bool Failed(ApplicationCommandResult result) =>
            result.Status == ApplicationCommandStatus.Failed;

        public static bool TimeOut(ApplicationCommandResult result) =>
            result.Status == ApplicationCommandStatus.Timeouted;
    }
}
