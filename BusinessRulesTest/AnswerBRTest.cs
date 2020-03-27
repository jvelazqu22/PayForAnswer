using BusinessRules;
using BusinessRules.Interfaces;
using Domain.Constants;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;
using System;

namespace BusinessRulesTest
{
    /// <summary>
    /// Summary description for AnswerBRTest
    /// </summary>
    [TestClass]
    public class AnswerBRTest
    {
        // MethodName_StateUnderTest_ExpectedBehavior
        // Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()

        [TestMethod]
        public void SaveNewAnswerAndSendEmail_SuccessData1_SaveNewAnswer()
        {
            // Arrange
            ApplicationUser user = new ApplicationUser() { Id = 1, Notifications = new Notifications() };
            int userId = 1;
            Answer answerModel = new Answer();
            AnswerBR answerBr = new AnswerBR();
            Question questionModel = new Question();
            var answerRepository = new Mock<IAnswerRepository>();
            var emailBR = new Mock<IEmailBR>();
            var blobRepository = new Mock<IBlobRepository>();

            NewAnswerViewModel newAnswerViewModel = new NewAnswerViewModel() 
            { 
                QuestionId = new Guid("02FA647C-AD54-4BCC-A860-E5A2664B019D"), NewPostedAnswer = "some answer", QuestionTitle = "some title"
            };

            // Act
            answerModel = answerBr.SaveNewAnswerAndSendEmail(user, newAnswerViewModel, answerRepository.Object, emailBR.Object, blobRepository.Object);

            // Assert
            Assert.IsTrue(answerModel.QuestionId == newAnswerViewModel.QuestionId);
            Assert.IsTrue(answerModel.StatusId == StatusValues.AnswerSubmitted);
            Assert.IsTrue(answerModel.UserId == userId);
            Assert.IsFalse(answerModel.CreatedOn == new DateTime());
        }

        [TestMethod]
        public void GetAnswerToPay_AnswerFound_GetAnswerToPay()
        {
            // Arrange
            int answerId = 1;
            ApplicationUser userProfile = new ApplicationUser() { Email = "jvelazqu24@hotmail.com }" };
            Question questionModel = new Question() { Amount = Convert.ToDecimal(5.00) };
            Answer answerModel = new Answer() { Id = 1, User = userProfile, Question = questionModel };
            AnswerBR answerBr = new AnswerBR();
            var answerRepository = new Mock<IAnswerRepository>();

            answerRepository.Setup(ar => ar.GetAnswerByID(1)).Returns(answerModel);

            // Act
            answerModel = answerBr.GetAnswerToPay(answerId, answerRepository.Object);

            // Assert
            Assert.IsTrue(answerModel.EmailAddressOfUserWhoPostedAnswer == userProfile.Email);
            Assert.IsTrue(answerModel.QuestionAmount == questionModel.Amount);
        }

        [TestMethod]
        public void UpdateAnswerStatus_UserPosterAnswerHisOwnQuestion_SetAnswerStatusToAcceptedByRequester()
        {
            // Arrange
            int answerId = 1;
            int statusId = StatusValues.Accepted;
            ApplicationUser user1 = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com }" };
            ApplicationUser user2 = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com }" };
            Question questionModel = new Question() { Id = new Guid("02FA647C-AD54-4BCC-A860-E5A2664B019D"), Amount = Convert.ToDecimal(5.00), Title = "some title", UserId = user1.Id };
            Status status = new Status() { DisplayName = "some display name" };
            Answer answerModel = new Answer()
            {
                Id = 1,
                UserId = user2.Id,
                User = user2,
                Question = questionModel,
                StatusId = StatusValues.AnswerSubmitted,
                Status = status
            };
            AnswerBR answerBr = new AnswerBR();
            var answerRepository = new Mock<IAnswerRepository>();

            answerRepository.Setup(ar => ar.GetAnswerByID(1)).Returns(answerModel);

            // Act
            answerModel = answerBr.UpdateAnswerStatus(answerId, statusId, answerRepository.Object);

            // Assert
            Assert.IsTrue(answerModel.StatusId == StatusValues.AcceptedByRequester);
            Assert.IsFalse(answerModel.SendEmailToWinner);
        }

        [TestMethod]
        public void UpdateAnswerStatus_UserPosterDidNotAnswerHisOwnQuestion_SetAnswerStatusToAccepted()
        {
            // Arrange
            int answerId = 1;
            int statusId = StatusValues.Accepted;
            ApplicationUser user1 = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com }" };
            ApplicationUser user2 = new ApplicationUser() { Id = 2, Email = "jvelazqu24@hotmail.com }" };
            Question questionModel = new Question() { Id = new Guid("02FA647C-AD54-4BCC-A860-E5A2664B019D"), Amount = Convert.ToDecimal(5.00), Title = "some title", UserId = user1.Id };
            Status status = new Status() { DisplayName = "some display name" };
            Answer answerModel = new Answer()
            {
                Id = 1,
                UserId = user2.Id,
                User = user2,
                Question = questionModel,
                StatusId = StatusValues.AnswerSubmitted,
                Status = status
            };
            AnswerBR answerBr = new AnswerBR();
            var answerRepository = new Mock<IAnswerRepository>();

            answerRepository.Setup(ar => ar.GetAnswerByID(1)).Returns(answerModel);

            // Act
            answerModel = answerBr.UpdateAnswerStatus(answerId, statusId, answerRepository.Object);

            // Assert
            Assert.IsTrue(answerModel.StatusId == StatusValues.Accepted);
            Assert.IsFalse(answerModel.SendEmailToWinner);
        }

        [TestMethod]
        public void UpdateAnswerStatus_AcceptedAnswer_SetAnswerStatusToAnswerPaidAndQuestionPaid()
        {
            // Arrange
            int answerId = 1;
            int statusId = StatusValues.Paid;
            ApplicationUser user1 = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com }" };
            ApplicationUser user2 = new ApplicationUser() { Id = 2, Email = "jvelazqu24@hotmail.com }" };
            Question questionModel = new Question()
            {
                Id = new Guid("02FA647C-AD54-4BCC-A860-E5A2664B019D"),
                Amount = Convert.ToDecimal(5.00),
                Title = "some title",
                UserId = user1.Id,
                StatusId = StatusValues.PayPalRedirectConfirmed
            };
            Status status = new Status() { DisplayName = "some display name" };
            Answer answerModel = new Answer()
            {
                Id = 1,
                UserId = user2.Id,
                User = user2,
                Question = questionModel,
                StatusId = StatusValues.Accepted,
                Status = status,
                Payment = new Payment()
            };
            AnswerBR answerBr = new AnswerBR();
            var answerRepository = new Mock<IAnswerRepository>();

            answerRepository.Setup(ar => ar.GetAnswerByID(1)).Returns(answerModel);

            // Act
            answerModel = answerBr.UpdateAnswerStatus(answerId, statusId, answerRepository.Object);

            // Assert
            Assert.IsTrue(answerModel.StatusId == StatusValues.Paid);
            Assert.IsTrue(answerModel.Question.StatusId == StatusValues.Paid);
            Assert.IsTrue(answerModel.SendEmailToWinner);
        }
        
        [TestMethod]
        public void UpdateAnswerStatus_PayAnswerASecondTime_DoNotUpdateAnswerStatusToAnswerPaidASecondTime()
        {
            // Arrange
            int answerId = 1;
            int statusId = StatusValues.Paid;
            ApplicationUser user1 = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com }" };
            ApplicationUser user2 = new ApplicationUser() { Id = 2, Email = "jvelazqu24@hotmail.com }" };
            Question questionModel = new Question() { Id = new Guid("02FA647C-AD54-4BCC-A860-E5A2664B019D"), Amount = Convert.ToDecimal(5.00), Title = "some title", UserId = user1.Id };
            Status status = new Status() { DisplayName = "some display name" };
            Answer answerModel = new Answer()
            {
                Id = 1,
                UserId = user2.Id,
                User = user2,
                Question = questionModel,
                StatusId = StatusValues.Paid,
                Status = status
            };
            AnswerBR answerBr = new AnswerBR();
            var answerRepository = new Mock<IAnswerRepository>();

            answerRepository.Setup(ar => ar.GetAnswerByID(1)).Returns(answerModel);

            // Act
            answerModel = answerBr.UpdateAnswerStatus(answerId, statusId, answerRepository.Object);

            // Assert
            Assert.IsFalse(answerModel.SendEmailToWinner);
        }

        [TestMethod]
        public void UpdateAnswerStatus_DeclineAnswer_UpdateAnswerStatusToAnswerUnrelated()
        {
            // Arrange
            int answerId = 1;
            int statusId = StatusValues.Reviewed;
            ApplicationUser user1 = new ApplicationUser() { Id = 1, Email = "jvelazqu24@hotmail.com }" };
            ApplicationUser user2 = new ApplicationUser() { Id = 2, Email = "jvelazqu24@hotmail.com }" };
            Question questionModel = new Question() { Id = new Guid("02FA647C-AD54-4BCC-A860-E5A2664B019D"), Amount = Convert.ToDecimal(5.00), Title = "some title", UserId = user1.Id };
            Status status = new Status() { DisplayName = "some display name" };
            Answer answerModel = new Answer()
            {
                Id = 1,
                UserId = user2.Id,
                User = user2,
                Question = questionModel,
                StatusId = StatusValues.AnswerSubmitted,
                Status = status
            };
            AnswerBR answerBr = new AnswerBR();
            var answerRepository = new Mock<IAnswerRepository>();

            answerRepository.Setup(ar => ar.GetAnswerByID(1)).Returns(answerModel);

            // Act
            answerModel = answerBr.UpdateAnswerStatus(answerId, statusId, answerRepository.Object);

            // Assert
            Assert.IsTrue(answerModel.StatusId == StatusValues.Reviewed);
            Assert.IsFalse(answerModel.SendEmailToWinner);
        }

    }
}
