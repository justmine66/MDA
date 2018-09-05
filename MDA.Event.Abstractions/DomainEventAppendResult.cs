namespace MDA.Event.Abstractions
{
    public enum DomainEventAppendResult
    {
        DuplicateEvent = 1,
        DuplicateCommand = 2
    }
}
