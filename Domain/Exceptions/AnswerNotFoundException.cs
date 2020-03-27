using System;

namespace Domain.Exceptions
{
    public class AnswerNotFoundException : Exception
    {
        public AnswerNotFoundException() { }
        public AnswerNotFoundException(string message) : base(message) { }
    }
}
