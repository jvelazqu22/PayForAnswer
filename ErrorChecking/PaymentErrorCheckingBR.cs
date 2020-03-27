using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Helper;
using log4net;
using Repository.Interfaces;
using System;

namespace ErrorChecking
{
    public class PaymentErrorCheckingBR
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public QuestionPaymentDetail CheckForErrorsAndProceedIfThereAreNoExceptions(PayPalResponse payPalResponse, IPaymentRepository paymentRepository, ref Error error)
        {
            // Check if question id was provided
            if (string.IsNullOrEmpty(payPalResponse.Custom))
            {
                var msg = string.Format(Errors.PAYPAL_RESPONSE_MISSING_REQUEST_ID_MSG, payPalResponse.UnformattedResponse);
                log.Error(msg);
                error.ErrorType = StatusValues.PayPalResponseMissingPaymentID;
                error.Message = msg;
                error.ErrorFound = true;
            }

            long paymentId = Convert.ToInt64(payPalResponse.Custom);
            QuestionPaymentDetail paymentModel = paymentRepository.GetPaymentDetailByPaymentID(paymentId);
            if (paymentModel == null)
            {
                var msg = string.Format(Errors.PAYMENT_NOT_FOUND_ERROR_MSG, paymentId.ToString(), payPalResponse.UnformattedResponse);
                log.Error(msg);
                error.ErrorType = StatusValues.PayPalResponsePaymentIDNotFound;
                error.Message = msg;
                error.ErrorFound = true;
            }

            if (paymentModel.Payment.Total > Math.Round(Convert.ToDecimal(payPalResponse.GrossTotal), 2))
            {
                var msg = string.Format(Errors.PAYMENT_TOTAL_AND_PAYPAL_PAYMENT_MISS_MATCH_MSG,
                        paymentModel.QuestionPaymentDetailID.ToString(), paymentModel.Payment.Total.ToString(), payPalResponse.GrossTotal.ToString(), payPalResponse.UnformattedResponse);
                log.Error(msg);
                error.ErrorType = StatusValues.PayPaylResponseTotalIsLessThanDBValue;
                error.Message = msg;
                error.ErrorFound = true;
            }

            if (paymentModel.Payment.Total < Math.Round(Convert.ToDecimal(payPalResponse.GrossTotal), 2))
            {
                var msg = string.Format(Errors.PAYMENT_TOTAL_AND_PAYPAL_PAYMENT_MISS_MATCH_MSG,
                        paymentModel.QuestionPaymentDetailID.ToString(), paymentModel.Payment.Total.ToString(), payPalResponse.GrossTotal.ToString(), payPalResponse.UnformattedResponse);
                error.ErrorType = StatusValues.PayPalResponseTotalIsGreaterThanDBValue;
                error.Message = msg;
                error.ErrorFound = true;
                log.Warn(msg);
            }

            if (payPalResponse.PaymentStatus.ToLower() != StatusValues.PayPalCompletedPaymentStatus.ToLower())
            {
                var msg = string.Format(Errors.PAYPAL_PAYMENT_STATUS_NOT_COMPLETE_YET, paymentModel.QuestionPaymentDetailID.ToString(), payPalResponse.PaymentStatus, payPalResponse.UnformattedResponse);
                log.Error(msg);
                error.ErrorType = StatusValues.PayPaylResponseInvalidPaymentStatus;
                error.Message = msg;
                error.ErrorFound = true;
            }

            return paymentModel;
        }

        public bool CanTheRequestBeMarkedAsPaymentReceivedForTheFirstTime(QuestionPaymentDetail questionPaymentDetail, PayPalModel payPalModel)
        {
            return !StatusList.CONFIRM_PAYMENT_STATUS.Contains(Convert.ToInt16(questionPaymentDetail.Payment.StatusId))
                    && StatusList.CONFIRM_PAYMENT_STATUS.Contains(payPalModel.RequestStatus);
        }

    }
}
