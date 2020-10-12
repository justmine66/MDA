using System;

namespace MDA.Domain.Models
{
    public class MethodNotFoundException : Exception
    {
        public MethodNotFoundException(string message) : base(message) { }
    }
}
