using Domain.Exceptions;
using Domain.Models.Entities;
using System;

namespace ErrorChecking
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
