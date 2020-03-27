using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRules.Errors
{
    public class PaymentErrorCheckingBR
    {
        public QuestionPaymentDetail CheckForErrorsAndProceedIfThereAreNoExceptions(PayPalResponse payPalResponse, IPaymentRepository paymentRepository)
        {
            // Check if question id was provided
            if (string.IsNullOrEmpty(payPalResponse.Custom))
                throw new RequestIdMissingInPayPalResponseException(string.Format(Errors.PAYPAL_RESPONSE_MISSING_REQUEST_ID_MSG, payPalResponse.UnformattedResponse));

            long paymentId = Convert.ToInt64(payPalResponse.Custom);
            QuestionPaymentDetail paymentModel = paymentRepository.GetPaymentDetailByPaymentID(paymentId);
            if (paymentModel == null)
                throw new RequestNotFoundException(string.Format(Errors.PAYMENT_NOT_FOUND_ERROR_MSG, paymentId.ToString(), payPalResponse.UnformattedResponse));

            if (paymentModel.Payment.Total != Math.Round(Convert.ToDecimal(payPalResponse.GrossTotal), 2))
                throw new RequestTotalAndPayPalGrossTotalMissmatchException(string.Format(Errors.PAYMENT_TOTAL_AND_PAYPAL_PAYMENT_MISS_MATCH_MSG,
                        paymentModel.QuestionPaymentDetailID.ToString(), paymentModel.Payment.Total.ToString(), payPalResponse.GrossTotal.ToString(), payPalResponse.UnformattedResponse));

            if (payPalResponse.PaymentStatus.ToLower() != StatusValues.PayPalCompletedPaymentStatus.ToLower())
                throw new PayPalPaymentStatusNotCompleteException(
                    string.Format(Errors.PAYPAL_PAYMENT_STATUS_NOT_COMPLETE_YET, paymentModel.QuestionPaymentDetailID.ToString(), payPalResponse.PaymentStatus, payPalResponse.UnformattedResponse));

            return paymentModel;
        }

        public bool CanTheRequestBeMarkedAsPaymentReceivedForTheFirstTime(QuestionPaymentDetail questionPaymentDetail, PayPalModel payPalModel)
        {
            return !StatusList.CONFIRM_PAYMENT_STATUS.Contains(Convert.ToInt16(questionPaymentDetail.Payment.StatusId))
                    && StatusList.CONFIRM_PAYMENT_STATUS.Contains(payPalModel.RequestStatus);
        }

    }
}
