using BusinessRules;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;
using System;
using System.Configuration;

namespace BusinessRulesTest
{
    /// <summary>
    /// Summary description for QuestionErrorCheckingBRTest
    /// </summary>
    [TestClass]
    public class QuestionErrorCheckingBRTest
    {

        [TestMethod]
        [ExpectedException(typeof(InvalidQuestionAmountException))]
        public void CalculateFees_QuestionAmountIsLessThanMinimum_ThrowException()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Decimal questionMinimum = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmount"]);
            questionMinimum -= (Decimal)0.01;
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel() { Amount = questionMinimum };

            // Act
            questionBr.CalculateFees(questionViewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidQuestionAmountException))]
        public void CalculateFees_QuestionMarketingBudgetPerDayIsLessThanMinimum_ThrowException()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Decimal minimumMarketingBudget = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingBudget"]);
            minimumMarketingBudget -= (Decimal)0.01;
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel() { MarketingBudgetPerDay = minimumMarketingBudget };

            // Act
            questionBr.CalculateFees(questionViewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidQuestionAmountException))]
        public void CalculateFees_NumberOfCampaignDaysIsLessThanMinimum_ThrowException()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Decimal questionMinimum = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmount"]);
            Decimal questionMarketingBudgetMinimum = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingBudget"]);
            int questionMarketingDaysMinimum = Convert.ToInt32(ConfigurationManager.AppSettings["MinimumMarketingDays"]);
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel()
            {
                Amount = questionMinimum,
                MarketingBudgetPerDay = questionMarketingBudgetMinimum,
                NumberOfCampaignDays = --questionMarketingDaysMinimum
            };

            // Act
            questionBr.CalculateFees(questionViewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidQuestionAmountException))]
        public void CalculateFees_QuestionMarketingBudgetIsLessThanMinimum_ThrowException()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            Decimal questionMinimum = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumQuestionAmount"]);
            Decimal questionMarketingBudgetMinimum = Convert.ToDecimal(ConfigurationManager.AppSettings["MinimumMarketingBudget"]);
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel()
            {
                Amount = Convert.ToDecimal(++questionMinimum),
                MarketingBudgetPerDay = Convert.ToDecimal(--questionMarketingBudgetMinimum)
            };

            // Act
            questionBr.CalculateFees(questionViewModel);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyDescriptionException))]
        public void CreateQuestion_QuestionDescriptionIsEmpty_ThrowException()
        {
            // Arrange
            QuestionBR questionBr = new QuestionBR();
            CreateQuestionViewModel questionViewModel = new CreateQuestionViewModel() 
            { 
                Description = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n</html>" 
            };
            var questionSubjectRepository = new Mock<IQuestionSubjectRepository>();
            var blobRepository = new Mock<IBlobRepository>();

            // Act
            questionBr.CreateQuestion(questionViewModel, questionSubjectRepository.Object, blobRepository.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(QuestionPrevalidationException))]
        public void PrevalidateQuestion_InvalidQuestionStatus_ThrowException()
        {
            // Arrange
            Guid guid1 = new Guid("10FA647C-AD54-4BCC-A860-E5A2664B019D");
            QuestionBR questionBr = new QuestionBR();
            Question questionModel = new Question() { Id = guid1, StatusId = StatusValues.PayPalRedirectConfirmed, UserId = 2 };
            ValidateQuestionViewModel validateQuestionModel = new ValidateQuestionViewModel() { QuestionId = guid1, IdOfUserTryingToMakeUpdate = 2 };
            var questionRepository = new Mock<IQuestionRepository>();
            questionRepository.Setup(sr => sr.GetQuestionByID(guid1)).Returns(questionModel);

            // Act
            questionModel = questionBr.PrevalidateQuestion(validateQuestionModel, questionRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(QuestionPrevalidationException))]
        public void PrevalidateQuestion_InvalidUserId_ThrowException()
        {
            // Arrange
            Guid guid1 = new Guid("01FA647C-AD54-4BCC-A860-E5A2664B019D");
            QuestionBR questionBr = new QuestionBR();
            Question questionModel = new Question() { Id = guid1, StatusId = StatusValues.PayPalRedirectConfirmed, UserId = 2 };
            ValidateQuestionViewModel validateQuestionModel = new ValidateQuestionViewModel() { QuestionId = guid1, IdOfUserTryingToMakeUpdate = 3 };
            var questionRepository = new Mock<IQuestionRepository>();
            questionRepository.Setup(sr => sr.GetQuestionByID(guid1)).Returns(questionModel);

            // Act
            questionModel = questionBr.PrevalidateQuestion(validateQuestionModel, questionRepository.Object);
        }

    }
}
