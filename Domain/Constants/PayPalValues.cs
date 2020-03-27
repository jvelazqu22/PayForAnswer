
namespace Domain.Constants
{
    public static class PayPalValues
    {
        /// <summary>
        /// USA Dollar currency code
        /// </summary>
        public const string US_CURRENCY_CODE = "USD";

        /// <summary>
        /// Payee method identification
        /// </summary>
        public const string IDENTIFY_PAYEE_BY_EMAIL_ADDRESS = "EMAILADDRESS";

        /// <summary>
        /// This is the unique value that contains the answer id where the payment
        /// was attempted. The one at the end of the variable represetns that first payee
        /// in a list of people to make payments to.
        /// </summary>
        public const string MASS_PAY_UNIQUE_ID = "unique_id_1";
        public const string IPN_QUESTION_PAYMENT_TRANSACTION_TYPE_VALUE = "web_accept";
        public const string IPN_ANSWER_PAYMENT_TRANSACTION_TYPE_VALUE = "masspay";
        public const string IPN_PAYPAL_RESPONSE_STATUS_VALUE_VERIFIED = "VERIFIED";
    }
}
