namespace MDA.Domain.Commands
{
    public interface IDomainCommandFiller<in TDomainCommand, in TArg>
        where TDomainCommand : IDomainCommand
    {
        /// <summary>
        /// Translate a data representation into fields set in given domain command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arg"></param>
        void TranslateTo(TDomainCommand command, TArg arg);
    }

    public interface IDomainCommandFiller<in TDomainCommand, in TArg1, in TArg2>
        where TDomainCommand : IDomainCommand
    {
        /// <summary>
        /// Translate a data representation into fields set in given domain command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        void TranslateTo(TDomainCommand command, TArg1 arg1, TArg2 arg2);
    }

    public interface IDomainCommandFiller<in TDomainCommand, in TArg1, in TArg2, in TArg3>
        where TDomainCommand : IDomainCommand
    {
        /// <summary>
        /// Translate a data representation into fields set in given domain command.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        void TranslateTo(TDomainCommand command, TArg1 arg1, TArg2 arg2, TArg3 arg3);
    }
}
