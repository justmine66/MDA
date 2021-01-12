namespace MDA.Application.Commands
{
    public interface IApplicationCommandResultProcessor
    {
        void AddExecutionPromise(ApplicationCommandExecutionPromise promise);

        void AddExecutionPromise<TPayload>(ApplicationCommandExecutionPromise<TPayload> promise);
    }
}
