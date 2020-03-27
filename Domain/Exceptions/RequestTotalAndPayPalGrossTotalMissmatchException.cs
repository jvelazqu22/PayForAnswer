using System;

namespace Domain.Exceptions
{
    public class RequestTotalAndPayPalGrossTotalMissmatchException : Exception
    {
        public RequestTotalAndPayPalGrossTotalMissmatchException() { }
        public RequestTotalAndPayPalGrossTotalMissmatchException(string message) : base(message) { }
    }
}
