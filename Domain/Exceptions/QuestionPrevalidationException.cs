using System;

namespace Domain.Exceptions
{
    public class QuestionPrevalidationException : Exception
    {
        public QuestionPrevalidationException() { }
        public QuestionPrevalidationException(string message) : base(message) { }
    }
}
