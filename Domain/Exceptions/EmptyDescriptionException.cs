using System;

namespace Domain.Exceptions
{
    public class EmptyDescriptionException : Exception
    {
        public EmptyDescriptionException() { }
        public EmptyDescriptionException(string message) : base(message) { }
    }
}
