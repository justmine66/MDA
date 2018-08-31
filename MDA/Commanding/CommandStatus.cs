namespace MDA.Commanding
{
    /// <summary>
    /// Encapsulates an status from an command operation.
    /// </summary>
    public enum CommandStatus
    {
        None = 0,
        Success = 1,
        NothingChanged = 2,
        Timeout = 3,
        Failed = 4
    }
}
