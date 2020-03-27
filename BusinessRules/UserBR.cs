using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace BusinessRules
{
    public class UserBR
    {
        public bool DidUserPostQuestion(int userID, Guid questionID, IQuestionRepository questionRepository)
        {
            var question = questionRepository.GetQuestionByID(questionID);
            if (question == null) return false;
            return question.UserId == userID ? true : false;
        }

        public bool IsUserInAllowedRoles(List<string> userRoles, List<string> allowedRoles)
        {
            return userRoles.Intersect(allowedRoles).Any() ? true : false;
        }

        public bool IsUserEmailAddressFromMicrosoft(string emailAddress)
        {
            MailAddress address = new MailAddress(emailAddress);
            List<string> microsoftEmailAddresses = new List<string>(){"hotmail.com", "outlook.com"};

            return microsoftEmailAddresses.Contains(address.Host) ? true : false;
        }
    }
}
