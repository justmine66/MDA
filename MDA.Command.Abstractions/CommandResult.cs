using System;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Command.Abstractions
{
    /// <summary>
    /// Represents the result of an command operation.
    /// </summary>
    public class CommandResult
    {
        private readonly List<CommandError> _errors = new List<CommandError>();

        /// <summary>
        /// Flag indicating execution status of an command operation.
        /// </summary>
        public CommandStatus CommandStatus { get; protected set; }
        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="CommandError"/>s containing an errors
        /// that occurred during the command operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="CommandError"/>s.</value>
        public IEnumerable<CommandError> Errors => _errors;
        /// <summary>
        /// Returns an <see cref="CommandResult"/> indicating a successful command operation.
        /// </summary>
        /// <returns>An <see cref="CommandResult"/> indicating a successful command operation.</returns>
        public static CommandResult Success { get; } = new CommandResult { CommandStatus = CommandStatus.Success };
        /// <summary>
        /// Creates an <see cref="CommandResult"/> indicating a failed command operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="CommandError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="CommandResult"/> indicating a failed command operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static CommandResult Failed(params CommandError[] errors)
        {
            var result = new CommandResult { CommandStatus = CommandStatus.Failed };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }
        /// <summary>
        /// Creates an <see cref="CommandResult"/> indicating a timeouted command operation.
        /// </summary>
        /// <returns>An <see cref="CommandResult"/> indicating a timeouted command operation.</returns>
        public static CommandResult Timeouted()
        {
            var result = new CommandResult { CommandStatus = CommandStatus.Timeout };

            return result;
        }
        /// <summary>
        /// Creates an <see cref="CommandResult"/> indicating a nothing changed command operation.
        /// </summary>
        /// <returns>An <see cref="CommandResult"/> indicating a nothing changed command operation.</returns>
        public static CommandResult NothingChanged()
        {
            var result = new CommandResult { CommandStatus = CommandStatus.NothingChanged };

            return result;
        }
        /// <summary>
        /// Converts the value of the current <see cref="CommandResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="CommandResult"/> object.</returns>
        public override string ToString()
        {
            switch (CommandStatus)
            {
                case CommandStatus.Failed:
                    return
                        $"CommandResult [CommandStatus: Failed, Errors: {string.Join(",", Errors.Select(x => x.Code).ToList())} ]";
                default:
                    return
                        $"CommandResult [CommandStatus: {Enum.GetName(typeof(CommandStatus), CommandStatus)}]";
            }
        }
    }
}
