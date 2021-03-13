namespace MDA.Domain.Exceptions
{
    public class DomainExceptionOptions
    {
        public string Topic { get; set; } = DomainDefaults.Topics.Exception;
    }
}