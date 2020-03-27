using BusinessRules;
using BusinessRules.Interfaces;
using Domain.Constants;
using Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;

namespace BusinessRulesTest
{
    /// <summary>
    /// Summary description for PayPalBRTest
    /// </summary>
    [TestClass]
    public class PayPalBRTest
    {

        [TestMethod]
        [ExpectedException(typeof(NotifyFromPayPalException))]
        public void ProcessNotifyFromPayPal_NotifiedFromPayPalResponseIsNotVerified_ThrowException()
        {
            // Arrange
            string strRequest = string.Empty;
            string strResponseStatus = string.Empty;
            var paymentRepository = new Mock<IPaymentRepository>();
            var answerRepository = new Mock<IAnswerRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();

            // Act
            new PayPalBR().ProcessNotifyFromPayPal(strRequest, strResponseStatus, paymentRepository.Object, answerRepository.Object, emailBR.Object, queueRepository.Object);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(NotifyFromPayPalException))]
        public void ProcessNotifyFromPayPal_NotifiedFromPayPalTransactionTypeIsUnexpected_ThrowException()
        {
            // Arrange
            string strResponseStatus = PayPalValues.IPN_PAYPAL_RESPONSE_STATUS_VALUE_VERIFIED;

            var paymentRepository = new Mock<IPaymentRepository>();
            var answerRepository = new Mock<IAnswerRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();
            string strRequest = "SUCCESS mc_gross=7.08 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=Completed charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom= "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=madeupValue item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";

            // Act
            new PayPalBR().ProcessNotifyFromPayPal(strRequest, strResponseStatus, paymentRepository.Object, answerRepository.Object, emailBR.Object, queueRepository.Object);

            // Assert
        }
    }
}
