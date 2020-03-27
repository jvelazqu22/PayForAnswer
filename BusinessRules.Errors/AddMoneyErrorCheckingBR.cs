using Domain.Constants;
using Domain.Exceptions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRules.Errors
{
    public class AddMoneyErrorCheckingBR
    {
        public void CheckErrors(Question questionModel, int currentUserId, long questionId)
        {
            if (questionModel == null) throw new RequestNotFoundException(string.Format("Question id: {0}", questionId));

            if (questionModel == null)
                throw new Exception("questionModel == null");

            if (questionModel.UserId != currentUserId)
                throw new InvalidUserException("Question.UserId: " + questionModel.UserId + " currentUserId: " + currentUserId);
        }

        public void ValidateIfQuestionPaymentDetailCanBePrevalidated(ValidateAddMoneyViewModel validateAddMoneyViewModel)
        {
            string details = "QuestionPaymentDetail id: " + validateAddMoneyViewModel.QuestionPaymentDetailID.ToString() + " - Payment status: " + validateAddMoneyViewModel.PaymentStatusId +
                " Question status: " + validateAddMoneyViewModel.QuestionStatusId + " - Question user id: " + validateAddMoneyViewModel.QuestionUserId + " - Id of user trying to make update: " +
                validateAddMoneyViewModel.IdOfUserTryingToMakeUpdate;
            if (StatusList.CONFIRM_PAYMENT_STATUS.Contains((int)validateAddMoneyViewModel.PaymentStatusId) || validateAddMoneyViewModel.QuestionStatusId == StatusValues.Paid
                || validateAddMoneyViewModel.QuestionStatusId == StatusValues.Accepted || validateAddMoneyViewModel.QuestionStatusId == StatusValues.AcceptedByRequester)
                throw new AddMoneyPrevalidationException(details);

            if (validateAddMoneyViewModel.IdOfUserTryingToMakeUpdate != validateAddMoneyViewModel.QuestionUserId)
                throw new AddMoneyPrevalidationException(details);
        }
    }
}
