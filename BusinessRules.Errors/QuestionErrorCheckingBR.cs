using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using Repository.Interfaces;
using System;

namespace BusinessRules.Errors
{
    public class QuestionErrorCheckingBR : ErrorCheckingBR
    {
        public void ValidateIfQuestionCanBePrevalidated(Question questionModel, int UserIdOfUserTryingToPrevalidateQuestion)
        {
            string details = "Question id: " + questionModel.Id.ToString() + " - Question status: " + questionModel.StatusId +
                " - Question user id: " + questionModel.UserId + " - Id of user trying to make update: " + UserIdOfUserTryingToPrevalidateQuestion.ToString();

            if ( !(questionModel.StatusId == StatusValues.QuestionRequested || questionModel.StatusId == StatusValues.WaitingForPaymentNotification) )
                throw new QuestionPrevalidationException(details);

            if (UserIdOfUserTryingToPrevalidateQuestion != questionModel.UserId)
                throw new QuestionPrevalidationException(details);
        }
    }
}
