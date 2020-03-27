using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessRules;
using Domain.Models;
using ErrorChecking;
using Domain.Models.ViewModel;

namespace BusinessRulesTest
{
    /// <summary>
    /// Summary description for ErrorCheckingBRTest
    /// </summary>
    [TestClass]
    public class ErrorCheckingBRTest
    {
        [TestMethod]
        public void ValidateDescriptionIsEmpty()
        {
            // Arrange
            var questionErrorCheckingBr = new QuestionErrorCheckingBR();
            QuestionDetailsViewModel questionDetailsViewModel = new QuestionDetailsViewModel();
            questionDetailsViewModel.Description = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n</head>\r\n<body>\r\n\r\n</body>\r\n</html>";
            bool isDescriptionEmpty = false;

            // Act
            isDescriptionEmpty = questionErrorCheckingBr.IsDescriptionEmpty(questionDetailsViewModel.Description);

            // Assert
            Assert.IsTrue(isDescriptionEmpty);
        }

        [TestMethod]
        public void ValidateDescriptionIsNotEmpty()
        {
            // Arrange
            var questionErrorCheckingBr = new QuestionErrorCheckingBR();
            QuestionDetailsViewModel questionViewModel = new QuestionDetailsViewModel();
            questionViewModel.Description = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n</head>\r\n<body>\r\n\r\nsomething</body>\r\n</html>";
            bool isDescriptionEmpty = true;

            // Act
            isDescriptionEmpty = questionErrorCheckingBr.IsDescriptionEmpty(questionViewModel.Description);

            // Assert
            Assert.IsFalse(isDescriptionEmpty);
        }

    }
}
