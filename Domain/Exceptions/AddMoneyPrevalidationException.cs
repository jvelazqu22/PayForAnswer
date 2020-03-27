using System;

namespace Domain.Exceptions
{
    public class AddMoneyPrevalidationException : Exception
    {
        public AddMoneyPrevalidationException() { }
        public AddMoneyPrevalidationException(string message) : base(message) { }
    }
}
