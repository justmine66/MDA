namespace MDA.Application.Commands
{
    public static class ApplicationCommandResultExtensions
    {
        public static bool Succeed(this ApplicationCommandResult result) =>
            result.Status == ApplicationCommandStatus.Succeed;

        public static bool Failed(this ApplicationCommandResult result) =>
            result.Status == ApplicationCommandStatus.Failed;

        public static bool TimeOuted(this ApplicationCommandResult result) =>
            result.Status == ApplicationCommandStatus.TimeOuted;

        public static bool Canceled(this ApplicationCommandResult result) =>
            result.Status == ApplicationCommandStatus.Canceled;
    }
}
