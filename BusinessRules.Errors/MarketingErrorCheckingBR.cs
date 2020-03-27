using Domain.Exceptions;
using Domain.Models;
using System;

namespace BusinessRules.Errors
{
    public class MarketingErrorCheckingBR
    {
        public void ValidateUser(QuestionPaymentDetail questionPaymentDetail, int currentUserId)
        {
            if (questionPaymentDetail == null)
                throw new Exception("questionPaymentDetail == null");

            if (questionPaymentDetail.Question.UserId != currentUserId)
                throw new InvalidUserException("Question.UserId: " + questionPaymentDetail.Question.UserId + " currentUserId: " + currentUserId);
        }
    }
}
