using Domain.Constants;
using Domain.Models.Account;
using Domain.Models.Helper;
using ErrorChecking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ErrorChcking.Test
{
    [TestClass]
    public class UserErrorCheckingTest
    {
        // MethodName_StateUnderTest_ExpectedBehavior
        // Public void Sum_NegativeNumberAs1stParam_ExceptionThrown()

        [TestMethod]
        public void AreThereAnyRegistrationErrors_LessThanMinimumAge_ErrorMsg()
        {
            // Arrange
            RegisterModel model = new RegisterModel() { Year = DateTime.UtcNow.Year - (Size.MinimumAgeForRegistration - 1), Month = DateTime.UtcNow.Month, Day = DateTime.UtcNow.Day, TermsAndConditionsAccepted = true, UserName = "jvelazquez24", FullName = "JVO" };
            Error error = null;

            // Act
            error = new UserErrorChecking().AreThereAnyRegistrationErrors(model);

            // Assert
            Assert.IsTrue(error.ErrorFound);
        }

        [TestMethod]
        public void AreThereAnyRegistrationErrors_EqualToMinimumAge_NoErrorMsg()
        {
            // Arrange
            RegisterModel model = new RegisterModel() { Year = DateTime.UtcNow.Year - Size.MinimumAgeForRegistration, Month = DateTime.UtcNow.Month, Day = DateTime.UtcNow.Day, TermsAndConditionsAccepted = true, UserName = "jvelazquez24", FullName = "JVO" };
            Error error = null;

            // Act
            error = new UserErrorChecking().AreThereAnyRegistrationErrors(model);

            // Assert
            Assert.IsFalse(error.ErrorFound);
        }

        [TestMethod]
        public void AreThereAnyRegistrationErrors_GreaterThanMinimumAge_NoErrorMsg()
        {
            // Arrange
            RegisterModel model = new RegisterModel() { Year = DateTime.UtcNow.Year - (Size.MinimumAgeForRegistration + 1), Month = DateTime.UtcNow.Month, Day = DateTime.UtcNow.Day, TermsAndConditionsAccepted = true, UserName = "jvelazquez24", FullName = "JVO" };
            Error error = null;

            // Act
            error = new UserErrorChecking().AreThereAnyRegistrationErrors(model);

            // Assert
            Assert.IsFalse(error.ErrorFound);
        }
    }
}
