
using Domain.Constants;
using Domain.Models.Entities;
using System.Collections.Generic;

namespace BusinessRules
{
    public class UpdateQuestionBR
    {
        public bool CanTheQuestionBeUpdated(Question question, int currentUserID, List<string> currentUserRoles)
        {
            bool isUserInRoles = new UserBR().IsUserInAllowedRoles(currentUserRoles, new List<string>() { Role.Admin });

            if (question.UserId != currentUserID && !isUserInRoles)
                return false;

            if (StatusList.CONFIRM_PAYMENT_STATUS.Contains((int)question.StatusId))
                return true;

            if (question.StatusId == StatusValues.QuestionRequested || question.StatusId == StatusValues.WaitingForPaymentNotification)
                return true;

            return false;
        }
    }
}
