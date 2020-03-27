using BusinessRules;
using Domain.Constants;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Models.Entities.Identity;
using Domain.Models.ViewModel;
using ErrorChecking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Repository.Interfaces;
using System;

namespace BusinessRulesTest
{
    [TestClass]
    public class AnswerErrorCheckingBRTest
    {
        // [UnitOfWork_StateUnderTest_ExpectedBehavior]
        // Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()

        [TestMethod]
        [ExpectedException(typeof(NewAnswerException))]
        public void CheckForErrorsAndProceedIfThereAreNoExceptions_UserIdIsZero_ThrowException()
        {
            // Arrange
            int userId = 0;
            NewAnswerViewModel newAnswerViewModel = new NewAnswerViewModel();

            // Act
            new AnswerErrorCheckingBR().CheckForErrorsAndProceedIfThereAreNoExceptions(userId, newAnswerViewModel);

            // Assert
        }
        
        [TestMethod]
        [ExpectedException(typeof(NewAnswerException))]
        public void CheckForErrorsAndProceedIfThereAreNoExceptions_NewAnswerViewModelIsNull_ThrowException()
        {
            // Arrange
            int userId = 1;

            // Act
            new AnswerErrorCheckingBR().CheckForErrorsAndProceedIfThereAreNoExceptions(userId, null);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyDescriptionException))]
        public void CheckForErrorsAndProceedIfThereAreNoExceptions_AnswerDescriptionIsEmpty_ThrowException()
        {
            // Arrange
            int userId = 1;
            NewAnswerViewModel newAnswerViewModel = new NewAnswerViewModel() { QuestionId = new Guid("02FA647C-AD54-4BCC-A860-E5A2664B019D"), };
            newAnswerViewModel.NewPostedAnswer = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n</html>";

            // Act
            new AnswerErrorCheckingBR().CheckForErrorsAndProceedIfThereAreNoExceptions(userId, newAnswerViewModel);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(AnswerNotFoundException))]
        public void UserUpdateAnswerStatus_AnswerIsNotFound_ThrowException()
        {
            // Arrange
            long answerId = 0;
            int userIdThatChangeAnswerStatus = 1;
            var answerRepository = new Mock<IAnswerRepository>();

            // Act
            new AnswerBR().UserUpdateAnswerStatus(answerId, userIdThatChangeAnswerStatus, StatusValues.Accepted, answerRepository.Object);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(UpdateAnswerStatusAttack))]
        public void UserUpdateAnswerStatus_UpdateAnswerStatusAttack_ThrowException()
        {
            // Arrange
            long answerId = 1;
            int statusId = StatusValues.Accepted;
            int userIdThatChangeAnswerStatus = 2;
            ApplicationUser user1 = new ApplicationUser() { Id = 4, Email = "jvelazqu24@hotmail.com }" };
            ApplicationUser user2 = new ApplicationUser() { Id = 4, Email = "jvelazqu24@hotmail.com }" };
            Question questionModel = new Question() { Id = new Guid("03FA647C-AD54-4BCC-A860-E5A2664B019D"), Amount = Convert.ToDecimal(5.00), Title = "some title", UserId = user1.Id };
            Status status = new Status() { DisplayName = "some display name" };
            Answer answerModel = new Answer()
            {
                Id = 1,
                UserId = user2.Id,
                User = user2,
                Question = questionModel,
                StatusId = StatusValues.Accepted,
                Status = status
            };
            var answerRepository = new Mock<IAnswerRepository>();

            answerRepository.Setup(ar => ar.GetAnswerByID(1)).Returns(answerModel);

            // Act
            new AnswerBR().UserUpdateAnswerStatus(answerId, userIdThatChangeAnswerStatus, statusId, answerRepository.Object);

            // Assert
        }
    }
}
