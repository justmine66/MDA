namespace MDA.Command.Abstractions
{
    /// <summary>
    /// Encapsulates an error from an command operation.
    /// </summary>
    public class CommandError
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
