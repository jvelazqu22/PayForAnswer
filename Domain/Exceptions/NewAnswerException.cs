using System;

namespace Domain.Exceptions
{
    public class NewAnswerException : Exception
    {
        public NewAnswerException() { }
        public NewAnswerException(string message) : base(message) { }
    }
}
