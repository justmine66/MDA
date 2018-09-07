namespace MDA.Event.Abstractions
{
    public enum DomainEventAppendResult
    {
        None = 1,
        DuplicateEvent = 2,
        DuplicateCommand = 3
    }
}
