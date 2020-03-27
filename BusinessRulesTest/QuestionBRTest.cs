namespace BusinessRulesTest
{
    using BusinessRules;
    using Domain.Constants;
    using Domain.Models;
    using Domain.Models.Entities;
    using ErrorChecking;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Repository.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class QuestionBRTest
    {
        // MethodName_StateUnderTest_ExpectedBehavior
        // Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()
        
        [TestMethod]
        public void CreateQuestion_AllSubjectsExistedInDbAnd_StatusIsSubmitted()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Question questionModel = new Question();
            var questionSubjectRepository = new Mock<IQuestionSubjectRepository>();
            var blobRepository = new Mock<IBlobRepository>();
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel()
            {
                Title = "Is there a c# function to calculate the square root of a number?",
                Amount = 5,
                MarketingBudgetPerDay = 5,
                NumberOfCampaignDays = 5,
                CommaDelimitedSubjects = "Math,Physics,Computer-Science",
                Description = "Is there a c# function to calculate the square root of a number?",
                UserId = 1,
                UserName = "jvelazquez1",
            };

            List<Subject> questionSubjects = new List<Subject>
                {
                    new Subject { Id = 1, SubjectName = "Math" }, //1
                    new Subject { Id = 2, SubjectName = "Physics" }, //4
                    new Subject { Id = 3, SubjectName = "Computer-Science" }, //7
                };

            questionSubjectRepository.Setup(sr => sr.GetSubjectBySubjectName("Math")).Returns(questionSubjects[0]);
            questionSubjectRepository.Setup(sr => sr.GetSubjectBySubjectName("Physics")).Returns(questionSubjects[1]);
            questionSubjectRepository.Setup(sr => sr.GetSubjectBySubjectName("Computer-Science")).Returns(questionSubjects[2]);

            // Act
            questionModel = questionBr.CreateQuestion(questionViewModel, questionSubjectRepository.Object, blobRepository.Object);

            // Assert
            Assert.IsTrue(questionModel.StatusId == StatusValues.QuestionRequested);
            Assert.IsTrue(questionModel.Subjects.Count == 3);
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == questionSubjects[0].Id && s.SubjectName == questionSubjects[0].SubjectName));
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == questionSubjects[1].Id && s.SubjectName == questionSubjects[1].SubjectName));
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == questionSubjects[2].Id && s.SubjectName == questionSubjects[2].SubjectName));
        }

        [TestMethod]
        public void CreateQuestion_SomeSubjectsExistedInDbAndSomeDoNot_StatusIsSubmitted()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Question questionModel = new Question();
            var questionSubjectRepository = new Mock<IQuestionSubjectRepository>();
            var blobRepository = new Mock<IBlobRepository>();
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel()
            {
                Title = "Is there a c# function to calculate the square root of a number?",
                Amount = 5,
                MarketingBudgetPerDay = 5,
                NumberOfCampaignDays = 5,
                CommaDelimitedSubjects = "Math,Physics,Computer-Science",
                Description = "Is there a c# function to calculate the square root of a number?",
                UserId = 1,
                UserName = "jvelazquez1",
            };

            List<Subject> questionSubjects = new List<Subject>
                {
                    new Subject { Id = 1, SubjectName = "Math" }, //1
                    new Subject { Id = 2, SubjectName = "Physics" }, //4
                    new Subject { Id = 3, SubjectName = "Computer-Science" }, //7
                };

            questionSubjectRepository.Setup(sr => sr.GetSubjectBySubjectName("Physics")).Returns(questionSubjects[1]);


            // Act
            questionModel = questionBr.CreateQuestion(questionViewModel, questionSubjectRepository.Object, blobRepository.Object);

            // Assert
            Assert.IsTrue(questionModel.StatusId == StatusValues.QuestionRequested);
            Assert.IsTrue(questionModel.Subjects.Count == 3);
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == 0 && s.SubjectName == questionSubjects[0].SubjectName));
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == questionSubjects[1].Id && s.SubjectName == questionSubjects[1].SubjectName));
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == 0 && s.SubjectName == questionSubjects[2].SubjectName));
        }

        [TestMethod]
        public void CreateQuestion_AllSubjectsDoNotExistedInDbAnd_StatusIsSubmitted()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Question questionModel = new Question();
            var questionSubjectRepository = new Mock<IQuestionSubjectRepository>();
            var blobRepository = new Mock<IBlobRepository>();
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel()
            {
                Title = "Is there a c# function to calculate the square root of a number?",
                Amount = 5,
                MarketingBudgetPerDay = 5,
                NumberOfCampaignDays = 5,
                CommaDelimitedSubjects = "Math,Physics,Computer-Science",
                Description = "Is there a c# function to calculate the square root of a number?",
                UserId = 1,
                UserName = "jvelazquez1",
            };

            List<Subject> questionSubjects = new List<Subject>
                {
                    new Subject { Id = 1, SubjectName = "Math" }, //1
                    new Subject { Id = 2, SubjectName = "Physics" }, //4
                    new Subject { Id = 3, SubjectName = "Computer-Science" }, //7
                };

            // Act
            questionModel = questionBr.CreateQuestion(questionViewModel, questionSubjectRepository.Object, blobRepository.Object);

            // Assert
            Assert.IsTrue(questionModel.StatusId == StatusValues.QuestionRequested);
            Assert.IsTrue(questionModel.Subjects.Count == 3);
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == 0 && s.SubjectName == questionSubjects[0].SubjectName));
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == 0 && s.SubjectName == questionSubjects[1].SubjectName));
            Assert.IsNotNull(questionModel.Subjects.FirstOrDefault(s => s.Id == 0 && s.SubjectName == questionSubjects[2].SubjectName));
        }

        [TestMethod]
        public void CalculateFees_SomeValidValues_ValidateQuestionFeesAndTotals()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Question questionModel = new Question();

            Decimal percentageFee = General.ChargePercentageFee;
            Decimal fixFee = General.ChargeFixFee;
            Decimal minimumQuestionAmount = General.MinimumQuestionAmount;
            Decimal minimumMarketingBudget = General.MinimumMarketingBudgetPerDay;
            int minimumMarketingDays = General.MinimumMarketingDays;

            List<CreateQuestionViewModel> listOfQuestions = new List<CreateQuestionViewModel>
            {
                new CreateQuestionViewModel { Amount = 5, MarketingBudgetPerDay = 2, NumberOfCampaignDays = 3 },
                new CreateQuestionViewModel { Amount = Convert.ToDecimal(1.123), MarketingBudgetPerDay = 2, NumberOfCampaignDays = 2 },
                new CreateQuestionViewModel { Amount = 5, MarketingBudgetPerDay = 3, NumberOfCampaignDays = 3 },
                new CreateQuestionViewModel { Amount = 5, MarketingBudgetPerDay = 5, NumberOfCampaignDays = 3 },
            };

            List<decimal> expectedFees = new List<decimal>
            {
                Math.Round((listOfQuestions[0].Amount * percentageFee) + fixFee,2),
                Math.Round((listOfQuestions[1].Amount * percentageFee) + fixFee,2),
                Math.Round((listOfQuestions[2].Amount * percentageFee) + fixFee,2),
                Math.Round((listOfQuestions[3].Amount * percentageFee) + fixFee,2),
            };
            List<decimal> expectedTotals = new List<decimal>
            {
                Math.Round((listOfQuestions[0].MarketingBudgetPerDay * listOfQuestions[0].NumberOfCampaignDays) + listOfQuestions[0].Amount + expectedFees[0], 2),
                Math.Round((listOfQuestions[1].MarketingBudgetPerDay * listOfQuestions[1].NumberOfCampaignDays) + listOfQuestions[1].Amount + expectedFees[1], 2),
                Math.Round((listOfQuestions[2].MarketingBudgetPerDay * listOfQuestions[2].NumberOfCampaignDays) + listOfQuestions[2].Amount + expectedFees[2], 2),
                Math.Round((listOfQuestions[3].MarketingBudgetPerDay * listOfQuestions[3].NumberOfCampaignDays) + listOfQuestions[3].Amount + expectedFees[3], 2),
            };

            // Act
            for (int i = 0; i < listOfQuestions.Count(); i++)
                listOfQuestions[i] = questionBr.CalculateFees(listOfQuestions[i]);

            // Assert
            Assert.IsTrue(listOfQuestions[0].Fee == expectedFees[0]);
            Assert.IsTrue(listOfQuestions[1].Fee == expectedFees[1]);
            Assert.IsTrue(listOfQuestions[2].Fee == expectedFees[2]);
            Assert.IsTrue(listOfQuestions[3].Fee == expectedFees[3]);

            Assert.IsTrue(listOfQuestions[0].Total == expectedTotals[0]);
            Assert.IsTrue(listOfQuestions[1].Total == expectedTotals[1]);
            Assert.IsTrue(listOfQuestions[2].Total == expectedTotals[2]);
            Assert.IsTrue(listOfQuestions[3].Total == expectedTotals[3]);
        }

        [TestMethod]
        public void PreValidationHttpGet_ValidateQuestionViewModel_ValidateValues()
        {
            // Arrange
            var paymentRepository = new Mock<IPaymentRepository>();
            int currentUserId = 1;
            Question questionModel = new Question()
            #region questionModel { values }
            {
                Id = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.QuestionRequested,
                UserId = 1,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
            };
            #endregion

            QuestionPaymentDetail questionPaymentDetailsModel = new QuestionPaymentDetail()
            #region paymentDetailsModel { values }
            {
                QuestionPaymentDetailID = 2,
                PaymentId = 3,
                Payment = new Payment() { Id = 3, StatusId = StatusValues.PayPalIPNNotifyConfirmed, Total = 12 },
                QuestionAmountBeforeIncrease = 1,
                QuestionId = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D"),
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
                Question = questionModel
            };
            #endregion

            paymentRepository.Setup(pr => pr.GetPaymentDetailByID(1)).Returns(questionPaymentDetailsModel);

            // Act
            new QuestionErrorCheckingBR().ValidateIfQuestionCanBePrevalidated(questionPaymentDetailsModel.Question, currentUserId);
            ValidateQuestionViewModel validateQuestionModel = new PaymentBR().GetValidateQuestionModel(questionPaymentDetailsModel);
            validateQuestionModel.IdOfUserTryingToMakeUpdate = currentUserId;

            // Assert
            Assert.AreEqual(validateQuestionModel.QuestionId, questionPaymentDetailsModel.QuestionId);
            Assert.AreEqual(validateQuestionModel.PaymentId, questionPaymentDetailsModel.PaymentId);
            Assert.AreEqual(validateQuestionModel.Title, questionPaymentDetailsModel.Question.Title);
            Assert.AreEqual(validateQuestionModel.Amount, questionPaymentDetailsModel.QuestionAmountBeforeIncrease);
            Assert.AreEqual(validateQuestionModel.Fee, questionPaymentDetailsModel.Fee);
            Assert.AreEqual(validateQuestionModel.MarketingBudgetPerDay, questionPaymentDetailsModel.MarketingCampaign.PerDayBudget);
            Assert.AreEqual(validateQuestionModel.NumberOfCampaignDays, questionPaymentDetailsModel.MarketingCampaign.NumberOfDaysToRun);
            Assert.AreEqual(validateQuestionModel.TotalMarketingBudget, questionPaymentDetailsModel.TotalMarketingBudget);
            Assert.AreEqual(validateQuestionModel.Total, questionPaymentDetailsModel.Payment.Total);
        }

        [TestMethod]
        public void PreValidationHttpPost_ValidQuestionPaymentDetailId_GetValidQuestionViewModel()
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

            Guid guid1 = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D");
            Question questionModel = new Question()
            #region questionModel { values }
            {
                Id = guid1,
                Title = "What is the square root of -1?",
                //Description = "What is the square root of -1?",
                Amount = 1,
                StatusId = StatusValues.QuestionRequested,
                UserId = 1,
                CreatedOn = DateTime.UtcNow,
                Subjects = new List<Subject>(),
                QuestionPaymentDetails = new List<QuestionPaymentDetail>() { questionPaymentDetails }
            };
            #endregion

            questionPaymentDetails.Question = questionModel;

            ValidateQuestionViewModel validateQuestionModel = new ValidateQuestionViewModel()
            #region validateQuestionModel { values }
            {
                QuestionId = questionPaymentDetails.QuestionId,
                PaymentId = questionPaymentDetails.PaymentId,
                Title = questionPaymentDetails.Question.Title,
                Amount = questionPaymentDetails.QuestionAmountBeforeIncrease,
                Fee = questionPaymentDetails.Fee,
                MarketingBudgetPerDay = questionPaymentDetails.MarketingCampaign.PerDayBudget,
                NumberOfCampaignDays = questionPaymentDetails.MarketingCampaign.NumberOfDaysToRun,
                TotalMarketingBudget = questionPaymentDetails.TotalMarketingBudget,
                Total = questionPaymentDetails.Payment.Total,
                IdOfUserTryingToMakeUpdate = questionModel.UserId
            };
            #endregion

            var questionRepository = new Mock<IQuestionRepository>();
            questionRepository.Setup(q => q.GetQuestionByID(guid1)).Returns(questionModel);

            // Act
            questionModel = new QuestionBR().PrevalidateQuestion(validateQuestionModel, questionRepository.Object);

            // Assert
            Assert.AreEqual(questionModel.StatusId, StatusValues.WaitingForPaymentNotification);
            Assert.AreEqual(questionModel.QuestionPaymentDetails.Where(r => r.PaymentId == validateQuestionModel.PaymentId).FirstOrDefault().Payment.StatusId, 
                StatusValues.WaitingForPaymentNotification);
        }
    }
}
