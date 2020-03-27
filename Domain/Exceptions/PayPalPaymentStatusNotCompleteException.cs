using System;

namespace Domain.Exceptions
{
    public class PayPalPaymentStatusNotCompleteException : Exception
    {
        public PayPalPaymentStatusNotCompleteException() { }
        public PayPalPaymentStatusNotCompleteException(string message) : base(message) { }
    }
}
