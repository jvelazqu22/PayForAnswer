
namespace Domain.Constants
{
    public static class Errors
    {
        /// <summary>
        /// Path to error view
        /// </summary>
        public const string PATH_TO_ERROR_VIEW = "~/Views/Shared/Error.cshtml";

        /// <summary>
        /// Prefix to log when an exception occurs.
        /// </summary>
        public const string EXCEPTION_MESSAGE_PREFIX = "Exception: ";

        public const string SELF_CREATED_EXCEPTION_MESSAGE_SUFIX = " Invalid Operation.";
        public const string PAYPAL_RESPONSE_MISSING_REQUEST_ID_MSG = "PayPal response: {0}.";
        public const string QUESTION_NOT_FOUND_ERROR_MSG = "Question id: {0} not found. PayPal response {1}.";
        public const string PAYMENT_NOT_FOUND_ERROR_MSG = "Payment id: {0} not found. PayPal response {1}.";
        public const string QUESTION_TOTAL_AND_PAYMENT_MISS_MATCH_MSG = "Question id: {0}, question total: ${1}, paypal payment ${2}. PayPal response {3}.";
        public const string PAYMENT_TOTAL_AND_PAYPAL_PAYMENT_MISS_MATCH_MSG = "Payment id: {0}, payment total: ${1}, paypal payment ${2}. PayPal response {3}.";
        public const string QUESTION_PAYPAL_PAYMENT_STATUS_NOT_COMPLETE_YET = "Question id: {0}, PayPal Payment status: {1}. PayPal response {2}.";
        public const string PAYPAL_PAYMENT_STATUS_NOT_COMPLETE_YET = "Payment id: {0}, PayPal Payment status: {1}. PayPal response {2}.";
        public const string QUESTION_PAYPAL_NO_SUCCESS_IN_RESPONSE_MSG = "PayPal response: {0}.";
        public const string UNEXPECTED_IPN_TRANSACTION_TYPE_RESPONSE_MSG = "Unexpected IPN Transaction. IPN Transaction type: {0}. IPN Response: {1}";
        public const string INVALID_IPN_RESPONSE_STATUS_MSG = "Invalid IPN Response status. IPN response status: {0}. IPN Response: {1}";
        public const string INVALID_USER_ID_TRIED_TO_UPDATE_ANSWER = "The user who changed the answer status is not the same as the same who posted "
            + "the question! Answer Id: {0}. UserIdThatChangeAnswerStatus: {1}, UserIdThatPostedQuestion: {2}";
        public const string ANSWER_ID = "Answer Id: {0}.";
        public const string INVALID_CHARACTERS_IN_TITLE_MSG = "You have entered an invalid combination of characters in the title. "
            + "Please change your title and try again. Some of the following characters may be causing the issue: {0}";
        public const string SPECIAL_CHARACTERS = "< > % @";

    }

    public static class AddingMoneyErrors
    {
        public const int MIN_INCREASE_Q_AMT_NOT_MET = 1;
        public const int MIN_MARKETING_BUDGET_PER_DAY_NOT_MET = 2;
        public const int MIN_MARKETING_DAYS_NOT_MET = 3;
        public const int MISSING_NUMBER_OF_MARKETING_CAMPAIGN_DAYS = 4;
        public const int MISSING_MARKETING_BUDGET_PER_DAY = 5;
        public const int NO_VALUES_ENTERED = 6;
    }
}
