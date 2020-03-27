using System;

namespace Domain.Exceptions
{
    public class RequestNotFoundException : Exception
    {
        public RequestNotFoundException() { }
        public RequestNotFoundException(string message) : base(message) { }
    }
}
