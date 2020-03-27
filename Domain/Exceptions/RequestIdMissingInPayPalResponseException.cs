using System;

namespace Domain.Exceptions
{
    public class RequestIdMissingInPayPalResponseException : Exception
    {
        public RequestIdMissingInPayPalResponseException() { }
        public RequestIdMissingInPayPalResponseException(string message) : base(message) { }
    }
}
