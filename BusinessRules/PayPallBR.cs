using BusinessRules.Interfaces;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.Helper;
using log4net;
using Repository.Interfaces;
using System;
using System.Linq;
using System.Web;

namespace BusinessRules
{
    public class PayPalBR
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RedirectFromPayPalViewModel ProcessRedirectFromPayPal(string strResponse, IPaymentRepository paymentRepository, IEmailBR emailBr, IQueueRepository queueRepository)
        {
            // Call the business rules to validate payment
            PayPalModel payPalModel = new PayPalModel() { RequestStatus = StatusValues.PayPalRedirectConfirmed };
            payPalModel.PayPalResponse = new PayPalBR().Parse(strResponse, ParsingDelimitedCharacters.PayPalRedirectResponseDelimiter, StatusValues.PayPalRedirectConfirmed);

            Error error = new Error() { ErrorFound = false };
            QuestionPaymentDetail paymentDetailModel = new PaymentBR().ValidateAndSaveQuestionPayment(payPalModel, paymentRepository, emailBr, ref error, queueRepository);
            return new RedirectFromPayPalViewModel() { Title = paymentDetailModel.Question.Title, Total = paymentDetailModel.Payment.Total, CreatedOn = paymentDetailModel.CreatedOn, Error = error };
        }

        public void ProcessNotifyFromPayPal(string strRequest, string strResponseStatus, IPaymentRepository paymentRepository, IAnswerRepository answerRepository, IEmailBR emailBr, IQueueRepository queueRepository)
        {
            if (strResponseStatus == PayPalValues.IPN_PAYPAL_RESPONSE_STATUS_VALUE_VERIFIED)
            {
                PayPalResponse payPalResponse = new PayPalBR().Parse(strRequest, ParsingDelimitedCharacters.PayPalNotifyIPNResponseDelimiter, StatusValues.PayPalIPNNotifyConfirmed);
                if (payPalResponse.TransactionType == PayPalValues.IPN_QUESTION_PAYMENT_TRANSACTION_TYPE_VALUE)
                {
                    // Check that txn_id has not been previously processed. Check that receiver_email is your 
                    // Primary PayPal email. Check that payment_amount/payment_currency are correct process payment.
                    PayPalModel payPalModel = new PayPalModel() { RequestStatus = StatusValues.PayPalIPNNotifyConfirmed, PayPalResponse = payPalResponse };
                    Error error = new Error() { ErrorFound = false };
                    new PaymentBR().ValidateAndSaveQuestionPayment(payPalModel, paymentRepository, emailBr, ref error, queueRepository);
                }
                else if (payPalResponse.TransactionType == PayPalValues.IPN_ANSWER_PAYMENT_TRANSACTION_TYPE_VALUE)
                {
                    long answerId = Convert.ToInt64(payPalResponse.MassPayUniqueId);
                    new AnswerBR().MarkAnswerAsPaidAndSendEmail(answerId, answerRepository, new EmailBR());
                }
                else
                {
                    log.Error(strRequest);
                    var msg = string.Format(Errors.UNEXPECTED_IPN_TRANSACTION_TYPE_RESPONSE_MSG, payPalResponse.TransactionType, strRequest);
                    new EmailBR().SendEmail(Emails.ReportErrorsEmailAddress, "Unexpected PayPal IPN TransactionType", msg);
                    throw new NotifyFromPayPalException(msg);
                }
            }
            else //strResponseStatus == "INVALID" or something else
            {
                log.Error(strRequest);
                var msg = string.Format(Errors.INVALID_IPN_RESPONSE_STATUS_MSG, strResponseStatus, strRequest);
                new EmailBR().SendEmail(Emails.ReportErrorsEmailAddress, "Invalid PayPal IPN Response Status", msg);
                throw new NotifyFromPayPalException(msg);
            }
        }

        public PayPalResponse Parse(string postData, char delimiter, int PayPalNotificationType)
        {
            // If response was SUCCESS, parse response string and output details
            // TODO: research possible values that Response can start with?
            if (StatusValues.PayPalRedirectConfirmed == PayPalNotificationType && !postData.StartsWith("SUCCESS"))
                throw new PayPalRedirectNoSuccessResponseException(string.Format(Errors.QUESTION_PAYPAL_NO_SUCCESS_IN_RESPONSE_MSG, postData));

            String sKey, sValue;
            PayPalResponse payPalResponse = new PayPalResponse() { UnformattedResponse = postData };

            String[] StringArray = postData.Split(delimiter);

            // NOTE:
            /* In some cases the loop is set to start at 1 rather than 0 because the first string 
             * in the array will be single the word SUCCESS or FAIL used to verify post data
            */

            int i;
            int startingIndex = postData.StartsWith("SUCCESS") ? 1 : 0;
            for (i = startingIndex; i < StringArray.Length - 1; i++)
            {
                if (!StringArray[i].Contains('='))
                    throw new Exception("");

                String[] StringArray1 = StringArray[i].Split('=');

                sKey = StringArray1[0];
                sValue = HttpUtility.UrlDecode(StringArray1[1]);

                switch (sKey)
                {
                    case "mc_gross":
                        payPalResponse.GrossTotal = Convert.ToDouble(sValue);
                        break;

                    case "invoice":
                        payPalResponse.InvoiceNumber = Convert.ToInt32(sValue);
                        break;

                    case "payment_status":
                        payPalResponse.PaymentStatus = Convert.ToString(sValue);
                        break;

                    case "first_name":
                        payPalResponse.PayerFirstName = Convert.ToString(sValue);
                        break;

                    case "mc_fee":
                        payPalResponse.PaymentFee = Convert.ToDouble(sValue);
                        break;

                    case "business":
                        payPalResponse.BusinessEmail = Convert.ToString(sValue);
                        break;

                    case "payer_email":
                        payPalResponse.PayerEmail = Convert.ToString(sValue);
                        break;

                    case "Tx Token":
                        payPalResponse.TxToken = Convert.ToString(sValue);
                        break;

                    case "last_name":
                        payPalResponse.PayerLastName = Convert.ToString(sValue);
                        break;

                    case "receiver_email":
                        payPalResponse.ReceiverEmail = Convert.ToString(sValue);
                        break;

                    case "item_name":
                        payPalResponse.ItemName = Convert.ToString(sValue);
                        break;

                    case "mc_currency":
                        payPalResponse.Currency = Convert.ToString(sValue);
                        break;

                    case "txn_id":
                        payPalResponse.TransactionId = Convert.ToString(sValue);
                        break;

                    case "custom":
                        payPalResponse.Custom = Convert.ToString(sValue);
                        break;

                    case "subscr_id":
                        payPalResponse.SubscriberId = Convert.ToString(sValue);
                        break;

                    case "txn_type":
                        payPalResponse.TransactionType = Convert.ToString(sValue);
                        break;

                    case "unique_id_1":
                        payPalResponse.MassPayUniqueId = Convert.ToString(sValue);
                        break;
                }
            }

            return payPalResponse;
        }
    }
}
