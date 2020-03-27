using System;

namespace Domain.Exceptions
{
    public class InvalidQuestionAmountException : Exception
    {
        public InvalidQuestionAmountException() { }
        public InvalidQuestionAmountException(string message) : base(message) { }
    }
}
