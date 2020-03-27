using BusinessRules;
using BusinessRules.Interfaces;
using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessRulesTest
{
    [TestClass]
    public class PaymentBRTest
    {
        [TestMethod]
        public void ValidateAndSaveQuestionPayment_RedirectFromPayPalComesFirst_UpdateStatusStatusSuccessfully()
        {
            // Arrange
            QuestionPaymentDetail questionPaymentDetails = new QuestionPaymentDetail()
            #region paymentDetailsModel { values }
            {
                QuestionId = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                QuestionPaymentDetailID = 2,
                PaymentId = 3,
                Payment = new Payment() { Id = 3, StatusId = StatusValues.WaitingForPaymentNotification, Total = 12 },
                QuestionAmountBeforeIncrease = 1,
                Fee = 1,
                TotalMarketingBudget = 10,
                QuestionAmountIncrease = 0,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = Role.PayForAnswer,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = Role.PayForAnswer,
                Type = QuestionPaymentDetailType.FirstPayment,
                MarketingCampaign = new MarketingCampaign()
                {
                    PerDayBudget = 1,
                    NumberOfDaysToRun = 10,
                    StatusId = CampaignStatus.CampaignReadyToBeStarted,
                    CreatedBy = Role.PayForAnswer,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedBy = Role.PayForAnswer,
                    UpdatedOn = DateTime.UtcNow,
                },
            };
            #endregion

            ApplicationUser userProfile = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com" };
            Question questionModel = new Question()
            #region questionModel { values }
            {
                Id = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.WaitingForPaymentNotification,
                UserId = userProfile.Id,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>() { questionPaymentDetails },
                User = userProfile
            };
            #endregion

            questionPaymentDetails.Question = questionModel;

            PayPalModel payPalModel = new PayPalModel();
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();

            payPalModel.RequestStatus = StatusValues.PayPalRedirectConfirmed;
            string strPayPalResponse = "SUCCESS mc_gross=12.00 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=Completed charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom=3 "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=web_accept item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.UnitTestPayPalRedirectResponseDelimiter,
                                            StatusValues.PayPalRedirectConfirmed);
            paymentRepository.Setup(qr => qr.GetPaymentDetailByPaymentID(questionPaymentDetails.PaymentId)).Returns(questionPaymentDetails);
            Error error = new Error() { ErrorFound = false };
            // Act
            questionPaymentDetails = paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
            Assert.IsTrue(StatusList.CONFIRM_PAYMENT_STATUS.Contains((int)questionPaymentDetails.Payment.StatusId));
            Assert.IsTrue(StatusList.CONFIRM_PAYMENT_STATUS.Contains((int)questionPaymentDetails.Question.StatusId));
            Assert.AreEqual(CampaignStatus.CampaignReadyToBeStarted, (int)questionPaymentDetails.MarketingCampaign.StatusId);
        }


        [TestMethod]
        public void ValidateAndSaveQuestionPayment_PayPalIPNResponseComesFirst_UpdateStatusStatusSuccessfully()
        {
            // Arrange
            QuestionPaymentDetail questionPaymentDetails = new QuestionPaymentDetail()
            #region paymentDetailsModel { values }
            {
                QuestionId = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                QuestionPaymentDetailID = 2,
                PaymentId = 3,
                Payment = new Payment() { Id = 3, StatusId = StatusValues.WaitingForPaymentNotification, Total = 12 },
                QuestionAmountBeforeIncrease = 1,
                Fee = 1,
                TotalMarketingBudget = 10,
                QuestionAmountIncrease = 0,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = Role.PayForAnswer,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = Role.PayForAnswer,
                Type = QuestionPaymentDetailType.FirstPayment,
                MarketingCampaign = new MarketingCampaign()
                {
                    PerDayBudget = 1,
                    NumberOfDaysToRun = 10,
                    StatusId = StatusValues.WaitingForPaymentNotification,
                    CreatedBy = Role.PayForAnswer,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedBy = Role.PayForAnswer,
                    UpdatedOn = DateTime.UtcNow,
                },
            };
            #endregion

            ApplicationUser userProfile = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com" };
            Question questionModel = new Question()
            #region questionModel { values }
            {
                Id = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.WaitingForPaymentNotification,
                UserId = userProfile.Id,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>() { questionPaymentDetails },
                User = userProfile
            };
            #endregion

            questionPaymentDetails.Question = questionModel;

            PayPalModel payPalModel = new PayPalModel();
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();

            payPalModel.RequestStatus = StatusValues.PayPalIPNNotifyConfirmed;
            string strPayPalResponse = "mc_gross=12.00&protection_eligibility=Eligible&address_status=confirmed&payer_id=RE74GSNYL2WTG"
                + "&tax=0.00&address_street=1+Main+St&payment_date=15%3A44%3A31+Jul+09%2C+2013+PDT&payment_status=Completed"
                + "&charset=windows-1252&address_zip=95131&first_name=Ricardo&mc_fee=0.57&address_country_code=US"
                + "&address_name=Ricardo+Velazquez&notify_version=3.7&custom=3&payer_status=verified"
                + "&business=jvelazqu22-facilitator%40gmail.com&address_country=United+States&address_city=San+Jose&quantity=1"
                + "&verify_sign=AFcWxV21C7fd0v3bYYYRCpSSRl31AeezuF5RmIpvcCe0iQMGU0PkfXlE&payer_email=jvelazquez1%40hotmail.com"
                + "&txn_id=6DF868027C6660610&payment_type=instant&last_name=Velazquez&address_state=CA"
                + "&receiver_email=jvelazqu22-facilitator%40gmail.com&payment_fee=0.57&receiver_id=RUB5M4PT53TAJ&txn_type=web_accept"
                + "&item_name=test69&mc_currency=USD&item_number=&residence_country=US&test_ipn=1&handling_amount=0.00"
                + "&transaction_subject=70&payment_gross=9.44&shipping=0.00&ipn_track_id=1d5e4a385faea&cmd=_notify-validate";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.PayPalNotifyIPNResponseDelimiter,
                                            StatusValues.PayPalIPNNotifyConfirmed);
            paymentRepository.Setup(qr => qr.GetPaymentDetailByPaymentID(questionPaymentDetails.PaymentId)).Returns(questionPaymentDetails);
            Error error = new Error() { ErrorFound = false };

            // Act
            questionPaymentDetails = paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
            Assert.AreEqual(StatusValues.PayPalIPNNotifyConfirmed, (int)questionPaymentDetails.Payment.StatusId);
            Assert.AreEqual(StatusValues.PayPalIPNNotifyConfirmed, (int)questionPaymentDetails.Question.StatusId);
            Assert.AreEqual(CampaignStatus.CampaignReadyToBeStarted, (int)questionPaymentDetails.MarketingCampaign.StatusId);
        }

        [TestMethod]
        public void ValidateAndSaveQuestionPayment_PayPalRedirectFirstAndPayPalIPNResponseSecond_UpdateStatusStatusSuccessfully()
        {
            // Arrange
            QuestionPaymentDetail questionPaymentDetails = new QuestionPaymentDetail()
            #region paymentDetailsModel { values }
            {
                QuestionId = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                QuestionPaymentDetailID = 2,
                PaymentId = 3,
                Payment = new Payment() { Id = 3, StatusId = StatusValues.PayPalRedirectConfirmed, Total = 12 },
                QuestionAmountBeforeIncrease = 1,
                Fee = 1,
                TotalMarketingBudget = 10,
                QuestionAmountIncrease = 0,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = Role.PayForAnswer,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = Role.PayForAnswer,
                MarketingCampaign = new MarketingCampaign()
                {
                    PerDayBudget = 1,
                    NumberOfDaysToRun = 10,
                    StatusId = CampaignStatus.CampaignReadyToBeStarted,
                    CreatedBy = Role.PayForAnswer,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedBy = Role.PayForAnswer,
                    UpdatedOn = DateTime.UtcNow,
                },
            };
            #endregion

            ApplicationUser userProfile = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com" };
            Question questionModel = new Question()
            #region questionModel { values }
            {
                Id = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.PayPalRedirectConfirmed,
                UserId = userProfile.Id,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>() { questionPaymentDetails },
                User = userProfile
            };
            #endregion

            questionPaymentDetails.Question = questionModel;

            PayPalModel payPalModel = new PayPalModel();
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();

            payPalModel.RequestStatus = StatusValues.PayPalIPNNotifyConfirmed;
            string strPayPalResponse = "mc_gross=12.00&protection_eligibility=Eligible&address_status=confirmed&payer_id=RE74GSNYL2WTG"
                + "&tax=0.00&address_street=1+Main+St&payment_date=15%3A44%3A31+Jul+09%2C+2013+PDT&payment_status=Completed"
                + "&charset=windows-1252&address_zip=95131&first_name=Ricardo&mc_fee=0.57&address_country_code=US"
                + "&address_name=Ricardo+Velazquez&notify_version=3.7&custom=3&payer_status=verified"
                + "&business=jvelazqu22-facilitator%40gmail.com&address_country=United+States&address_city=San+Jose&quantity=1"
                + "&verify_sign=AFcWxV21C7fd0v3bYYYRCpSSRl31AeezuF5RmIpvcCe0iQMGU0PkfXlE&payer_email=jvelazquez1%40hotmail.com"
                + "&txn_id=6DF868027C6660610&payment_type=instant&last_name=Velazquez&address_state=CA"
                + "&receiver_email=jvelazqu22-facilitator%40gmail.com&payment_fee=0.57&receiver_id=RUB5M4PT53TAJ&txn_type=web_accept"
                + "&item_name=test69&mc_currency=USD&item_number=&residence_country=US&test_ipn=1&handling_amount=0.00"
                + "&transaction_subject=70&payment_gross=9.44&shipping=0.00&ipn_track_id=1d5e4a385faea&cmd=_notify-validate";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.PayPalNotifyIPNResponseDelimiter,
                                            StatusValues.PayPalIPNNotifyConfirmed);
            paymentRepository.Setup(qr => qr.GetPaymentDetailByPaymentID(questionPaymentDetails.PaymentId)).Returns(questionPaymentDetails);
            Error error = new Error() { ErrorFound = false };

            // Act
            questionPaymentDetails = paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
            Assert.AreEqual(StatusValues.PayPalRedirectConfirmed, (int)questionPaymentDetails.Payment.StatusId);
            Assert.AreEqual(StatusValues.PayPalRedirectConfirmed, (int)questionPaymentDetails.Question.StatusId);
            Assert.AreEqual(CampaignStatus.CampaignReadyToBeStarted, (int)questionPaymentDetails.MarketingCampaign.StatusId);
        }

        [TestMethod]
        public void ValidateAndSaveQuestionPayment_PayPalIPNResponseFirstAndPayPalRedirectSecond_NoStatusUpdatesMade()
        {
            // Arrange
            QuestionPaymentDetail questionPaymentDetails = new QuestionPaymentDetail()
            #region paymentDetailsModel { values }
            {
                QuestionId = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                QuestionPaymentDetailID = 2,
                PaymentId = 3,
                Payment = new Payment() { Id = 3, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                QuestionAmountBeforeIncrease = 1,
                Fee = 1,
                TotalMarketingBudget = 10,
                QuestionAmountIncrease = 0,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = Role.PayForAnswer,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = Role.PayForAnswer,
                MarketingCampaign = new MarketingCampaign()
                {
                    PerDayBudget = 1,
                    NumberOfDaysToRun = 10,
                    StatusId = CampaignStatus.CampaignReadyToBeStarted,
                    CreatedBy = Role.PayForAnswer,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedBy = Role.PayForAnswer,
                    UpdatedOn = DateTime.UtcNow,
                },
            };
            #endregion

            ApplicationUser userProfile = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com" };
            Question questionModel = new Question()
            #region questionModel { values }
            {
                Id = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.PayPalIPNNotifyConfirmed,
                UserId = userProfile.Id,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>() { questionPaymentDetails },
                User = userProfile
            };
            #endregion

            questionPaymentDetails.Question = questionModel;

            PayPalModel payPalModel = new PayPalModel();
            PaymentBR paymentBr = new PaymentBR();
            var paymentRepository = new Mock<IPaymentRepository>();
            var queueRepository = new Mock<IQueueRepository>();
            var emailBR = new Mock<IEmailBR>();

            payPalModel.RequestStatus = StatusValues.PayPalRedirectConfirmed;
            string strPayPalResponse = "SUCCESS mc_gross=12.00 protection_eligibility=Eligible address_status=confirmed payer_id=RE74GSNYL2WTG tax=0.00 "
                + "address_street=1+Main+St payment_date=11%3A55%3A37+Jul+09%2C+2013+PDT payment_status=Completed charset=windows-1252 "
                + "address_zip=95131 first_name=Ricardo mc_fee=0.47 address_country_code=US address_name=Ricardo+Velazquez custom=3 "
                + "payer_status=verified business=jvelazqu22-facilitator%40gmail.com address_country=United+States address_city=San+Jose "
                + "quantity=1 payer_email=jvelazquez1%40hotmail.com txn_id=6K934001TA890584R payment_type=instant last_name=Velazquez "
                + "address_state=CA receiver_email=jvelazqu22-facilitator%40gmail.com payment_fee=0.47 receiver_id=RUB5M4PT53TAJ "
                + "txn_type=web_accept item_name=test63 mc_currency=USD item_number= residence_country=US handling_amount=0.00 "
                + "transaction_subject=63 payment_gross=6.00 shipping=0.00";
            payPalModel.PayPalResponse = new PayPalBR().Parse(strPayPalResponse, ParsingDelimitedCharacters.UnitTestPayPalRedirectResponseDelimiter,
                                            StatusValues.PayPalRedirectConfirmed);
            paymentRepository.Setup(qr => qr.GetPaymentDetailByPaymentID(questionPaymentDetails.PaymentId)).Returns(questionPaymentDetails);
            Error error = new Error() { ErrorFound = false };

            // Act
            questionPaymentDetails = paymentBr.ValidateAndSaveQuestionPayment(payPalModel, paymentRepository.Object, emailBR.Object, ref error, queueRepository.Object);

            // Assert
            Assert.AreEqual(StatusValues.PayPalIPNNotifyConfirmed, (int)questionPaymentDetails.Payment.StatusId);
            Assert.AreEqual(StatusValues.PayPalIPNNotifyConfirmed, (int)questionPaymentDetails.Question.StatusId);
            Assert.AreEqual(CampaignStatus.CampaignReadyToBeStarted, (int)questionPaymentDetails.MarketingCampaign.StatusId);
        }

        [TestMethod]
        public void ValidateAndSaveQuestionPayment_PayPalIPNResponseAndPayPalRedirectComeInAtTheSameTime_WhatHappens()
        {
            // Arrange
            // Act
            // Assert
            Assert.Fail();
        }

    }
}
