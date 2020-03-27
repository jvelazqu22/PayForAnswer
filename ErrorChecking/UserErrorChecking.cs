using Domain.App_GlobalResources;
using Domain.Constants;
using Domain.Models.Account;
using Domain.Models.Helper;
using System;
using System.Net.Mail;

namespace ErrorChecking
{
    public class UserErrorChecking
    {
        public Error AreThereAnyRegistrationErrors(RegisterModel model)
        {
            if (!model.TermsAndConditionsAccepted)
                return new Error() { ErrorFound = true, Message = CommonResources.TermsAndConditionsErrorMsg };

            if (IsUserNameAnEmailAddress(model.UserName))
                return new Error() { ErrorFound = true, Message = CommonResources.UserNameCantBeAnEmailAddress };

            //if(string.IsNullOrWhiteSpace(model.FullName))
            //    return new Error() { ErrorFound = true, Message = CommonResources.FullNameErrorMsg };

            try
            {
                var dob = new DateTime(model.Year, model.Month, model.Day);
                DateTime nowMinusMinimumAge = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddYears(-Size.MinimumAgeForRegistration);
                if (dob > nowMinusMinimumAge)
                    return new Error() { ErrorFound = true, Message = string.Format(CommonResources.YouMustBeAtLeasXYearsErrorMsg, Size.MinimumAgeForRegistration) };
            }
            catch (Exception) 
            {
                return new Error() { ErrorFound = true, Message = CommonResources.DoBErrorMsg };
            }

            return new Error() { ErrorFound = false };
        }

        public bool IsUserNameAnEmailAddress(string userName)
        {
            try
            {
                MailAddress m = new MailAddress(userName);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
