namespace MDA.Application.Commands
{
    public interface IApplicationCommandService : 
        IApplicationCommandPublisher,
        IApplicationCommandExecutor 
    { }
}
