using System;

namespace Domain.Exceptions
{
    public class PayPalRedirectNoSuccessResponseException : Exception
    {
        public PayPalRedirectNoSuccessResponseException() { }
        public PayPalRedirectNoSuccessResponseException(string message) : base(message) { }
    }
}
