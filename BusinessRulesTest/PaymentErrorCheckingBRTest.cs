using BusinessRules;
using BusinessRules.Interfaces;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;
using System;

namespace BusinessRulesTest
{
    [TestClass]
    public class PaymentErrorCheckingBRTest
    {
        [TestMethod]
        [ExpectedException(typeof(RequestIdMissingInPayPalResponseException))]
        public void ValidateAndSaveQuestionPayment_RedirectFromPayPalIsMissingPaymentId_ThrowException()
        {
            // Arrange
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();
            PayPalModel payPalModel = new PayPalModel();
            QuestionPaymentDetail questionPaymentDetailModel = new QuestionPaymentDetail()
            {
                Payment = new Payment() { Id = 71, StatusId = StatusValues.WaitingForPaymentNotification, Total = Convert.ToDecimal(7.08) },
            };

            payPalModel.RequestStatus = StatusValues.PayPalRedirectConfirmed;
            string strPayPalResponse = "SUCCESS mc_gross=7.08 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=Completed charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom= "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=web_accept item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.UnitTestPayPalRedirectResponseDelimiter,
                                            StatusValues.PayPalRedirectConfirmed);

            paymentRepository.Setup(qpdr => qpdr.GetPaymentDetailByPaymentID(71)).Returns(questionPaymentDetailModel);
            Error error = new Error() { ErrorFound = false };
            // Act
            paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(RequestNotFoundException))]
        public void ValidateAndSaveQuestionPayment_RedirectFromPayPalQuestionIdIsNotFoundInDb_ThrowException()
        {
            // Arrange
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();
            PayPalModel payPalModel = new PayPalModel();
            QuestionPaymentDetail questionPaymentDetailModel = new QuestionPaymentDetail()
            {
                Payment = new Payment() { Id = 71, StatusId = StatusValues.WaitingForPaymentNotification, Total = Convert.ToDecimal(7.08) },
            };

            payPalModel.RequestStatus = StatusValues.PayPalRedirectConfirmed;
            string strPayPalResponse = "SUCCESS mc_gross=7.08 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=Completed charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom=0 "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=web_accept item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.UnitTestPayPalRedirectResponseDelimiter,
                                            StatusValues.PayPalRedirectConfirmed);

            paymentRepository.Setup(qpdr => qpdr.GetPaymentDetailByPaymentID(71)).Returns(questionPaymentDetailModel);
            Error error = new Error() { ErrorFound = false };
            // Act
            paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(RequestTotalAndPayPalGrossTotalMissmatchException))]
        public void ValidateAndSaveQuestionPayment_RedirectFromPayPalQuestionTotalAndPayPalGroosTotalDoNotMatch_ThrowException()
        {
            // Arrange
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();
            PayPalModel payPalModel = new PayPalModel();
            QuestionPaymentDetail questionPaymentDetailModel = new QuestionPaymentDetail()
            {
                Payment = new Payment() { Id = 71, StatusId = StatusValues.WaitingForPaymentNotification, Total = Convert.ToDecimal(7.08) },
            };

            payPalModel.RequestStatus = StatusValues.PayPalRedirectConfirmed;
            string strPayPalResponse = "SUCCESS mc_gross=5.08 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=Completed charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom=71 "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=web_accept item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.UnitTestPayPalRedirectResponseDelimiter,
                                            StatusValues.PayPalRedirectConfirmed);

            paymentRepository.Setup(qpdr => qpdr.GetPaymentDetailByPaymentID(71)).Returns(questionPaymentDetailModel);
            Error error = new Error() { ErrorFound = false };

            // Act
            paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(PayPalPaymentStatusNotCompleteException))]
        public void ValidateAndSaveQuestionPayment_RedirectFromPayPalStatusIsNotCompleted_ThrowException()
        {
            // Arrange
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();
            PayPalModel payPalModel = new PayPalModel();
            QuestionPaymentDetail questionPaymentDetailModel = new QuestionPaymentDetail()
            {
                Payment = new Payment() { Id = 71, StatusId = StatusValues.WaitingForPaymentNotification, Total = Convert.ToDecimal(7.08) },
            };

            payPalModel.RequestStatus = StatusValues.PayPalRedirectConfirmed;
            string strPayPalResponse = "SUCCESS mc_gross=7.08 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=NotCompleted charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom=71 "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=web_accept item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.UnitTestPayPalRedirectResponseDelimiter,
                                            StatusValues.PayPalRedirectConfirmed);

            paymentRepository.Setup(qpdr => qpdr.GetPaymentDetailByPaymentID(71)).Returns(questionPaymentDetailModel);
            Error error = new Error() { ErrorFound = false };

            // Act
            paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(PayPalRedirectNoSuccessResponseException))]
        public void ValidateAndSaveQuestionPayment_RedirectFromPayPalResponseNotStartWithSuccess_ThrowException()
        {
            // Arrange
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();
            PayPalModel payPalModel = new PayPalModel();
            QuestionPaymentDetail questionPaymentDetailModel = new QuestionPaymentDetail()
            {
                Payment = new Payment() { Id = 71, StatusId = StatusValues.WaitingForPaymentNotification, Total = Convert.ToDecimal(7.08) },
            };

            payPalModel.RequestStatus = StatusValues.PayPalRedirectConfirmed;
            string strPayPalResponse = " mc_gross=7.08 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=NotCompleted charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom=71 "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=web_accept item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.UnitTestPayPalRedirectResponseDelimiter,
                                            StatusValues.PayPalRedirectConfirmed);

            paymentRepository.Setup(qpdr => qpdr.GetPaymentDetailByPaymentID(71)).Returns(questionPaymentDetailModel);
            Error error = new Error() { ErrorFound = false };

            // Act
            paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
        }


    }
}
