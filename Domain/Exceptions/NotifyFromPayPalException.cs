using System;

namespace Domain.Exceptions
{
    public class NotifyFromPayPalException : Exception
    {
        public NotifyFromPayPalException() { }
        public NotifyFromPayPalException(string message) : base(message) { }
    }
}
